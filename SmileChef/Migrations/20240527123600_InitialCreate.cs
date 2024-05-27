using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SmileChef.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Chefs",
                columns: table => new
                {
                    ChefId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chefs", x => x.ChefId);
                    table.ForeignKey(
                        name: "FK_Chefs_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Recipes",
                columns: table => new
                {
                    RecipeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ChefId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recipes", x => x.RecipeId);
                    table.ForeignKey(
                        name: "FK_Recipes_Chefs_ChefId",
                        column: x => x.ChefId,
                        principalTable: "Chefs",
                        principalColumn: "ChefId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Subscriptions",
                columns: table => new
                {
                    SubscriptionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubscriberId = table.Column<int>(type: "int", nullable: false),
                    PublisherId = table.Column<int>(type: "int", nullable: false),
                    SubscriptionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: false),
                    SubscriptionType = table.Column<string>(type: "nvarchar(25)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscriptions", x => x.SubscriptionId);
                    table.ForeignKey(
                        name: "FK_Subscriptions_Chefs_PublisherId",
                        column: x => x.PublisherId,
                        principalTable: "Chefs",
                        principalColumn: "ChefId");
                    table.ForeignKey(
                        name: "FK_Subscriptions_Chefs_SubscriberId",
                        column: x => x.SubscriberId,
                        principalTable: "Chefs",
                        principalColumn: "ChefId");
                });

            migrationBuilder.CreateTable(
                name: "Instructions",
                columns: table => new
                {
                    InstructionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RecipeId = table.Column<int>(type: "int", nullable: false),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    Duration = table.Column<TimeSpan>(type: "time", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Instructions", x => x.InstructionId);
                    table.ForeignKey(
                        name: "FK_Instructions_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipes",
                        principalColumn: "RecipeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "Email", "Password" },
                values: new object[,]
                {
                    { 1, "gordan@gmail.com", "123" },
                    { 2, "jamie@gmail.com", "123" },
                    { 3, "wolfgang@gmail.com", "123" },
                    { 4, "alice@gmail.com", "123" },
                    { 5, "thomas@gmail.com", "123" },
                    { 6, "emeril@gmail.com", "123" }
                });

            migrationBuilder.InsertData(
                table: "Chefs",
                columns: new[] { "ChefId", "FirstName", "LastName", "UserId" },
                values: new object[,]
                {
                    { 1, "Gordon", "Ramsay", 1 },
                    { 2, "Jamie", "Oliver", 2 },
                    { 3, "Wolfgang", "Puck", 3 },
                    { 4, "Alice", "Waters", 4 },
                    { 5, "Thomas", "Keller", 5 },
                    { 6, "Emeril", "Lagasse", 6 }
                });

            migrationBuilder.InsertData(
                table: "Recipes",
                columns: new[] { "RecipeId", "ChefId", "Name" },
                values: new object[,]
                {
                    { 1, 1, "Beef Wellington" },
                    { 2, 2, "Pasta Carbonara" },
                    { 3, 3, "Spicy Tuna Rolls" },
                    { 4, 4, "Lentil Soup" },
                    { 5, 5, "Roast Chicken" },
                    { 6, 6, "Bananas Foster" },
                    { 7, 1, "Beef Bolognese" },
                    { 8, 1, "Chicken Mushroom Soup" }
                });

            migrationBuilder.InsertData(
                table: "Subscriptions",
                columns: new[] { "SubscriptionId", "Amount", "PublisherId", "SubscriberId", "SubscriptionDate", "SubscriptionType" },
                values: new object[,]
                {
                    { 1, 99.99m, 1, 3, new DateTime(2024, 5, 17, 0, 0, 0, 0, DateTimeKind.Local), "Monthly" },
                    { 2, 99.99m, 1, 4, new DateTime(2024, 5, 17, 0, 0, 0, 0, DateTimeKind.Local), "Monthly" },
                    { 3, 99.99m, 1, 5, new DateTime(2024, 5, 17, 0, 0, 0, 0, DateTimeKind.Local), "Monthly" },
                    { 4, 199.99m, 2, 4, new DateTime(2024, 5, 17, 0, 0, 0, 0, DateTimeKind.Local), "Yearly" },
                    { 5, 199.99m, 2, 5, new DateTime(2024, 5, 17, 0, 0, 0, 0, DateTimeKind.Local), "Yearly" },
                    { 6, 199.99m, 2, 6, new DateTime(2024, 5, 17, 0, 0, 0, 0, DateTimeKind.Local), "Yearly" }
                });

            migrationBuilder.InsertData(
                table: "Instructions",
                columns: new[] { "InstructionId", "Description", "Duration", "OrderId", "RecipeId" },
                values: new object[,]
                {
                    { 1, "Prepare beef", new TimeSpan(0, 0, 10, 0, 0), 1, 1 },
                    { 2, "Wrap in puff pastry", null, 2, 1 },
                    { 3, "Bake", new TimeSpan(0, 0, 20, 0, 0), 3, 1 },
                    { 4, "Cook pasta", new TimeSpan(0, 0, 15, 0, 0), 1, 2 },
                    { 5, "Prepare sauce", null, 2, 2 },
                    { 6, "Combine and serve", new TimeSpan(0, 0, 3, 0, 0), 3, 2 },
                    { 7, "Prepare tuna", null, 1, 3 },
                    { 8, "Roll sushi", new TimeSpan(0, 0, 5, 0, 0), 2, 3 },
                    { 9, "Serve with soy sauce", null, 3, 3 },
                    { 10, "Prepare lentils", null, 1, 4 },
                    { 11, "Cook lentils", new TimeSpan(0, 0, 15, 0, 0), 2, 4 },
                    { 12, "Serve hot", null, 3, 4 },
                    { 13, "Season chicken", null, 1, 5 },
                    { 14, "Roast chicken", new TimeSpan(0, 0, 45, 0, 0), 2, 5 },
                    { 15, "Serve with vegetables", null, 3, 5 },
                    { 16, "Prepare bananas", new TimeSpan(0, 0, 15, 0, 0), 1, 6 },
                    { 17, "Cook with butter and sugar", null, 2, 6 },
                    { 18, "Serve with ice cream", null, 3, 6 },
                    { 19, "Cook Mince Beef", new TimeSpan(0, 0, 10, 0, 0), 1, 7 },
                    { 20, "Cook Tomato Sauce", new TimeSpan(0, 0, 25, 0, 0), 2, 7 },
                    { 21, "Boil Sphagetti", new TimeSpan(0, 0, 15, 0, 0), 3, 7 },
                    { 22, "Mix Cooked Beef, Suace, and Sphagetti", null, 4, 7 },
                    { 23, "Cook Chicken Soup", new TimeSpan(0, 0, 35, 0, 0), 1, 8 },
                    { 24, "Add chopped Mushroom, Cillantro, Corriander", new TimeSpan(0, 0, 2, 0, 0), 2, 8 },
                    { 25, "Simmer the ingredients", new TimeSpan(0, 0, 5, 0, 0), 3, 8 },
                    { 26, "Plate the soup with sprinkled corriander and chillies", null, 4, 8 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Chefs_UserId",
                table: "Chefs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Instructions_RecipeId",
                table: "Instructions",
                column: "RecipeId");

            migrationBuilder.CreateIndex(
                name: "IX_Recipes_ChefId",
                table: "Recipes",
                column: "ChefId");

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_PublisherId",
                table: "Subscriptions",
                column: "PublisherId");

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_SubscriberId",
                table: "Subscriptions",
                column: "SubscriberId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Instructions");

            migrationBuilder.DropTable(
                name: "Subscriptions");

            migrationBuilder.DropTable(
                name: "Recipes");

            migrationBuilder.DropTable(
                name: "Chefs");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
