using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmileChef.Migrations
{
    /// <inheritdoc />
    public partial class Added_ImageUrl_for_Recipes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Recipes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Recipes",
                keyColumn: "RecipeId",
                keyValue: 1,
                column: "ImageUrl",
                value: "recipe1.jpeg");

            migrationBuilder.UpdateData(
                table: "Recipes",
                keyColumn: "RecipeId",
                keyValue: 2,
                column: "ImageUrl",
                value: "recipe2.jpeg");

            migrationBuilder.UpdateData(
                table: "Recipes",
                keyColumn: "RecipeId",
                keyValue: 3,
                column: "ImageUrl",
                value: "recipe3.jpeg");

            migrationBuilder.UpdateData(
                table: "Recipes",
                keyColumn: "RecipeId",
                keyValue: 4,
                column: "ImageUrl",
                value: "recipe4.jpeg");

            migrationBuilder.UpdateData(
                table: "Recipes",
                keyColumn: "RecipeId",
                keyValue: 5,
                column: "ImageUrl",
                value: "recipe5.jpeg");

            migrationBuilder.UpdateData(
                table: "Recipes",
                keyColumn: "RecipeId",
                keyValue: 6,
                column: "ImageUrl",
                value: "recipe6.jpeg");

            migrationBuilder.UpdateData(
                table: "Recipes",
                keyColumn: "RecipeId",
                keyValue: 7,
                column: "ImageUrl",
                value: "recipe7.jpeg");

            migrationBuilder.UpdateData(
                table: "Recipes",
                keyColumn: "RecipeId",
                keyValue: 8,
                column: "ImageUrl",
                value: "recipe8.jpeg");

            migrationBuilder.UpdateData(
                table: "Recipes",
                keyColumn: "RecipeId",
                keyValue: 9,
                column: "ImageUrl",
                value: "recipe2.jpeg");

            migrationBuilder.UpdateData(
                table: "Recipes",
                keyColumn: "RecipeId",
                keyValue: 10,
                column: "ImageUrl",
                value: "recipe3.jpeg");

            migrationBuilder.UpdateData(
                table: "Recipes",
                keyColumn: "RecipeId",
                keyValue: 11,
                column: "ImageUrl",
                value: "recipe4.jpeg");

            migrationBuilder.UpdateData(
                table: "Recipes",
                keyColumn: "RecipeId",
                keyValue: 12,
                column: "ImageUrl",
                value: "recipe5.jpeg");

            migrationBuilder.UpdateData(
                table: "Recipes",
                keyColumn: "RecipeId",
                keyValue: 13,
                column: "ImageUrl",
                value: "recipe6.jpeg");

            migrationBuilder.UpdateData(
                table: "Recipes",
                keyColumn: "RecipeId",
                keyValue: 14,
                column: "ImageUrl",
                value: "recipe7.jpeg");

            migrationBuilder.UpdateData(
                table: "Recipes",
                keyColumn: "RecipeId",
                keyValue: 15,
                column: "ImageUrl",
                value: "recipe9.jpeg");

            migrationBuilder.UpdateData(
                table: "Recipes",
                keyColumn: "RecipeId",
                keyValue: 16,
                column: "ImageUrl",
                value: "recipe10.jpeg");

            migrationBuilder.UpdateData(
                table: "Recipes",
                keyColumn: "RecipeId",
                keyValue: 17,
                column: "ImageUrl",
                value: "recipe11.jpeg");

            migrationBuilder.UpdateData(
                table: "Recipes",
                keyColumn: "RecipeId",
                keyValue: 18,
                column: "ImageUrl",
                value: "recipe12.jpeg");

            migrationBuilder.UpdateData(
                table: "Recipes",
                keyColumn: "RecipeId",
                keyValue: 19,
                column: "ImageUrl",
                value: "recipe13.jpeg");

            migrationBuilder.UpdateData(
                table: "Recipes",
                keyColumn: "RecipeId",
                keyValue: 20,
                column: "ImageUrl",
                value: "recipe8.jpeg");

            migrationBuilder.UpdateData(
                table: "Recipes",
                keyColumn: "RecipeId",
                keyValue: 21,
                column: "ImageUrl",
                value: "recipe9.jpeg");

            migrationBuilder.UpdateData(
                table: "Recipes",
                keyColumn: "RecipeId",
                keyValue: 22,
                column: "ImageUrl",
                value: "recipe10.jpeg");

            migrationBuilder.UpdateData(
                table: "Recipes",
                keyColumn: "RecipeId",
                keyValue: 23,
                column: "ImageUrl",
                value: "recipe11.jpeg");

            migrationBuilder.UpdateData(
                table: "Recipes",
                keyColumn: "RecipeId",
                keyValue: 24,
                column: "ImageUrl",
                value: "recipe14.jpeg");

            migrationBuilder.UpdateData(
                table: "Recipes",
                keyColumn: "RecipeId",
                keyValue: 25,
                column: "ImageUrl",
                value: "recipe15.jpeg");

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "RestaurantId",
                keyValue: 2,
                columns: new[] { "Lat", "Lng", "Location" },
                values: new object[] { 51.48230385097871, 0.16090573471658554, "Located in Erith" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Recipes");

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "RestaurantId",
                keyValue: 2,
                columns: new[] { "Lat", "Lng", "Location" },
                values: new object[] { 51.498995498502502, -0.11582109991560025, "Located in Central London" });
        }
    }
}
