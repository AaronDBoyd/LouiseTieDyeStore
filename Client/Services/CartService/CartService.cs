
using Blazored.LocalStorage;
using LouiseTieDyeStore.Client.Pages;
using Newtonsoft.Json;

namespace LouiseTieDyeStore.Client.Services.CartService
{
    public class CartService : ICartService
    {
        private readonly ILocalStorageService _localStorage;
        private readonly HttpClient _privateClient;
        private readonly HttpClient _publicClient;
        private readonly IAuthService _authService;

        public CartService(ILocalStorageService localStorage,
            PublicClient publicClient,
            HttpClient privateClient,
            IAuthService authService)
        {
            _localStorage = localStorage;
            _privateClient = privateClient;
            _publicClient = publicClient.Client;
            _authService = authService;
        }

        public event Action OnChange;

        public async Task AddToCart(CartItem cartItem)
        {
            if (await _authService.IsUserAuthenticated())
            {
                cartItem.UserEmail = await _authService.GetAuthenticatedUsername();
                await _privateClient.PostAsJsonAsync("api/cart/add", cartItem);
            }
            else
            {
                var cart = await _localStorage.GetItemAsync<List<CartItem>>("cart");
                if (cart == null)
                {
                    cart = new List<CartItem>();
                }

                var sameItem = cart.Find(x => x.ProductId == cartItem.ProductId);
                if (sameItem == null)
                {
                    cart.Add(cartItem);
                }
 
                await _localStorage.SetItemAsync("cart", cart);
            }

            await GetCartItemsCount();
        }

        public async Task GetCartItemsCount()
        {
            if (await _authService.IsUserAuthenticated())
            {
                var result = await _privateClient.GetFromJsonAsync<ServiceResponse<int>>("api/cart/count");
                var count = result.Data;

                await _localStorage.SetItemAsync<int>("cartItemsCount", count);
            }
            else
            {
                var cart = await _localStorage.GetItemAsync<List<CartItem>>("cart");
                await _localStorage.SetItemAsync<int>("cartItemsCount", cart != null ? cart.Count : 0);
            }

            OnChange.Invoke();
        }

        public async Task<List<CartProductResponse>> GetCartProducts()
        {
            if (await _authService.IsUserAuthenticated())
            {
                var response = await _privateClient.GetFromJsonAsync<ServiceResponse<List<CartProductResponse>>>("api/cart");
                return response.Data;
            }
            else
            {
                var cartItems = await _localStorage.GetItemAsync<List<CartItem>>("cart");
                if (cartItems == null)
                {
                    return new List<CartProductResponse>();
                }

                // Turn "CartItems" into "CartProductResponses" **if Products have not been sold**
                var response = await _publicClient.PostAsJsonAsync("api/cart/products", cartItems);
                var cartProducts =
                    await response.Content.ReadFromJsonAsync<ServiceResponse<List<CartProductResponse>>>();

                List<int> cartProductIds = cartProducts.Data.Select(p => p.ProductId).ToList();

                var newList = new List<CartItem>();

                foreach (var item in cartItems)
                {
                    if (cartProductIds.Contains(item.ProductId))
                    {
                        newList.Add(item);
                    }
                }

                await _localStorage.SetItemAsync("cart", newList);

                return cartProducts.Data;
            }
        }

        public async Task RemoveProductFromCart(int productId)
        {
            if (await _authService.IsUserAuthenticated())
            {
                await _privateClient.DeleteAsync($"api/cart/{productId}");
            }
            else
            {
                var cart = await _localStorage.GetItemAsync<List<CartItem>>("cart");
                if (cart == null)
                {
                    return;
                }

                var cartItem = cart.Find(x => x.ProductId == productId);

                if (cartItem != null)
                {
                    cart.Remove(cartItem);
                    await _localStorage.SetItemAsync("cart", cart);
                }
            }
        }

        public async Task StoreCartItems(bool emptyLocalCart = false)
        {
            var localCart = await _localStorage.GetItemAsync<List<CartItem>>("cart");
            if (localCart == null)
            {
                return;
            }

            string email;
            if (await _authService.IsUserAuthenticated())
            {
                email = await _authService.GetAuthenticatedUsername();
            }
            else
            {
                email = await _localStorage.GetItemAsync<string>("guestCheckoutEmail");
            }

            foreach (var item in localCart)
            {
                item.UserEmail = email;
            }

            await _publicClient.PostAsJsonAsync("api/cart", localCart);

            if (emptyLocalCart)
            {
                await _localStorage.RemoveItemAsync("cart");
            }
        }
    }
}
