using LouiseTieDyeStore.Client.Pages.Admin;
using System.Security.Policy;
using System.Xml.Linq;

namespace LouiseTieDyeStore.Server.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductType>().HasData(
                new ProductType { Id = 1, Name = "T-Shirt" },
                new ProductType { Id = 2, Name = "Long Sleeve Shirt" },
                new ProductType { Id = 3, Name = "Onesie" }
            );

            modelBuilder.Entity<Category>().HasData(
                new Category
                { Id = 1, Name = "Women", Url = "women" },
                new Category
                { Id = 2, Name = "Men", Url = "men" },
                new Category
                { Id = 3, Name = "Kids", Url = "kids" }
            );

            modelBuilder.Entity<Image>().HasData(
                new Image
                {
                    Id = 1,
                    Url = "https://i.imgur.com/1DnQj7V.jpg",
                    ProductId = 1
                },
                new Image
                {
                    Id = 2,
                    Url = "https://i.imgur.com/tGJkEL9.jpg",
                    ProductId = 1
                },
                new Image
                {
                    Id = 3,
                    Url = "https://i.imgur.com/zw3Olrp.jpg",
                    ProductId = 2
                },
                new Image
                {
                    Id = 4,
                    Url = "https://i.imgur.com/9uEVXR1.png",
                    ProductId = 2
                },
                new Image
                {
                    Id = 5,
                    Url = "https://i.imgur.com/uHKlJtV.jpg",
                    ProductId = 3
                },
                new Image
                {
                    Id = 6,
                    Url = "https://i.imgur.com/MD7Wzx4.jpg",
                    ProductId = 3
                }
            );

            modelBuilder.Entity<Product>().HasData(
                 new Product
                 {
                     Id = 1,
                     Title = "Cool TyeDye Shirt",
                     Description = "Blue and Yellow pattern on a womens ScoobyDoo t-shirt",
                     Size = "XL",
                     OriginalPrice = 35.00m,
                     Price = 30.00m,
                     ProductTypeId = 1,
                     CategoryId = 1,
                 },
                 new Product
                 {
                     Id = 2,
                     Title = "Really Cool TyeDye Shirt",
                     Description = "Blue and Yellow pattern on a Long Sleeve Shirt",
                     Size = "S",
                     OriginalPrice = 25.00m,
                     Price = 20.00m,
                     ProductTypeId = 2,
                     CategoryId = 2,
                 },
                 new Product
                 {
                     Id = 3,
                     Title = "TyeDye Onesie",
                     Description = "Blue and Red spiral pattern on a baby onesie.",
                     Size = "M",
                     OriginalPrice = 15.00m,
                     Price = 10.00m,
                     ProductTypeId = 3,
                     CategoryId = 3,
                 }
             );
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Address> Addresses { get; set; }
    }
}
