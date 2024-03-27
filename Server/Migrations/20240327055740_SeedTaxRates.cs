using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LouiseTieDyeStore.Server.Migrations
{
    public partial class SeedTaxRates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "TaxRates",
                columns: new[] { "Id", "Abbreviation", "Rate", "State" },
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
                    { 42, "TN", 7.000m, "Tennessee" }
                });

            migrationBuilder.InsertData(
                table: "TaxRates",
                columns: new[] { "Id", "Abbreviation", "Rate", "State" },
                values: new object[,]
                {
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "TaxRates",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "TaxRates",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "TaxRates",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "TaxRates",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "TaxRates",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "TaxRates",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "TaxRates",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "TaxRates",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "TaxRates",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "TaxRates",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "TaxRates",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "TaxRates",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "TaxRates",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "TaxRates",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "TaxRates",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "TaxRates",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "TaxRates",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "TaxRates",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "TaxRates",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "TaxRates",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "TaxRates",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "TaxRates",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "TaxRates",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "TaxRates",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "TaxRates",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "TaxRates",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "TaxRates",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "TaxRates",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "TaxRates",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "TaxRates",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "TaxRates",
                keyColumn: "Id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "TaxRates",
                keyColumn: "Id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "TaxRates",
                keyColumn: "Id",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "TaxRates",
                keyColumn: "Id",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "TaxRates",
                keyColumn: "Id",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "TaxRates",
                keyColumn: "Id",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "TaxRates",
                keyColumn: "Id",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "TaxRates",
                keyColumn: "Id",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "TaxRates",
                keyColumn: "Id",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "TaxRates",
                keyColumn: "Id",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "TaxRates",
                keyColumn: "Id",
                keyValue: 41);

            migrationBuilder.DeleteData(
                table: "TaxRates",
                keyColumn: "Id",
                keyValue: 42);

            migrationBuilder.DeleteData(
                table: "TaxRates",
                keyColumn: "Id",
                keyValue: 43);

            migrationBuilder.DeleteData(
                table: "TaxRates",
                keyColumn: "Id",
                keyValue: 44);

            migrationBuilder.DeleteData(
                table: "TaxRates",
                keyColumn: "Id",
                keyValue: 45);

            migrationBuilder.DeleteData(
                table: "TaxRates",
                keyColumn: "Id",
                keyValue: 46);

            migrationBuilder.DeleteData(
                table: "TaxRates",
                keyColumn: "Id",
                keyValue: 47);

            migrationBuilder.DeleteData(
                table: "TaxRates",
                keyColumn: "Id",
                keyValue: 48);

            migrationBuilder.DeleteData(
                table: "TaxRates",
                keyColumn: "Id",
                keyValue: 49);

            migrationBuilder.DeleteData(
                table: "TaxRates",
                keyColumn: "Id",
                keyValue: 50);

            migrationBuilder.DeleteData(
                table: "TaxRates",
                keyColumn: "Id",
                keyValue: 51);
        }
    }
}
