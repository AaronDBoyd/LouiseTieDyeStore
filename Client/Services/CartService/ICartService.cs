namespace LouiseTieDyeStore.Client.Services.CartService
{
    public interface ICartService
    {
        event Action OnChange;
        Task AddToCart(CartItem cartItem);
        Task<List<CartProductResponse>> GetCartProducts();
        Task RemoveProductFromCart(int productId);
        Task StoreCartItems(bool emptyLocalCart = false);
        Task GetCartItemsCount();
    }
}
