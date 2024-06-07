using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Newtonsoft.Json;
using Square;
using Square.Authentication;
using Square.Exceptions;
using Square.Models;
using Square.Utilities;
using Stripe.Checkout;
using Address = Square.Models.Address;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;
using Money = Square.Models.Money;

namespace LouiseTieDyeStore.Server.Services.PaymentService
{
    public class SquareService : IPaymentService
    {
        private readonly IConfiguration _config;
        private readonly IOrderService _orderService;
        private readonly DataContext _context;
        private readonly ISquareClient _squareClient;
        private readonly string _locationId;

        public SquareService(IConfiguration config, IOrderService orderService, DataContext context)
        {
            _config = config;
            _orderService = orderService;
            _context = context;

            // TODO: Check for Environment Variable
            string accessToken = System.Environment.GetEnvironmentVariable("SquareKeys_AccessToken")
                ?? _config["SquareKeys:AccessToken"];

            _locationId = System.Environment.GetEnvironmentVariable("SquareKeys_LocationId")
                ?? _config["SquareKeys:LocationId"];

            _squareClient = new SquareClient.Builder()
                .BearerAuthCredentials(
                    new BearerAuthModel.Builder(
                        accessToken
                    )
                .Build())
                .Environment(Square.Environment.Production) // TODO: Change to production
                .Build();
        }
        public async Task<ServiceResponse<string>> CreatePaymentLink(CheckoutDTO checkout)
        {
            Console.WriteLine("!!! Checkout: " + JsonConvert.SerializeObject(checkout));

            var lineItems = new List<OrderLineItem>();

            foreach (var item in checkout.Items)
            {
                long longPrice = (long)(item.Price * 100);

                var price = new Money.Builder()
                    .Amount(longPrice)
                    .Currency("USD")
                    .Build();

                lineItems.Add(new OrderLineItem.Builder(quantity: "1")
                    .Name(item.Title)
                    .BasePriceMoney(price)
                    .Build());
            }

            var taxRate = await _context.TaxRates.FirstOrDefaultAsync(x => x.Abbreviation == checkout.Address.State);
            var orderLineItemTax = new OrderLineItemTax.Builder()
              .Name("Sales Tax")
              .Percentage(taxRate.Rate.ToString())
              .Build();

            var taxes = new List<OrderLineItemTax>();
            taxes.Add(orderLineItemTax);

            var order = new Square.Models.Order.Builder(_locationId)
              .LineItems(lineItems)
              .Taxes(taxes)
              .Build();

            long longShippingCost = (long)(checkout.ShippingCost * 100);

            var shippingCharge = new Money.Builder()
              .Amount(longShippingCost)
              .Currency("USD")
              .Build();

            var shippingFee = new ShippingFee.Builder(charge: shippingCharge)
              .Name("Shipping Charge")
              .Build();

            string currentDomain = System.Environment.GetEnvironmentVariable("ActiveDomain")
                ?? _config["ActiveDomain"];

            var checkoutOptions = new CheckoutOptions.Builder()
              .RedirectUrl($"{currentDomain}/order-success")
              .AskForShippingAddress(true)
              .ShippingFee(shippingFee)
              .Build();

            var buyerAddress = new Address.Builder()
                .AddressLine1(checkout.Address.LineOne)
                .AddressLine2(checkout.Address.LineTwo)
                .Locality(checkout.Address.City)
                .AdministrativeDistrictLevel1(checkout.Address.State)
                .PostalCode(checkout.Address.Zip)
                .Country("US")
                .FirstName(checkout.Address.FirstName)
                .LastName(checkout.Address.LastName)
                .Build();

            var prePopulatedData = new PrePopulatedData.Builder()
              .BuyerEmail(checkout.UserEmail)
              .BuyerPhoneNumber($"1{checkout.Address.PhoneNumber}")
              .BuyerAddress(buyerAddress)
              .Build();

            var body = new CreatePaymentLinkRequest.Builder()
              .Order(order)
              .PrePopulatedData(prePopulatedData)
              .CheckoutOptions(checkoutOptions)
              .Build();

            Console.WriteLine("!!! Body: " + JsonConvert.SerializeObject(body));

            try
            {
                CreatePaymentLinkResponse result = await _squareClient.CheckoutApi.CreatePaymentLinkAsync(body: body);

                return new ServiceResponse<string>
                {
                    Data = result.PaymentLink.LongUrl
                };
            }
            catch (ApiException e)
            {
                Console.WriteLine("Failed to make the request");
                Console.WriteLine($"Response Code: {e.ResponseCode}");
                Console.WriteLine($"Exception: {e.Message}");

                return new ServiceResponse<string>
                {
                    Success = false,
                    Message = e.Message
                };
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception: {e.Message}");

                return new ServiceResponse<string>
                {
                    Success = false,
                    Message = e.Message
                };
            }
        }

