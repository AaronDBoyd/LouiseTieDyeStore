
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using System.Net.Http.Json;

namespace LouiseTieDyeStore.Client.Services.ProductService
{
    public class ProductService : IProductService
    {
        private readonly HttpClient _privateClient;
        private readonly HttpClient _publicClient;

        public ProductService(HttpClient http, PublicClient publicClient)
        {
            _privateClient = http;
            _publicClient = publicClient.Client;
        }

        public List<Product> Products { get; set; } = new List<Product>();
        public List<Product> AdminProducts { get; set; } = new List<Product>();
        public List<string> Sizes { get; set; } = new List<string>
        {
            "XXS", "XS", "S", "M", "L", "XL", "XXL"
        };
        public string? SizeFilter { get; set; } = null;
        public string? TypeFilter { get; set; } = null;
        public string Message { get; set; } = "Loading Products...";
        public int CurrentPage { get; set; } = 1;
        public int PageCount { get; set; } = 0;
        public string LastSearchText { get; set; } = string.Empty;

        public event Action ProductsChanged;

        public async Task<Product> CreateProduct(Product product)
        {
            var result = await _privateClient.PostAsJsonAsync("api/product", product);
            var newProduct = (await result.Content
                .ReadFromJsonAsync<ServiceResponse<Product>>()).Data;
            return newProduct;
        }

        public async Task DeleteProduct(Product product)
        {
            try
            {
            var result = await _privateClient.DeleteAsync($"api/product/{product.Id}");

            }
            catch (AccessTokenNotAvailableException ex)
            {
                ex.Redirect();
            }
        }

        public async Task GetAdminProducts()
        {
                var result = await _privateClient
                  .GetFromJsonAsync<ServiceResponse<List<Product>>>("api/product/admin");
                AdminProducts = result.Data;
                CurrentPage = 1;
                PageCount = 0;
                if (AdminProducts == null || AdminProducts.Count == 0)
                {
                    Message = "No Products found.";
                }
        }

        public async Task<ServiceResponse<Product>> GetProduct(int productId)
        {
            var result = await _publicClient.GetFromJsonAsync<ServiceResponse<Product>>($"api/product/{productId}");
            return result;
        }

        public async Task GetProducts(int page, string? categoryUrl = null)
        {
            if (categoryUrl == null)
            {
                var result = await _publicClient.GetFromJsonAsync<ServiceResponse<List<Product>>>("api/Product/newest");

                if (result != null && result.Data != null)
                {
                    Products = result.Data;
                }

                CurrentPage = 1;
                PageCount = 0;
            }
            else
            {
                var result = await _publicClient.GetFromJsonAsync<ServiceResponse<ProductPageResults>>($"api/Product/category/{categoryUrl}/{page}?sizeFilter={SizeFilter}&typeFilter={TypeFilter}");
                if (result != null && result.Data != null)
                {
                    Products = result.Data.Products;
                    CurrentPage = result.Data.CurrentPage;
                    PageCount = result.Data.Pages;
                }
            }

            if (Products.Count == 0)
            {
                Message = "No products found";
            }

            ProductsChanged.Invoke();
        }

        public async Task<List<string>> GetProductSearchSuggestions(string searchText)
        {
            var result = await _publicClient
                .GetFromJsonAsync<ServiceResponse<List<string>>>($"api/product/searchsuggestions/{searchText}");
            return result.Data;
        }

        public async Task SearchProducts(string searchText, int page)
        {
            LastSearchText = searchText;
            var result = await _publicClient
                .GetFromJsonAsync<ServiceResponse<ProductPageResults>>($"api/product/search/{searchText}/{page}");
            if (result != null && result.Data != null)
            {
                Products = result.Data.Products;
                CurrentPage = result.Data.CurrentPage;
                PageCount = result.Data.Pages;
            }
            if (Products.Count == 0)
            {
                Message = "No products found.";
            }

            ProductsChanged?.Invoke();
        }

        public async Task<Product> UpdateProduct(Product product)
        {
            var result = await _privateClient.PutAsJsonAsync($"api/product", product);
            var content = await result.Content.ReadFromJsonAsync<ServiceResponse<Product>>();
            return content.Data;
        }

        public async Task FilterSize(string category, string size)
        {
            SizeFilter = size;

            await GetProducts(1, category);
        }

        public async Task FilterType(string category, string type)
        {
            TypeFilter = type;

            await GetProducts(1, category);
        }

        public async Task<ServiceResponse<Product>> GetAdminProduct(int productId)
        {
            var result = await _privateClient.GetFromJsonAsync<ServiceResponse<Product>>($"api/product/admin/{productId}");
            return result;
        }
    }
}
