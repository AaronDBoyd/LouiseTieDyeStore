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
            modelBuilder.Entity<CartItem>()
                .HasKey(ci => new { ci.UserEmail, ci.ProductId });

            #region ProductTypes
            modelBuilder.Entity<ProductType>().HasData(
                new ProductType { Id = 1, Name = "T-Shirt" },
                new ProductType { Id = 2, Name = "Long Sleeve Shirt" },
                new ProductType { Id = 3, Name = "Onesie" }
            );
            #endregion

            #region Categories
            modelBuilder.Entity<Category>().HasData(
                            new Category
                            { Id = 1, Name = "Women", Url = "women" },
                            new Category
                            { Id = 2, Name = "Men", Url = "men" },
                            new Category
                            { Id = 3, Name = "Kids", Url = "kids" }
                        );
                        #endregion

            #region Images
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
            #endregion

            #region Products
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
            #endregion 

            #region TaxRates
            modelBuilder.Entity<TaxRate>().HasData(
                new()
                {
                    Id = 1,
                    State = "Alabama",
                    Abbreviation = "AL",
                    Rate = 4.000M
                },
                new()
                {
                    Id = 2,
                    State = "Alaska",
                    Abbreviation = "AK",
                    Rate = 0.000M
                },
                new()
                {
                    Id = 3,
                    State = "Arizona",
                    Abbreviation = "AZ",
                    Rate = 5.600M
                },
                new()
                {
                    Id = 4,
                    State = "Arkansas",
                    Abbreviation = "AR",
                    Rate = 6.500M
                },
                new()
                {
                    Id = 5,
                    State = "California",
                    Abbreviation = "CA",
                    Rate = 7.250M
                },
                new()
                {
                    Id = 6,
                    State = "Colorado",
                    Abbreviation = "CO",
                    Rate = 2.900M
                },
                new()
                {
                    Id = 7,
                    State = "Connecticut",
                    Abbreviation = "CT",
                    Rate = 6.350M
                },
                new()
                {
                    Id = 8,
                    State = "Delaware",
                    Abbreviation = "DE",
                    Rate = 0.000M
                },
                new()
                {
                    Id = 9,
                    State = "Florida",
                    Abbreviation = "FL",
                    Rate = 6.000M
                },
                new()
                {
                    Id = 10,
                    State = "Georgia",
                    Abbreviation = "GA",
                    Rate = 4.000M
                },
                new()
                {
                    Id = 11,
                    State = "Hawaii",
                    Abbreviation = "HI",
                    Rate = 4.000M
                },
                new()
                {
                    Id = 12,
                    State = "Idaho",
                    Abbreviation = "ID",
                    Rate = 6.000M
                },
                new()
                {
                    Id = 13,
                    State = "Illinois",
                    Abbreviation = "IL",
                    Rate = 6.250M
                },
                new()
                {
                    Id = 14,
                    State = "Indiana",
                    Abbreviation = "IN",
                    Rate = 7.000M
                },
                new()
                {
                    Id = 15,
                    State = "Iowa",
                    Abbreviation = "IA",
                    Rate = 6.000M
                },
                new()
                {
                    Id = 16,
                    State = "Kansas",
                    Abbreviation = "KS",
                    Rate = 6.500M
                },
                new()
                {
                    Id = 17,
                    State = "Kentucky",
                    Abbreviation = "KY",
                    Rate = 6.000M
                },
                new()
                {
                    Id = 18,
                    State = "Louisiana",
                    Abbreviation = "LA",
                    Rate = 4.450M
                },
                new()
                {
                    Id = 19,
                    State = "Maine",
                    Abbreviation = "ME",
                    Rate = 5.500M
                },
                new()
                {
                    Id = 20,
                    State = "Maryland",
                    Abbreviation = "MD",
                    Rate = 6.000M
                },
                new()
                {
                    Id = 21,
                    State = "Massachusetts",
                    Abbreviation = "MA",
                    Rate = 6.250M
                },
                new()
                {
                    Id = 22,
                    State = "Michigan",
                    Abbreviation = "MI",
                    Rate = 6.000M
                },
                new()
                {
                    Id = 23,
                    State = "Minnesota",
                    Abbreviation = "MN",
                    Rate = 6.875M
                },
                new()
                {
                    Id = 24,
                    State = "Mississippi",
                    Abbreviation = "MS",
                    Rate = 7.000M
                },
                new()
                {
                    Id = 25,
                    State = "Missouri",
                    Abbreviation = "MO",
                    Rate = 4.225M
                },
                new()
                {
                    Id = 26,
                    State = "Montana",
                    Abbreviation = "MT",
                    Rate = 0.000M
                },
                new()
                {
                    Id = 27,
                    State = "Nebraska",
                    Abbreviation = "NE",
                    Rate = 5.500M
                },
                new()
                {
                    Id = 28,
                    State = "Nevada",
                    Abbreviation = "NV",
                    Rate = 6.850M
                },
                new()
                {
                    Id = 29,
                    State = "New Hampshire",
                    Abbreviation = "NH",
                    Rate = 0.000M
                },
                new()
                {
                    Id = 30,
                    State = "New Jersey",
                    Abbreviation = "NJ",
                    Rate = 6.625M
                },
                new()
                {
                    Id = 31,
                    State = "New Mexico",
                    Abbreviation = "MN",
                    Rate = 4.875M
                },
                new()
                {
                    Id = 32,
                    State = "New York",
                    Abbreviation = "NY",
                    Rate = 4.000M
                },
                new()
                {
                    Id = 33,
                    State = "North Carolina",
                    Abbreviation = "NC",
                    Rate = 4.750M
                },
                new()
                {
                    Id = 34,
                    State = "North Dakota",
                    Abbreviation = "ND",
                    Rate = 5.000M
                },
                new()
                {
                    Id = 35,
                    State = "Ohio",
                    Abbreviation = "OH",
                    Rate = 5.750M
                },
                new()
                {
                    Id = 36,
                    State = "Oklahoma",
                    Abbreviation = "OK",
                    Rate = 4.500M
                },
                new()
                {
                    Id = 37,
                    State = "Oregon",
                    Abbreviation = "OR",
                    Rate = 0.000M
                },
                new()
                {
                    Id = 38,
                    State = "Pennsylvania",
                    Abbreviation = "PA",
                    Rate = 6.000M
                },
                new()
                {
                    Id = 39,
                    State = "Rhode Island",
                    Abbreviation = "RI",
                    Rate = 7.000M
                },
                new()
                {
                    Id = 40,
                    State = "South Carolina",
                    Abbreviation = "SC",
                    Rate = 6.000M
                },
                new()
                {
                    Id = 41,
                    State = "South Dakota",
                    Abbreviation = "SD",
                    Rate = 4.200M
                },
                new()
                {
                    Id = 42,
                    State = "Tennessee",
                    Abbreviation = "TN",
                    Rate = 7.000M
                },
                new()
                {
                    Id = 43,
                    State = "Texas",
                    Abbreviation = "TX",
                    Rate = 6.250M
                },
                new()
                {
                    Id = 44,
                    State = "Utah",
                    Abbreviation = "UT",
                    Rate = 6.100M
                },
                new()
                {
                    Id = 45,
                    State = "Vermont",
                    Abbreviation = "VT",
                    Rate = 6.000M
                },
                new()
                {
                    Id = 46,
                    State = "Virginia",
                    Abbreviation = "VA",
                    Rate = 5.300M
                },
                new()
                {
                    Id = 47,
                    State = "Washington",
                    Abbreviation = "WA",
                    Rate = 6.500M
                },
                new()
                {
                    Id = 48,
                    State = "West Virginia",
                    Abbreviation = "WV",
                    Rate = 6.000M
                },
                new()
                {
                    Id = 49,
                    State = "Wisconsin",
                    Abbreviation = "WI",
                    Rate = 5.000M
                },
                new()
                {
                    Id = 50,
                    State = "Wyoming",
                    Abbreviation = "WY",
                    Rate = 4.000M
                },
                new()
                {
                    Id = 51,
                    State = "District of Columbia",
                    Abbreviation = "DC",
                    Rate = 6.000M
                }
           );
            #endregion
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<TaxRate> TaxRates { get; set; }
    }
}
