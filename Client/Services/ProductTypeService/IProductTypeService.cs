namespace LouiseTieDyeStore.Client.Services.ProductTypeService
{
    public interface IProductTypeService
    {
        public List<ProductType> ProductTypes { get; set; }
        Task GetProductTypes();
    }
}
