using Newtonsoft.Json;
using Stripe;
using Stripe.Checkout;

namespace LouiseTieDyeStore.Server.Services.PaymentService
{
    public class PaymentService : IPaymentService
    {
        private readonly ICartService _cartService;
        private readonly IAuthService _authService;
        private readonly IOrderService _orderService;
        private readonly IConfiguration _config;

        public PaymentService(ICartService cartService,
            IAuthService authService,
            IOrderService orderService,
            IConfiguration config
            )
        {
            _cartService = cartService;
            _authService = authService;
            _orderService = orderService;
            _config = config;

            StripeConfiguration.ApiKey = _config["StripeKeys:ApiKey"];
        }

        public Session CreateCheckoutSession(CheckoutDTO checkout)
        {
            var lineItems = new List<SessionLineItemOptions>();

            checkout.Items.ForEach(item => lineItems.Add(new SessionLineItemOptions()
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    UnitAmountDecimal = item.Price * 100,
                    Currency = "usd",
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = item.Title,
                        Images = new List<string> { item.ImageUrl }
                    }
                },
                Quantity = 1
            }));

            lineItems.Add(new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    UnitAmountDecimal = checkout.SalesTax * 100,
                    Currency = "usd",
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = "Sales Tax"
                    }
                },
                Quantity = 1
            });

            var options = new SessionCreateOptions
            {
                CustomerEmail = checkout.UserEmail,
                ShippingAddressCollection =
                    new SessionShippingAddressCollectionOptions
                    {
                        AllowedCountries = new List<string> { "US" }
                    },
                ShippingOptions = new List<SessionShippingOptionOptions>
                {
                    new SessionShippingOptionOptions
                    {
                        ShippingRateData = new SessionShippingOptionShippingRateDataOptions
                        {
                            DisplayName = "FedEx Ground",
                            Type = "fixed_amount",
                            FixedAmount = new SessionShippingOptionShippingRateDataFixedAmountOptions
                            {
                                Amount = (long?)(checkout.ShippingCost * 100),
                                Currency = "usd"
                            },
                            DeliveryEstimate = new SessionShippingOptionShippingRateDataDeliveryEstimateOptions
                            {
                                Minimum = new SessionShippingOptionShippingRateDataDeliveryEstimateMinimumOptions
                                {
                                    Unit = "business_day",
                                    Value = 10
                                },
                                Maximum = new SessionShippingOptionShippingRateDataDeliveryEstimateMaximumOptions
                                {
                                    Unit = "business_day",
                                    Value = 20
                                }
                            }
                        }
                    }

                },
                PhoneNumberCollection = new SessionPhoneNumberCollectionOptions
                {
                    Enabled = true,
                },
                PaymentMethodTypes = new List<string>
                {
                    "card"
                },
                LineItems = lineItems,
                Mode = "payment",
                SuccessUrl = $"{_config["ActiveDomain"]}/order-success",
                CancelUrl = $"{_config["ActiveDomain"]}/cart"
            };

            var service = new SessionService();
            Session session = service.Create(options);
            return session;
        }

        public async Task<ServiceResponse<bool>> FullfillOrder(HttpRequest request)
        {
            var json = await new StreamReader(request.Body).ReadToEndAsync();

            try
            {
                var stripeEvent = EventUtility.ConstructEvent(
                        json,
                        request.Headers["Stripe-Signature"],
                        _config["StripeKeys:WebHookSecret"]);

                if (stripeEvent.Type == Events.CheckoutSessionCompleted)
                {
                    var session = stripeEvent.Data.Object as Session;

                    Console.WriteLine("!!!Session: " + JsonConvert.SerializeObject(session));

                    var orderId = Guid.NewGuid();

                    var order = new Order
                    {
                        Id = orderId,
                        Email = session.CustomerEmail,
                        Address = new Shared.Address
                        {
                            OrderId = orderId,
                            FirstName = session.CustomerDetails.Name.Split(" ")[0],
                            LastName = session.CustomerDetails.Name.Split(" ")[1],
                            PhoneNumber = session.CustomerDetails.Phone,
                            City = session.CustomerDetails.Address.City,
                            LineOne = session.CustomerDetails.Address.Line1,
                            LineTwo = session.CustomerDetails.Address.Line2 ?? string.Empty,
                            Zip = session.CustomerDetails.Address.PostalCode,
                            State = session.CustomerDetails.Address.State
                        },
                        ShippingCost = (decimal)session.ShippingCost.AmountTotal / 100,
                        TotalPrice = (decimal)session.AmountTotal / 100
                    };


                    await _orderService.PlaceOrder(order);
                }

                return new ServiceResponse<bool> { Data = true };
            }
            catch (StripeException ex)
            {
                return new ServiceResponse<bool>
                {
                    Data = false,
                    Success = false,
                    Message = ex.Message
                };
            }
        }
    }
}
