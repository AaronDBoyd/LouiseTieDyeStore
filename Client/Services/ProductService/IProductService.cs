namespace LouiseTieDyeStore.Client.Services.ProductService
{
    public interface IProductService
    {
        event Action ProductsChanged;
        List<Product> Products { get; set; }
        List<Product> AdminProducts { get; set; }
        List<string> Sizes { get; set; }
        string? SizeFilter { get; set; }
        string? TypeFilter { get; set; }
        string Message { get; set; }
        int CurrentPage { get; set; }
        int PageCount { get; set; }
        string LastSearchText { get; set; }
        Task GetProducts(int page, string? categoryUrl = null);
        Task<ServiceResponse<Product>> GetProduct(int productId);
        Task<ServiceResponse<Product>> GetAdminProduct(int productId);
        Task SearchProducts(string searchText, int page);
        Task<List<string>> GetProductSearchSuggestions(string searchText);
        Task GetAdminProducts();
        Task<Product> CreateProduct(Product product);
        Task<Product> UpdateProduct(Product product);
        Task DeleteProduct(Product product);
        Task FilterSize(string category, string size);
        Task FilterType(string category, string type);
    }
}
