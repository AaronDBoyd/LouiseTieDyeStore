using Newtonsoft.Json;
using Stripe;
using Stripe.Checkout;

namespace LouiseTieDyeStore.Server.Services.PaymentService
{
    public class PaymentService : IPaymentService
    {
        private readonly ICartService _cartService;
        private readonly IAuthService _authService;
        private readonly IConfiguration _config;

        public PaymentService(ICartService cartService,
            IAuthService authService,
            IConfiguration config
            )
        {
            _cartService = cartService;
            _authService = authService;
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

                    Console.WriteLine("session : " + JsonConvert.SerializeObject(session));
                    
                    // TODO: Process Order
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
