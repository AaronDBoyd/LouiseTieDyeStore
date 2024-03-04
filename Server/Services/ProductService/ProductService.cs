
namespace LouiseTieDyeStore.Server.Services.ProductService
{
    public class ProductService : IProductService
    {
        private readonly DataContext _context;

        public ProductService(DataContext context)
        {
            _context = context;
        }

        public async Task<ServiceResponse<Product>> CreateProduct(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return new ServiceResponse<Product> { Data = product };
        }

        public async Task<ServiceResponse<bool>> DeleteProduct(int productId)
        {
            var dbProduct = await _context.Products.FindAsync(productId);
            if (dbProduct == null)
            {
                return new ServiceResponse<bool>
                {
                    Success = false,
                    Data = false,
                    Message = "Product not found."
                };
            }

            dbProduct.Deleted = true;

            await _context.SaveChangesAsync();
            return new ServiceResponse<bool> { Data = true };
        }

        public async Task<ServiceResponse<List<Product>>> GetAdminProducts()
        {
            var response = new ServiceResponse<List<Product>>
            {
                Data = await _context.Products
                    .Where(p => !p.Deleted)
                    .Include(p => p.Images)
                    .Include(p => p.Category)
                    .Include(p => p.ProductType)
                    .ToListAsync()
            };

            return response;
        }

        public async Task<ServiceResponse<List<Product>>> GetNewestProducts()
        {
            var products = await _context.Products
                    .Where(p => p.Visible && !p.Deleted)
                    .Include(p => p.Images)
                    .OrderByDescending(p => p.Id)
                    .Take(9)
                    .ToListAsync();

            var response = new ServiceResponse<List<Product>>
            {
                Data = products
            };

            return response;
        }

        public async Task<ServiceResponse<Product>> GetProduct(int productId)
        {
            var response = new ServiceResponse<Product>();
            Product product = null;

            product = await _context.Products
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.Id == productId && !p.Deleted && p.Visible);

            if (product == null)
            {
                response.Success = false;
                response.Message = "Sorry, but this product does not exist.";
            }
            else
            {
                response.Data = product;
            }

            return response;
        }

        public async Task<ServiceResponse<List<Product>>> GetProducts()
        {
            var products = await _context.Products
                    .Where(p => p.Visible && !p.Deleted)
                    .Include(p => p.Images)
                    .ToListAsync();

            var response = new ServiceResponse<List<Product>>
            {
                Data = products
            };

            return response;
        }


        public async Task<ServiceResponse<ProductPageResults>> GetProductsByCategory(string categoryUrl, int page, string? sizeFilter = null, string? typeFilter = null)
        {
            var pageResults = 3f;

            // Get count of filtered products
            int count = new();

            if (sizeFilter == null && typeFilter == null)
            {
                count = await _context.Products
                .Where(p => p.Category.Url.ToLower().Equals(categoryUrl.ToLower()) &&
                    p.Visible && !p.Deleted)
                .CountAsync();
            }
            else if (sizeFilter == null && typeFilter != null)
            {
                count = await _context.Products
                .Where(p => p.Category.Url.ToLower().Equals(categoryUrl.ToLower()) &&
                    p.Visible && !p.Deleted
                    && p.ProductType.Name == typeFilter)
                .CountAsync();
            }
            else if (sizeFilter != null && typeFilter == null)
            {
                count = await _context.Products
                .Where(p => p.Category.Url.ToLower().Equals(categoryUrl.ToLower()) &&
                    p.Visible && !p.Deleted
                    && p.Size == sizeFilter)
                .CountAsync();
            }
            else
            {
                count = await _context.Products
                .Where(p => p.Category.Url.ToLower().Equals(categoryUrl.ToLower()) &&
                    p.Visible && !p.Deleted 
                    && p.Size == sizeFilter
                    && p.ProductType.Name == typeFilter)
                .CountAsync();
            }

            var pageCount = Math.Ceiling(count / pageResults);

            // Get page results of filtered products
            List<Product> products = new();

            if (sizeFilter == null && typeFilter == null)
            {
                products = await _context.Products
                .Where(p => p.Category.Url.ToLower().Equals(categoryUrl.ToLower()) &&
                    p.Visible && !p.Deleted)
                .Include(p => p.Images)
                .Skip((page - 1) * (int)pageResults)
                .Take((int)pageResults)
                .ToListAsync();
            }
            else if (sizeFilter == null && typeFilter != null)
            {
                products = await _context.Products
                .Where(p => p.Category.Url.ToLower().Equals(categoryUrl.ToLower()) &&
                    p.Visible && !p.Deleted
                    && p.ProductType.Name == typeFilter)
                .Include(p => p.Images)
                .Skip((page - 1) * (int)pageResults)
                .Take((int)pageResults)
                .ToListAsync();
            }
            else if (sizeFilter != null && typeFilter == null)
            {
                products = await _context.Products
                .Where(p => p.Category.Url.ToLower().Equals(categoryUrl.ToLower()) &&
                    p.Visible && !p.Deleted
                    && p.Size == sizeFilter)
                .Include(p => p.Images)
                .Skip((page - 1) * (int)pageResults)
                .Take((int)pageResults)
                .ToListAsync();
            }
            else
            {
                products = await _context.Products
                .Where(p => p.Category.Url.ToLower().Equals(categoryUrl.ToLower()) &&
                    p.Visible && !p.Deleted 
                    && p.Size == sizeFilter
                    && p.ProductType.Name == typeFilter)
                .Include(p => p.Images)
                .Skip((page - 1) * (int)pageResults)
                .Take((int)pageResults)
                .ToListAsync();
            }

            var response = new ServiceResponse<ProductPageResults>
            {
                Data = new ProductPageResults
                {
                    Products = products,
                    CurrentPage = page,
                    Pages = (int)pageCount
                }
            };

            return response;
        }

