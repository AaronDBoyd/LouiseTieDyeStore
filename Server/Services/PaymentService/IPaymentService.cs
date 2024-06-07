using Stripe.Checkout;

namespace LouiseTieDyeStore.Server.Services.PaymentService
{
    public interface IPaymentService
    {
        Task<ServiceResponse<bool>> FullfillOrder(HttpRequest request);

        // For Stripe Only
        Session CreateCheckoutSession(CheckoutDTO checkout);

        // For Square
        Task<ServiceResponse<string>> CreatePaymentLink(CheckoutDTO ckeckout);
    }
}
