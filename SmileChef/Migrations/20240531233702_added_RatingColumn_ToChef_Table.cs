using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmileChef.Migrations
{
    /// <inheritdoc />
    public partial class added_RatingColumn_ToChef_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Rating",
                table: "Chefs",
                type: "int",
                maxLength: 5,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Chefs",
                keyColumn: "ChefId",
                keyValue: 1,
                column: "Rating",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Chefs",
                keyColumn: "ChefId",
                keyValue: 2,
                column: "Rating",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Chefs",
                keyColumn: "ChefId",
                keyValue: 3,
                column: "Rating",
                value: 5);

            migrationBuilder.UpdateData(
                table: "Chefs",
                keyColumn: "ChefId",
                keyValue: 4,
                column: "Rating",
                value: 4);

            migrationBuilder.UpdateData(
                table: "Chefs",
                keyColumn: "ChefId",
                keyValue: 5,
                column: "Rating",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Chefs",
                keyColumn: "ChefId",
                keyValue: 6,
                column: "Rating",
                value: 5);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rating",
                table: "Chefs");
        }
    }
}
