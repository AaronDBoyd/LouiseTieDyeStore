
namespace LouiseTieDyeStore.Server.Services.ProductService
{
    public class ProductService : IProductService
    {
        private readonly DataContext _context;

        public ProductService(DataContext context)
        {
            _context = context;
        }

        public Task<ServiceResponse<Product>> GetProduct(int productId)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceResponse<List<Product>>> GetProducts()
        {
            var products = await _context.Products.ToListAsync();

            foreach ( var product in products)
            {
                product.Images = await _context.Images
                    .Where(i => i.ProductId == product.Id)
                    .ToListAsync();
            }


            var response = new ServiceResponse<List<Product>>
            {
                Data = products
            };

            return response;
        }
    }
}
