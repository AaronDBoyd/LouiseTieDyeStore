﻿// <auto-generated />
using LouiseTieDyeStore.Server.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace LouiseTieDyeStore.Server.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20240217033605_ProductSeeding")]
    partial class ProductSeeding
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.25")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("LouiseTieDyeStore.Shared.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Categories");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Women",
                            Url = "women"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Men",
                            Url = "men"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Kids",
                            Url = "kids"
                        });
                });

            modelBuilder.Entity("LouiseTieDyeStore.Shared.Image", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("Images");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            ProductId = 1,
                            Url = "https://i.imgur.com/1DnQj7V.jpg"
                        },
                        new
                        {
                            Id = 2,
                            ProductId = 1,
                            Url = "https://i.imgur.com/tGJkEL9.jpg"
                        },
                        new
                        {
                            Id = 3,
                            ProductId = 2,
                            Url = "https://i.imgur.com/zw3Olrp.jpg"
                        },
                        new
                        {
                            Id = 4,
                            ProductId = 2,
                            Url = "https://i.imgur.com/9uEVXR1.png"
                        },
                        new
                        {
                            Id = 5,
                            ProductId = 3,
                            Url = "https://i.imgur.com/uHKlJtV.jpg"
                        },
                        new
                        {
                            Id = 6,
                            ProductId = 3,
                            Url = "https://i.imgur.com/MD7Wzx4.jpg"
                        });
                });

            modelBuilder.Entity("LouiseTieDyeStore.Shared.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<bool>("Deleted")
                        .HasColumnType("bit");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("OriginalPrice")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("ProductTypeId")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Visible")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("ProductTypeId");

                    b.ToTable("Products");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CategoryId = 1,
                            Deleted = false,
                            Description = "Blue and Yellow pattern on a womens ScoobyDoo t-shirt",
                            OriginalPrice = 35.00m,
                            Price = 30.00m,
                            ProductTypeId = 1,
                            Title = "Cool TyeDye Shirt",
                            Visible = true
                        },
                        new
                        {
                            Id = 2,
                            CategoryId = 2,
                            Deleted = false,
                            Description = "Blue and Yellow pattern on a Long Sleeve Shirt",
                            OriginalPrice = 25.00m,
                            Price = 20.00m,
                            ProductTypeId = 2,
                            Title = "Really Cool TyeDye Shirt",
                            Visible = true
                        },
                        new
                        {
                            Id = 3,
                            CategoryId = 3,
                            Deleted = false,
                            Description = "Blue and Red spiral pattern on a baby onesie.",
                            OriginalPrice = 15.00m,
                            Price = 10.00m,
                            ProductTypeId = 3,
                            Title = "TyeDye Onesie",
                            Visible = true
                        });
                });

            modelBuilder.Entity("LouiseTieDyeStore.Shared.ProductType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("ProductTypes");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "T-Shirt"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Long Sleeve Shirt"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Onesie"
                        });
                });

            modelBuilder.Entity("LouiseTieDyeStore.Shared.Image", b =>
                {
                    b.HasOne("LouiseTieDyeStore.Shared.Product", "Product")
                        .WithMany("Images")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");
                });

            modelBuilder.Entity("LouiseTieDyeStore.Shared.Product", b =>
                {
                    b.HasOne("LouiseTieDyeStore.Shared.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LouiseTieDyeStore.Shared.ProductType", "ProductType")
                        .WithMany()
                        .HasForeignKey("ProductTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");

                    b.Navigation("ProductType");
                });

            modelBuilder.Entity("LouiseTieDyeStore.Shared.Product", b =>
                {
                    b.Navigation("Images");
                });
#pragma warning restore 612, 618
        }
    }
}
