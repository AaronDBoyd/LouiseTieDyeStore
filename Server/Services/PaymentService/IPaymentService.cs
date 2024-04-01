using Stripe.Checkout;

namespace LouiseTieDyeStore.Server.Services.PaymentService
{
    public interface IPaymentService
    {
        Session CreateCheckoutSession(CheckoutDTO checkout);
        Task<ServiceResponse<bool>> FullfillOrder(HttpRequest request);
    }
}
