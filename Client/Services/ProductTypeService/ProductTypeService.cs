
namespace LouiseTieDyeStore.Client.Services.ProductTypeService
{
    public class ProductTypeService : IProductTypeService
    {
        private readonly HttpClient _privateClient;
        private readonly HttpClient _publicClient;

        public ProductTypeService(HttpClient http, PublicClient publicClient)
        {
            _privateClient = http;
            _publicClient = publicClient.Client;
        }

        public List<ProductType> ProductTypes { get; set; } = new List<ProductType>();
        public string DeleteMessage { get; set; } = string.Empty;

        public event Action OnChange;

        public async Task AddProductType(ProductType productType)
        {
            var response = await _privateClient.PostAsJsonAsync("api/producttype", productType);
            ProductTypes = (await response.Content
                .ReadFromJsonAsync<ServiceResponse<List<ProductType>>>()).Data;
            OnChange.Invoke();
        }

        public async Task DeleteProductType(int typeId)
        {
            var result = await _privateClient.DeleteAsync($"api/producttype/{typeId}");
            var response = (await result.Content.ReadFromJsonAsync<ServiceResponse<List<ProductType>>>());

            if (response.Success)
            {
                ProductTypes = response.Data;
            }

            DeleteMessage = response.Message;
            OnChange.Invoke();
        }

        public async Task GetProductTypes()
        {
            var result = await _publicClient.GetFromJsonAsync<ServiceResponse<List<ProductType>>>("api/producttype");
            ProductTypes = result.Data;
        }

        public async Task UpdateProductType(ProductType productType)
        {
            var response = await _privateClient.PutAsJsonAsync("api/producttype", productType);
            ProductTypes = (await response.Content
                .ReadFromJsonAsync<ServiceResponse<List<ProductType>>>()).Data;
            OnChange.Invoke();
        }
    }
}
