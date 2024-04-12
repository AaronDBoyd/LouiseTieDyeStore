using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace LouiseTieDyeStore.Server.Migrations
{
    public partial class Postgres_Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "cart_items",
                columns: table => new
                {
                    user_email = table.Column<string>(type: "text", nullable: false),
                    product_id = table.Column<int>(type: "integer", nullable: false),
                    user_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_cart_items", x => new { x.user_email, x.product_id });
                });

            migrationBuilder.CreateTable(
                name: "categories",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    url = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_categories", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "messages",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    first_name = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    last_name = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    phone_number = table.Column<string>(type: "text", nullable: false),
                    email = table.Column<string>(type: "text", nullable: false),
                    date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    read = table.Column<bool>(type: "boolean", nullable: false),
                    body = table.Column<string>(type: "text", nullable: false),
                    subject = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_messages", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "orders",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    email = table.Column<string>(type: "text", nullable: false),
                    order_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    shipping_cost = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    sales_tax = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    sub_total = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    total_price = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_orders", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "product_types",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_product_types", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tax_rates",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    state = table.Column<string>(type: "text", nullable: false),
                    abbreviation = table.Column<string>(type: "text", nullable: false),
                    rate = table.Column<decimal>(type: "numeric(18,3)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_tax_rates", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    email = table.Column<string>(type: "text", nullable: false),
                    date_created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "addresses",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    order_id = table.Column<Guid>(type: "uuid", nullable: false),
                    first_name = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    last_name = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    phone_number = table.Column<string>(type: "text", nullable: false),
                    line_one = table.Column<string>(type: "text", nullable: false),
                    line_two = table.Column<string>(type: "text", nullable: false),
                    city = table.Column<string>(type: "text", nullable: false),
                    state = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    zip = table.Column<string>(type: "text", nullable: false),
                    country = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_addresses", x => x.id);
                    table.ForeignKey(
                        name: "fk_addresses_orders_order_id",
                        column: x => x.order_id,
                        principalTable: "orders",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "products",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    title = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    size = table.Column<string>(type: "text", nullable: false),
                    price = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    original_price = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    category_id = table.Column<int>(type: "integer", nullable: false),
                    product_type_id = table.Column<int>(type: "integer", nullable: false),
                    visible = table.Column<bool>(type: "boolean", nullable: false),
                    deleted = table.Column<bool>(type: "boolean", nullable: false),
                    sold = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_products", x => x.id);
                    table.ForeignKey(
                        name: "fk_products_categories_category_id",
                        column: x => x.category_id,
                        principalTable: "categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_products_product_types_product_type_id",
                        column: x => x.product_type_id,
                        principalTable: "product_types",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "images",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    url = table.Column<string>(type: "text", nullable: false),
                    product_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_images", x => x.id);
                    table.ForeignKey(
                        name: "fk_images_products_product_id",
                        column: x => x.product_id,
                        principalTable: "products",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "order_items",
                columns: table => new
                {
                    order_id = table.Column<Guid>(type: "uuid", nullable: false),
                    product_id = table.Column<int>(type: "integer", nullable: false),
                    price = table.Column<decimal>(type: "numeric(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_order_items", x => new { x.order_id, x.product_id });
                    table.ForeignKey(
                        name: "fk_order_items_orders_order_id",
                        column: x => x.order_id,
                        principalTable: "orders",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_order_items_products_product_id",
                        column: x => x.product_id,
                        principalTable: "products",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "categories",
                columns: new[] { "id", "name", "url" },
                values: new object[,]
                {
                    { 1, "Women", "women" },
                    { 2, "Men", "men" },
                    { 3, "Kids", "kids" }
                });

            migrationBuilder.InsertData(
                table: "product_types",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { 1, "T-Shirt" },
                    { 2, "Long Sleeve Shirt" },
                    { 3, "Onesie" }
                });

            migrationBuilder.InsertData(
                table: "tax_rates",
                columns: new[] { "id", "abbreviation", "rate", "state" },
                values: new object[,]
                {
                    { 1, "AL", 4.000m, "Alabama" },
                    { 2, "AK", 0.000m, "Alaska" },
                    { 3, "AZ", 5.600m, "Arizona" },
                    { 4, "AR", 6.500m, "Arkansas" },
                    { 5, "CA", 7.250m, "California" },
                    { 6, "CO", 2.900m, "Colorado" },
                    { 7, "CT", 6.350m, "Connecticut" },
                    { 8, "DE", 0.000m, "Delaware" },
                    { 9, "FL", 6.000m, "Florida" },
                    { 10, "GA", 4.000m, "Georgia" },
                    { 11, "HI", 4.000m, "Hawaii" },
                    { 12, "ID", 6.000m, "Idaho" },
                    { 13, "IL", 6.250m, "Illinois" },
                    { 14, "IN", 7.000m, "Indiana" },
                    { 15, "IA", 6.000m, "Iowa" },
                    { 16, "KS", 6.500m, "Kansas" },
                    { 17, "KY", 6.000m, "Kentucky" },
                    { 18, "LA", 4.450m, "Louisiana" },
                    { 19, "ME", 5.500m, "Maine" },
                    { 20, "MD", 6.000m, "Maryland" },
                    { 21, "MA", 6.250m, "Massachusetts" },
                    { 22, "MI", 6.000m, "Michigan" },
                    { 23, "MN", 6.875m, "Minnesota" },
                    { 24, "MS", 7.000m, "Mississippi" },
                    { 25, "MO", 4.225m, "Missouri" },
                    { 26, "MT", 0.000m, "Montana" },
                    { 27, "NE", 5.500m, "Nebraska" },
                    { 28, "NV", 6.850m, "Nevada" },
                    { 29, "NH", 0.000m, "New Hampshire" },
                    { 30, "NJ", 6.625m, "New Jersey" },
                    { 31, "MN", 4.875m, "New Mexico" },
                    { 32, "NY", 4.000m, "New York" },
                    { 33, "NC", 4.750m, "North Carolina" },
                    { 34, "ND", 5.000m, "North Dakota" },
                    { 35, "OH", 5.750m, "Ohio" },
                    { 36, "OK", 4.500m, "Oklahoma" },
                    { 37, "OR", 0.000m, "Oregon" },
                    { 38, "PA", 6.000m, "Pennsylvania" },
                    { 39, "RI", 7.000m, "Rhode Island" },
                    { 40, "SC", 6.000m, "South Carolina" },
                    { 41, "SD", 4.200m, "South Dakota" },
                    { 42, "TN", 7.000m, "Tennessee" },
                    { 43, "TX", 6.250m, "Texas" },
                    { 44, "UT", 6.100m, "Utah" },
                    { 45, "VT", 6.000m, "Vermont" },
                    { 46, "VA", 5.300m, "Virginia" },
                    { 47, "WA", 6.500m, "Washington" },
                    { 48, "WV", 6.000m, "West Virginia" },
                    { 49, "WI", 5.000m, "Wisconsin" },
                    { 50, "WY", 4.000m, "Wyoming" },
                    { 51, "DC", 6.000m, "District of Columbia" }
                });

            migrationBuilder.InsertData(
                table: "products",
                columns: new[] { "id", "category_id", "deleted", "description", "original_price", "price", "product_type_id", "size", "sold", "title", "visible" },
                values: new object[,]
                {
                    { 1, 1, false, "Blue and Yellow pattern on a womens ScoobyDoo t-shirt", 35.00m, 30.00m, 1, "XL", false, "Cool TyeDye Shirt", true },
                    { 2, 2, false, "Blue and Yellow pattern on a Long Sleeve Shirt", 25.00m, 20.00m, 2, "S", false, "Really Cool TyeDye Shirt", true },
                    { 3, 3, false, "Blue and Red spiral pattern on a baby onesie.", 15.00m, 10.00m, 3, "M", false, "TyeDye Onesie", true }
                });

            migrationBuilder.InsertData(
                table: "images",
                columns: new[] { "id", "product_id", "url" },
                values: new object[,]
                {
                    { 1, 1, "https://i.imgur.com/1DnQj7V.jpg" },
                    { 2, 1, "https://i.imgur.com/tGJkEL9.jpg" },
                    { 3, 2, "https://i.imgur.com/zw3Olrp.jpg" },
                    { 4, 2, "https://i.imgur.com/9uEVXR1.png" },
                    { 5, 3, "https://i.imgur.com/uHKlJtV.jpg" },
                    { 6, 3, "https://i.imgur.com/MD7Wzx4.jpg" }
                });

            migrationBuilder.CreateIndex(
                name: "ix_addresses_order_id",
                table: "addresses",
                column: "order_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_images_product_id",
                table: "images",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "ix_order_items_product_id",
                table: "order_items",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "ix_products_category_id",
                table: "products",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "ix_products_product_type_id",
                table: "products",
                column: "product_type_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "addresses");

            migrationBuilder.DropTable(
                name: "cart_items");

            migrationBuilder.DropTable(
                name: "images");

            migrationBuilder.DropTable(
                name: "messages");

            migrationBuilder.DropTable(
                name: "order_items");

            migrationBuilder.DropTable(
                name: "tax_rates");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "orders");

            migrationBuilder.DropTable(
                name: "products");

            migrationBuilder.DropTable(
                name: "categories");

            migrationBuilder.DropTable(
                name: "product_types");
        }
    }
}