        public async Task<ServiceResponse<List<string>>> GetProductSearchSuggestions(string searchText)
        {
            var products = await _context.Products
                            .Where(p => p.Title.ToLower().Contains(searchText.ToLower()) ||
                                p.Description.ToLower().Contains(searchText.ToLower()) &&
                                p.Visible && !p.Deleted)
                            .ToListAsync();

            List<string> result = new List<string>();

            foreach ( var product in products)
            {
                if (product.Title.Contains(searchText, StringComparison.OrdinalIgnoreCase))
                {
                    result.Add(product.Title);
                }

                if (product.Description != null)
                {
                    var punctuation = product.Description.Where(char.IsPunctuation)
                        .Distinct().ToArray();
                    var words = product.Description.Split()
                        .Select(s => s.Trim(punctuation));

                    foreach (var word in words)
                    {
                        if (word.Contains(searchText, StringComparison.OrdinalIgnoreCase)
                            && !result.Contains(word))
                        {
                            result.Add(word);
                        }
                    }
                }               
            }

            return new ServiceResponse<List<string>> { Data = result };
        }

        public async Task<ServiceResponse<ProductPageResults>> SearchProducts(string searchText, int page)
        {
            var pageResults = 3f;
            var count = await _context.Products
                .Where(p => p.Title.ToLower().Contains(searchText.ToLower()) ||
                                p.Description.ToLower().Contains(searchText.ToLower()) &&
                                p.Visible && !p.Deleted)
                .CountAsync();

            var pageCount = Math.Ceiling(count / pageResults);
            var products = await _context.Products
                            .Where(p => p.Title.ToLower().Contains(searchText.ToLower()) ||
                                p.Description.ToLower().Contains(searchText.ToLower()) &&
                                p.Visible && !p.Deleted)
                            .Include(p => p.Images)
                            .Skip((page - 1) * (int)pageResults)
                            .Take((int)pageResults)
                            .ToListAsync();


            var response = new ServiceResponse<ProductPageResults>
            {
                Data = new ProductPageResults
                {
                    Products = products,
                    CurrentPage = page,
                    Pages = (int)pageCount
                }
            };

            return response;
        }

        public async Task<ServiceResponse<Product>> UpdateProduct(Product product)
        {
            var dbProduct = await _context.Products
                                          .Include(p => p.Images)
                                          .FirstOrDefaultAsync(p => p.Id == product.Id);
            if (dbProduct == null)
            {
                return new ServiceResponse<Product>
                {
                    Success = false,
                    Message = "Product not found."
                };
            }

            dbProduct.Title = product.Title;
            dbProduct.Description = product.Description;
            dbProduct.Size = product.Size;
            dbProduct.OriginalPrice = product.OriginalPrice;
            dbProduct.Price = product.Price;
            dbProduct.CategoryId = product.CategoryId;
            dbProduct.ProductTypeId = product.ProductTypeId;
            dbProduct.Visible = product.Visible;

            var productImages = dbProduct.Images;
            _context.Images.RemoveRange(productImages);

            dbProduct.Images = product.Images;

            await _context.SaveChangesAsync();
            return new ServiceResponse<Product> { Data = product };
        }
    }
}
