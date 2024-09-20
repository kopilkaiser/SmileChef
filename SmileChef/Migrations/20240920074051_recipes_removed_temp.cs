using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SmileChef.Migrations
{
    /// <inheritdoc />
    public partial class recipes_removed_temp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Recipes",
                keyColumn: "RecipeId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Recipes",
                keyColumn: "RecipeId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Recipes",
                keyColumn: "RecipeId",
                keyValue: 19);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Recipes",
                columns: new[] { "RecipeId", "ChefId", "ImageUrl", "Name", "Price", "RecipeType" },
                values: new object[,]
                {
                    { 1, 1, "recipe1.jpeg", "Turmeric Rice Chicken", 40.99f, "Premium" },
                    { 7, 1, "recipe7.jpeg", "Aubergine Curry Vegetables", 58.99f, "Premium" },
                    { 19, 6, "recipe13.jpeg", "Crispy Shredded Beef", 25.99f, "Premium" }
                });
        }
    }
}
