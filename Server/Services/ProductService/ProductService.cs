
namespace LouiseTieDyeStore.Server.Services.ProductService
{
    public class ProductService : IProductService
    {
        private readonly DataContext _context;

        public ProductService(DataContext context)
        {
            _context = context;
        }

        public async Task<ServiceResponse<List<Product>>> GetNewestProducts()
        {
            var products = await _context.Products
                    .Where(p => p.Visible && !p.Deleted)
                    .OrderByDescending(p => p.Id)
                    .Take(9)
                    .ToListAsync();

            await GetProductImages(products);

            var response = new ServiceResponse<List<Product>>
            {
                Data = products
            };

            return response;
        }

        public Task<ServiceResponse<Product>> GetProduct(int productId)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceResponse<List<Product>>> GetProducts()
        {
            var products = await _context.Products
                    .Where(p => p.Visible && !p.Deleted)
                    .ToListAsync();

            await GetProductImages(products);

            var response = new ServiceResponse<List<Product>>
            {
                Data = products
            };

            return response;
        }


        public async Task<ServiceResponse<List<Product>>> GetProductsByCategory(string categoryUrl)
        {
            var products = await _context.Products
                .Where(p => p.Category.Url.ToLower().Equals(categoryUrl.ToLower()) &&
                    p.Visible && !p.Deleted)
                .ToListAsync();

            await GetProductImages(products);

            var response = new ServiceResponse<List<Product>>
            {
                Data = products
            };

            return response;
        }

        private async Task GetProductImages(List<Product> products)
        {
            foreach (var product in products)
            {
                product.Images = await _context.Images
                    .Where(i => i.ProductId == product.Id)
                    .ToListAsync();
            }
        }
    }
}
