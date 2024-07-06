using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmileChef.Migrations
{
    /// <inheritdoc />
    public partial class new_migration_01 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "AccountBalance",
                table: "Chefs",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "SubscriptionCost",
                table: "Chefs",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "NotifySubscribers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PublisherId = table.Column<int>(type: "int", nullable: false),
                    RecipeId = table.Column<int>(type: "int", nullable: false),
                    SubscriberId = table.Column<int>(type: "int", nullable: false),
                    Notified = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotifySubscribers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NotifySubscribers_Chefs_PublisherId",
                        column: x => x.PublisherId,
                        principalTable: "Chefs",
                        principalColumn: "ChefId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NotifySubscribers_Chefs_SubscriberId",
                        column: x => x.SubscriberId,
                        principalTable: "Chefs",
                        principalColumn: "ChefId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NotifySubscribers_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipes",
                        principalColumn: "RecipeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "Chefs",
                keyColumn: "ChefId",
                keyValue: 1,
                columns: new[] { "AccountBalance", "SubscriptionCost" },
                values: new object[] { 10000m, 12.99m });

            migrationBuilder.UpdateData(
                table: "Chefs",
                keyColumn: "ChefId",
                keyValue: 2,
                columns: new[] { "AccountBalance", "SubscriptionCost" },
                values: new object[] { 5500m, 11.99m });

            migrationBuilder.UpdateData(
                table: "Chefs",
                keyColumn: "ChefId",
                keyValue: 3,
                columns: new[] { "AccountBalance", "SubscriptionCost" },
                values: new object[] { 9000m, 5.99m });

            migrationBuilder.UpdateData(
                table: "Chefs",
                keyColumn: "ChefId",
                keyValue: 4,
                columns: new[] { "AccountBalance", "SubscriptionCost" },
                values: new object[] { 15000m, 6.99m });

            migrationBuilder.UpdateData(
                table: "Chefs",
                keyColumn: "ChefId",
                keyValue: 5,
                columns: new[] { "AccountBalance", "SubscriptionCost" },
                values: new object[] { 8000m, 15.99m });

            migrationBuilder.UpdateData(
                table: "Chefs",
                keyColumn: "ChefId",
                keyValue: 6,
                columns: new[] { "AccountBalance", "SubscriptionCost" },
                values: new object[] { 7000m, 10.99m });

            migrationBuilder.CreateIndex(
                name: "IX_NotifySubscribers_PublisherId",
                table: "NotifySubscribers",
                column: "PublisherId");

            migrationBuilder.CreateIndex(
                name: "IX_NotifySubscribers_RecipeId",
                table: "NotifySubscribers",
                column: "RecipeId");

            migrationBuilder.CreateIndex(
                name: "IX_NotifySubscribers_SubscriberId",
                table: "NotifySubscribers",
                column: "SubscriberId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NotifySubscribers");

            migrationBuilder.DropColumn(
                name: "AccountBalance",
                table: "Chefs");

            migrationBuilder.DropColumn(
                name: "SubscriptionCost",
                table: "Chefs");
        }
    }
}