        public async Task<ServiceResponse<bool>> FullfillOrder(HttpRequest request)
        {
            // Webhooks triggered: order.updated, payment.updated

            var json = await new StreamReader(request.Body).ReadToEndAsync();
            Console.WriteLine("!!! Request: " + json);

            var signature = request.Headers["x-square-hmacsha256-signature"].ToString() ?? "";

            // Update URL when testing
            var webhookUrl = System.Environment.GetEnvironmentVariable("SquareKeys_WebhookUrl")
                ?? _config["SquareKeys:WebhookUrl"];

            var webhookKey = System.Environment.GetEnvironmentVariable("SquareKeys_WebhookSignature") ?? _config["SquareKeys:WebhookSignature"];

            var validWebhook = WebhooksHelper.IsValidWebhookEventSignature(json, signature, webhookKey, webhookUrl);

            if (validWebhook)
            {
                try
                {
                    SquarePaymentUpdatedEvent paymentUpdatedEvent = JsonConvert.DeserializeObject<SquarePaymentUpdatedEvent>(json);

                    if (paymentUpdatedEvent != null && paymentUpdatedEvent.Data.Object.Payment.Status == "COMPLETED")
                    {
                        var squareOrderId = paymentUpdatedEvent.Data.Object.Payment.OrderId;
                        Console.WriteLine("!!! OrderId: " + squareOrderId);

                        RetrieveOrderResponse result = await _squareClient.OrdersApi.RetrieveOrderAsync(orderId: squareOrderId);

                        Square.Models.Order squareOrder = result.Order;

                        Console.WriteLine("!!! Square Order: " + JsonConvert.SerializeObject(squareOrder));

                        var shippingInfo = squareOrder.Fulfillments.First().ShipmentDetails.Recipient;


                        var orderId = Guid.NewGuid();

                        Shared.Order order = new Shared.Order
                        {
                            Id = orderId,
                            Email = shippingInfo.EmailAddress,
                            Address = new Shared.Address
                            {
                                OrderId = orderId,
                                FirstName = shippingInfo.Address.FirstName,
                                LastName = shippingInfo.Address.LastName,
                                PhoneNumber = shippingInfo.PhoneNumber,
                                City = shippingInfo.Address.Locality,
                                LineOne = shippingInfo.Address.AddressLine1,
                                LineTwo = shippingInfo.Address.AddressLine2,
                                Zip = shippingInfo.Address.PostalCode,
                                State = shippingInfo.Address.AdministrativeDistrictLevel1
                            },
                            ShippingCost = (decimal)squareOrder.TotalServiceChargeMoney.Amount / 100,
                            SalesTax = (decimal)squareOrder.TotalTaxMoney.Amount / 100,
                            TotalPrice = (decimal)squareOrder.TotalMoney.Amount / 100

                        };

                        Console.WriteLine("!!! Order: " + JsonConvert.SerializeObject(order));
                        await _orderService.PlaceOrder(order);
                    }

                    return new ServiceResponse<bool> { Data = true };
                }
                catch (Exception e)
                {
                    Console.WriteLine("!!! Exception: " + e.Message);

                    return new ServiceResponse<bool>
                    {
                        Data = false,
                        Success = false,
                        Message = e.Message
                    };
                }
            }
            else
            {
                return new ServiceResponse<bool>
                {
                    Data = false,
                    Success = false,
                    Message = "Invalid Webook Signature"
                };
            }
        }




        Session IPaymentService.CreateCheckoutSession(CheckoutDTO checkout)
        {
            throw new NotImplementedException();
        }
    }
}
