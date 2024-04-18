namespace LouiseTieDyeStore.Server.Services.ProductService
{
    public interface IProductService
    {
        Task<ServiceResponse<List<Product>>> GetProducts();
        Task<ServiceResponse<Product>> GetProduct(int productId);
        Task<ServiceResponse<Product>> GetAdminProduct(int productId);
        Task<ServiceResponse<ProductPageResults>> GetProductsByCategory(string categoryUrl, int page, string? sizeFilter = null, string? typeFilter = null);
        Task<ServiceResponse<List<Product>>> GetNewestProducts();
        Task<ServiceResponse<List<Product>>> GetAdminProducts();
        Task<ServiceResponse<Product>> CreateProduct(Product product);
        Task<ServiceResponse<Product>> UpdateProduct(Product product);
        Task<ServiceResponse<bool>> DeleteProduct(int productId);
        Task<ServiceResponse<ProductPageResults>> SearchProducts(string searchText, int page);
        Task<ServiceResponse<List<string>>> GetProductSearchSuggestions(string searchText);
    }
}
