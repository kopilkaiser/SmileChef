using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SmileChef.Migrations
{
    /// <inheritdoc />
    public partial class RecipeBookmark_entity_added : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Recipes",
                keyColumn: "RecipeId",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Recipes",
                keyColumn: "RecipeId",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Recipes",
                keyColumn: "RecipeId",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Recipes",
                keyColumn: "RecipeId",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Recipes",
                keyColumn: "RecipeId",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Recipes",
                keyColumn: "RecipeId",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Recipes",
                keyColumn: "RecipeId",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Recipes",
                keyColumn: "RecipeId",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Recipes",
                keyColumn: "RecipeId",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Recipes",
                keyColumn: "RecipeId",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Recipes",
                keyColumn: "RecipeId",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Recipes",
                keyColumn: "RecipeId",
                keyValue: 25);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Recipes",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "RecipeBookmarks",
                columns: table => new
                {
                    RecipeBookmarkId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecipeId = table.Column<int>(type: "int", nullable: true),
                    ChefId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipeBookmarks", x => x.RecipeBookmarkId);
                    table.ForeignKey(
                        name: "FK_RecipeBookmarks_Chefs_ChefId",
                        column: x => x.ChefId,
                        principalTable: "Chefs",
                        principalColumn: "ChefId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RecipeBookmarks_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipes",
                        principalColumn: "RecipeId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.InsertData(
                table: "Recipes",
                columns: new[] { "RecipeId", "ChefId", "Name", "Price", "RecipeType" },
                values: new object[,]
                {
                    { 9, 1, "Gourmet Beef Wellington", 49.99f, "Premium" },
                    { 10, 2, "Truffle Risotto", 39.99f, "Premium" },
                    { 11, 3, "Sushi Deluxe", 29.99f, "Premium" },
                    { 12, 4, "Organic Farm-to-Table Salad", 19.99f, "Premium" },
                    { 13, 5, "Foie Gras Terrine", 59.99f, "Premium" },
                    { 14, 6, "Emeril's Essence Special", 24.99f, "Premium" },
                    { 20, 1, "Lobster Thermidor", 69.99f, "Premium" },
                    { 21, 2, "Caviar Blinis", 99.99f, "Premium" },
                    { 22, 3, "Wagyu Beef Sushi", 89.99f, "Premium" },
                    { 23, 4, "Truffle Mushroom Soup", 34.99f, "Premium" },
                    { 24, 5, "Duck à l'Orange", 54.99f, "Premium" },
                    { 25, 6, "Oysters Rockefeller", 44.99f, "Premium" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_RecipeBookmarks_ChefId",
                table: "RecipeBookmarks",
                column: "ChefId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeBookmarks_RecipeId",
                table: "RecipeBookmarks",
                column: "RecipeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RecipeBookmarks");

            migrationBuilder.DeleteData(
                table: "Recipes",
                keyColumn: "RecipeId",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Recipes",
                keyColumn: "RecipeId",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Recipes",
                keyColumn: "RecipeId",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Recipes",
                keyColumn: "RecipeId",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Recipes",
                keyColumn: "RecipeId",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Recipes",
                keyColumn: "RecipeId",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Recipes",
                keyColumn: "RecipeId",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Recipes",
                keyColumn: "RecipeId",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Recipes",
                keyColumn: "RecipeId",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Recipes",
                keyColumn: "RecipeId",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Recipes",
                keyColumn: "RecipeId",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Recipes",
                keyColumn: "RecipeId",
                keyValue: 25);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Recipes",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.InsertData(
                table: "Recipes",
                columns: new[] { "RecipeId", "ChefId", "Name", "Price", "RecipeType" },
                values: new object[,]
                {
                    { 9, 1, "Gourmet Beef Wellington", 49.99f, "Basic" },
                    { 10, 2, "Truffle Risotto", 39.99f, "Basic" },
                    { 11, 3, "Sushi Deluxe", 29.99f, "Basic" },
                    { 12, 4, "Organic Farm-to-Table Salad", 19.99f, "Basic" },
                    { 13, 5, "Foie Gras Terrine", 59.99f, "Basic" },
                    { 14, 6, "Emeril's Essence Special", 24.99f, "Basic" },
                    { 20, 1, "Lobster Thermidor", 69.99f, "Basic" },
                    { 21, 2, "Caviar Blinis", 99.99f, "Basic" },
                    { 22, 3, "Wagyu Beef Sushi", 89.99f, "Basic" },
                    { 23, 4, "Truffle Mushroom Soup", 34.99f, "Basic" },
                    { 24, 5, "Duck à l'Orange", 54.99f, "Basic" },
                    { 25, 6, "Oysters Rockefeller", 44.99f, "Basic" }
                });
        }
    }
}
