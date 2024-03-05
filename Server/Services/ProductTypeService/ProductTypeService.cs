
namespace LouiseTieDyeStore.Server.Services.ProductTypeService
{
    public class ProductTypeService : IProductTypeService
    {
        private readonly DataContext _context;

        public ProductTypeService(DataContext context)
        {
            _context = context;
        }

        public async Task<ServiceResponse<List<ProductType>>> AddProductType(ProductType productType)
        {
            _context.ProductTypes.Add(productType);
            await _context.SaveChangesAsync();

            return await GetProductTypes();
        }

        public async Task<ServiceResponse<List<ProductType>>> DeleteProductType(int productTypeId)
        {
            var dbProductType = await _context.ProductTypes.FindAsync(productTypeId);
            if (dbProductType == null)
            {
                return new ServiceResponse<List<ProductType>>
                {
                    Success = false,
                    Message = "Product Type not found."
                };
            }

            // Check to see if any current products are of this type
            var dbProducts = await _context.Products
                .Where(p => p.ProductTypeId == productTypeId 
                && !p.Deleted)
                .ToListAsync();

            if (dbProducts.Count > 0)
            {
                return new ServiceResponse<List<ProductType>>
                {
                    Success = false,
                    Message = "Cannot Delete this Product Type as there are Currently Products of this Type."
                };
            }

            _context.ProductTypes.Remove(dbProductType);
            await _context.SaveChangesAsync();

            var productTypes = await _context.ProductTypes.ToListAsync();
            return await GetProductTypes();
        }

        public async Task<ServiceResponse<List<ProductType>>> GetProductTypes()
        {
            var productTypes = await _context.ProductTypes.ToListAsync();
            return new ServiceResponse<List<ProductType>> { Data = productTypes };
        }

        public async Task<ServiceResponse<List<ProductType>>> UpdateProductType(ProductType productType)
        {
            var dbProductType = await _context.ProductTypes.FindAsync(productType.Id);
            if (dbProductType == null)
            {
                return new ServiceResponse<List<ProductType>>
                {
                    Success = false,
                    Message = "Product Type not found."
                };
            }

            dbProductType.Name = productType.Name;
            await _context.SaveChangesAsync();

            return await GetProductTypes();
        }
    }
}
