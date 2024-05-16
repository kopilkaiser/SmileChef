using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ChefApp.Migrations
{
    /// <inheritdoc />
    public partial class Added_SeedData_To_ChefApplication : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Chefs",
                columns: new[] { "ChefId", "FirstName", "LastName" },
                values: new object[,]
                {
                    { 1, "Gordon", "Ramsay" },
                    { 2, "Jamie", "Oliver" },
                    { 3, "Wolfgang", "Puck" },
                    { 4, "Alice", "Waters" },
                    { 5, "Thomas", "Keller" },
                    { 6, "Emeril", "Lagasse" }
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
                    { 6, 6, "Bananas Foster" }
                });

            migrationBuilder.InsertData(
                table: "Subscriptions",
                columns: new[] { "SubscriptionId", "Amount", "PublisherId", "SubscriberId", "SubscriptionDate", "SubscriptionType" },
                values: new object[,]
                {
                    { 1, 99.99m, 1, 3, new DateTime(2024, 5, 17, 0, 10, 44, 159, DateTimeKind.Local).AddTicks(3432), "Monthly" },
                    { 2, 99.99m, 1, 4, new DateTime(2024, 5, 17, 0, 10, 44, 159, DateTimeKind.Local).AddTicks(3492), "Monthly" },
                    { 3, 99.99m, 1, 5, new DateTime(2024, 5, 17, 0, 10, 44, 159, DateTimeKind.Local).AddTicks(3494), "Monthly" },
                    { 4, 199.99m, 2, 4, new DateTime(2024, 5, 17, 0, 10, 44, 159, DateTimeKind.Local).AddTicks(3496), "Yearly" },
                    { 5, 199.99m, 2, 5, new DateTime(2024, 5, 17, 0, 10, 44, 159, DateTimeKind.Local).AddTicks(3498), "Yearly" },
                    { 6, 199.99m, 2, 6, new DateTime(2024, 5, 17, 0, 10, 44, 159, DateTimeKind.Local).AddTicks(3500), "Yearly" }
                });

            migrationBuilder.InsertData(
                table: "Instructions",
                columns: new[] { "InstructionId", "Description", "OrderId", "RecipeId" },
                values: new object[,]
                {
                    { 1, "Prepare beef", 1, 1 },
                    { 2, "Wrap in puff pastry", 2, 1 },
                    { 3, "Bake", 3, 1 },
                    { 4, "Cook pasta", 1, 2 },
                    { 5, "Prepare sauce", 2, 2 },
                    { 6, "Combine and serve", 3, 2 },
                    { 7, "Prepare tuna", 1, 3 },
                    { 8, "Roll sushi", 2, 3 },
                    { 9, "Serve with soy sauce", 3, 3 },
                    { 10, "Prepare lentils", 1, 4 },
                    { 11, "Cook lentils", 2, 4 },
                    { 12, "Serve hot", 3, 4 },
                    { 13, "Season chicken", 1, 5 },
                    { 14, "Roast chicken", 2, 5 },
                    { 15, "Serve with vegetables", 3, 5 },
                    { 16, "Prepare bananas", 1, 6 },
                    { 17, "Cook with butter and sugar", 2, 6 },
                    { 18, "Serve with ice cream", 3, 6 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Instructions",
                keyColumn: "InstructionId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Instructions",
                keyColumn: "InstructionId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Instructions",
                keyColumn: "InstructionId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Instructions",
                keyColumn: "InstructionId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Instructions",
                keyColumn: "InstructionId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Instructions",
                keyColumn: "InstructionId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Instructions",
                keyColumn: "InstructionId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Instructions",
                keyColumn: "InstructionId",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Instructions",
                keyColumn: "InstructionId",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Instructions",
                keyColumn: "InstructionId",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Instructions",
                keyColumn: "InstructionId",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Instructions",
                keyColumn: "InstructionId",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Instructions",
                keyColumn: "InstructionId",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Instructions",
                keyColumn: "InstructionId",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Instructions",
                keyColumn: "InstructionId",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Instructions",
                keyColumn: "InstructionId",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Instructions",
                keyColumn: "InstructionId",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Instructions",
                keyColumn: "InstructionId",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Subscriptions",
                keyColumn: "SubscriptionId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Subscriptions",
                keyColumn: "SubscriptionId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Subscriptions",
                keyColumn: "SubscriptionId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Subscriptions",
                keyColumn: "SubscriptionId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Subscriptions",
                keyColumn: "SubscriptionId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Subscriptions",
                keyColumn: "SubscriptionId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Recipes",
                keyColumn: "RecipeId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Recipes",
                keyColumn: "RecipeId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Recipes",
                keyColumn: "RecipeId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Recipes",
                keyColumn: "RecipeId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Recipes",
                keyColumn: "RecipeId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Recipes",
                keyColumn: "RecipeId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Chefs",
                keyColumn: "ChefId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Chefs",
                keyColumn: "ChefId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Chefs",
                keyColumn: "ChefId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Chefs",
                keyColumn: "ChefId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Chefs",
                keyColumn: "ChefId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Chefs",
                keyColumn: "ChefId",
                keyValue: 6);
        }
    }
}
