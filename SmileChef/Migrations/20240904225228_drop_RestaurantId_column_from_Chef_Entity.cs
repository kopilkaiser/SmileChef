using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmileChef.Migrations
{
    /// <inheritdoc />
    public partial class drop_RestaurantId_column_from_Chef_Entity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Chefs_Restaurants_RestaurantId",
                table: "Chefs");

            migrationBuilder.DropIndex(
                name: "IX_Restaurants_ChefId",
                table: "Restaurants");

            migrationBuilder.DropIndex(
                name: "IX_Chefs_RestaurantId",
                table: "Chefs");

            migrationBuilder.DropColumn(
                name: "RestaurantId",
                table: "Chefs");

            migrationBuilder.CreateIndex(
                name: "IX_Restaurants_ChefId",
                table: "Restaurants",
                column: "ChefId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Restaurants_ChefId",
                table: "Restaurants");

            migrationBuilder.AddColumn<int>(
                name: "RestaurantId",
                table: "Chefs",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Chefs",
                keyColumn: "ChefId",
                keyValue: 1,
                column: "RestaurantId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Chefs",
                keyColumn: "ChefId",
                keyValue: 2,
                column: "RestaurantId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Chefs",
                keyColumn: "ChefId",
                keyValue: 3,
                column: "RestaurantId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Chefs",
                keyColumn: "ChefId",
                keyValue: 4,
                column: "RestaurantId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Chefs",
                keyColumn: "ChefId",
                keyValue: 5,
                column: "RestaurantId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Chefs",
                keyColumn: "ChefId",
                keyValue: 6,
                column: "RestaurantId",
                value: null);

            migrationBuilder.CreateIndex(
                name: "IX_Restaurants_ChefId",
                table: "Restaurants",
                column: "ChefId");

            migrationBuilder.CreateIndex(
                name: "IX_Chefs_RestaurantId",
                table: "Chefs",
                column: "RestaurantId");

            migrationBuilder.AddForeignKey(
                name: "FK_Chefs_Restaurants_RestaurantId",
                table: "Chefs",
                column: "RestaurantId",
                principalTable: "Restaurants",
                principalColumn: "RestaurantId");
        }
    }
}
