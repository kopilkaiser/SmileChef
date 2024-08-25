using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SmileChef.Migrations
{
    /// <inheritdoc />
    public partial class Added_PremiumRecipes_For_RecipeMarketPlace : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {          
            migrationBuilder.AddColumn<float>(
                name: "Price",
                table: "Recipes",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RecipeType",
                table: "Recipes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "Basic");

            // Update existing records with default values
            migrationBuilder.Sql("UPDATE Recipes SET RecipeType = 'Basic' WHERE RecipeType IS NULL;");

            migrationBuilder.InsertData(
                table: "Recipes",
                columns: new[] { "RecipeId", "ChefId", "Name", "RecipeType" },
                values: new object[,]
                {
                    { 1, 1, "Beef Wellington", "Basic" },
                    { 2, 2, "Pasta Carbonara", "Basic" },
                    { 3, 3, "Spicy Tuna Rolls", "Basic" },
                    { 4, 4, "Lentil Soup", "Basic" },
                    { 5, 5, "Roast Chicken", "Basic" },
                    { 6, 6, "Bananas Foster", "Basic" },
                    { 7, 1, "Beef Bolognese", "Basic" },
                    { 8, 1, "Chicken Mushroom Soup", "Basic" }
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
                    { 14, 6, "Emeril's Essence Special", 24.99f, "Premium" }
                });

            migrationBuilder.InsertData(
                table: "Recipes",
                columns: new[] { "RecipeId", "ChefId", "Name", "RecipeType" },
                values: new object[,]
                {
                    { 15, 2, "Classic Margherita Pizza", "Basic" },
                    { 16, 3, "Seafood Paella", "Basic" },
                    { 17, 4, "Vegetarian Stir Fry", "Basic" },
                    { 18, 5, "Slow Cooked Lamb Shank", "Basic" },
                    { 19, 6, "Creole Gumbo", "Basic" }
                });

            migrationBuilder.InsertData(
                table: "Recipes",
                columns: new[] { "RecipeId", "ChefId", "Name", "Price", "RecipeType" },
                values: new object[,]
                {
                    { 20, 1, "Lobster Thermidor", 69.99f, "Premium" },
                    { 21, 2, "Caviar Blinis", 99.99f, "Premium" },
                    { 22, 3, "Wagyu Beef Sushi", 89.99f, "Premium" },
                    { 23, 4, "Truffle Mushroom Soup", 34.99f, "Premium" },
                    { 24, 5, "Duck à l'Orange", 54.99f, "Premium" },
                    { 25, 6, "Oysters Rockefeller", 44.99f, "Premium" }
                });

            migrationBuilder.InsertData(
                table: "Instructions",
                columns: new[] { "InstructionId", "Description", "Duration", "OrderId", "RecipeId" },
                values: new object[,]
                {
                    { 27, "Prepare gourmet beef", new TimeSpan(0, 0, 15, 0, 0), 1, 9 },
                    { 28, "Wrap with prosciutto and puff pastry", null, 2, 9 },
                    { 29, "Bake to perfection", new TimeSpan(0, 0, 25, 0, 0), 3, 9 },
                    { 30, "Prepare truffle stock", new TimeSpan(0, 0, 10, 0, 0), 1, 10 },
                    { 31, "Cook Arborio rice", new TimeSpan(0, 0, 18, 0, 0), 2, 10 },
                    { 32, "Add truffle and parmesan", null, 3, 10 },
                    { 33, "Prepare sushi-grade tuna", null, 1, 11 },
                    { 34, "Roll with sushi rice and nori", new TimeSpan(0, 0, 8, 0, 0), 2, 11 },
                    { 35, "Garnish with caviar", null, 3, 11 },
                    { 36, "Chop organic vegetables", new TimeSpan(0, 0, 12, 0, 0), 1, 12 },
                    { 37, "Prepare vinaigrette dressing", null, 2, 12 },
                    { 38, "Toss and serve fresh", null, 3, 12 },
                    { 39, "Prepare foie gras", new TimeSpan(0, 0, 15, 0, 0), 1, 13 },
                    { 40, "Cook terrine with truffles", new TimeSpan(0, 0, 30, 0, 0), 2, 13 },
                    { 41, "Serve with toasted brioche", null, 3, 13 },
                    { 42, "Mix Emeril's special spices", null, 1, 14 },
                    { 43, "Marinate the meat", new TimeSpan(0, 0, 20, 0, 0), 2, 14 },
                    { 44, "Cook to desired doneness", null, 3, 14 },
                    { 45, "Prepare pizza dough", new TimeSpan(0, 0, 15, 0, 0), 1, 15 },
                    { 46, "Add tomato sauce and mozzarella", null, 2, 15 },
                    { 47, "Bake in oven", new TimeSpan(0, 0, 10, 0, 0), 3, 15 },
                    { 48, "Cook seafood", new TimeSpan(0, 0, 15, 0, 0), 1, 16 },
                    { 49, "Prepare paella rice", null, 2, 16 },
                    { 50, "Combine seafood with rice", new TimeSpan(0, 0, 20, 0, 0), 3, 16 },
                    { 51, "Chop vegetables", new TimeSpan(0, 0, 10, 0, 0), 1, 17 },
                    { 52, "Stir fry in wok", null, 2, 17 },
                    { 53, "Serve with soy sauce", new TimeSpan(0, 0, 5, 0, 0), 3, 17 },
                    { 54, "Prepare lamb shank", new TimeSpan(0, 0, 15, 0, 0), 1, 18 },
                    { 55, "Slow cook for 4 hours", new TimeSpan(0, 4, 0, 0, 0), 2, 18 },
                    { 56, "Serve with mashed potatoes", null, 3, 18 },
                    { 57, "Prepare roux", new TimeSpan(0, 0, 15, 0, 0), 1, 19 },
                    { 58, "Add seafood and sausage", null, 2, 19 },
                    { 59, "Simmer and serve over rice", new TimeSpan(0, 0, 20, 0, 0), 3, 19 },
                    { 60, "Prepare lobster", new TimeSpan(0, 0, 15, 0, 0), 1, 20 },
                    { 61, "Cook thermidor sauce", new TimeSpan(0, 0, 10, 0, 0), 2, 20 },
                    { 62, "Bake lobster with sauce", new TimeSpan(0, 0, 20, 0, 0), 3, 20 },
                    { 63, "Prepare blini batter", new TimeSpan(0, 0, 10, 0, 0), 1, 21 },
                    { 64, "Cook blinis", new TimeSpan(0, 0, 5, 0, 0), 2, 21 },
                    { 65, "Top with caviar", null, 3, 21 },
                    { 66, "Prepare wagyu beef", new TimeSpan(0, 0, 10, 0, 0), 1, 22 },
                    { 67, "Roll sushi with wagyu and rice", null, 2, 22 },
                    { 68, "Serve with soy sauce", null, 3, 22 },
                    { 69, "Prepare mushroom soup base", new TimeSpan(0, 0, 15, 0, 0), 1, 23 },
                    { 70, "Add truffle oil and cream", null, 2, 23 },
                    { 71, "Simmer and serve", new TimeSpan(0, 0, 10, 0, 0), 3, 23 },
                    { 72, "Prepare duck", new TimeSpan(0, 0, 20, 0, 0), 1, 24 },
                    { 73, "Cook orange sauce", null, 2, 24 },
                    { 74, "Serve duck with sauce", new TimeSpan(0, 0, 15, 0, 0), 3, 24 },
                    { 75, "Prepare oysters", new TimeSpan(0, 0, 10, 0, 0), 1, 25 },
                    { 76, "Top with Rockefeller sauce", null, 2, 25 },
                    { 77, "Bake oysters", new TimeSpan(0, 0, 8, 0, 0), 3, 25 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Instructions",
                keyColumn: "InstructionId",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "Instructions",
                keyColumn: "InstructionId",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "Instructions",
                keyColumn: "InstructionId",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "Instructions",
                keyColumn: "InstructionId",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "Instructions",
                keyColumn: "InstructionId",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "Instructions",
                keyColumn: "InstructionId",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "Instructions",
                keyColumn: "InstructionId",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "Instructions",
                keyColumn: "InstructionId",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "Instructions",
                keyColumn: "InstructionId",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "Instructions",
                keyColumn: "InstructionId",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "Instructions",
                keyColumn: "InstructionId",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "Instructions",
                keyColumn: "InstructionId",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "Instructions",
                keyColumn: "InstructionId",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "Instructions",
                keyColumn: "InstructionId",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "Instructions",
                keyColumn: "InstructionId",
                keyValue: 41);

            migrationBuilder.DeleteData(
                table: "Instructions",
                keyColumn: "InstructionId",
                keyValue: 42);

            migrationBuilder.DeleteData(
                table: "Instructions",
                keyColumn: "InstructionId",
                keyValue: 43);

            migrationBuilder.DeleteData(
                table: "Instructions",
                keyColumn: "InstructionId",
                keyValue: 44);

            migrationBuilder.DeleteData(
                table: "Instructions",
                keyColumn: "InstructionId",
                keyValue: 45);

            migrationBuilder.DeleteData(
                table: "Instructions",
                keyColumn: "InstructionId",
                keyValue: 46);

            migrationBuilder.DeleteData(
                table: "Instructions",
                keyColumn: "InstructionId",
                keyValue: 47);

            migrationBuilder.DeleteData(
                table: "Instructions",
                keyColumn: "InstructionId",
                keyValue: 48);

            migrationBuilder.DeleteData(
                table: "Instructions",
                keyColumn: "InstructionId",
                keyValue: 49);

            migrationBuilder.DeleteData(
                table: "Instructions",
                keyColumn: "InstructionId",
                keyValue: 50);

            migrationBuilder.DeleteData(
                table: "Instructions",
                keyColumn: "InstructionId",
                keyValue: 51);

            migrationBuilder.DeleteData(
                table: "Instructions",
                keyColumn: "InstructionId",
                keyValue: 52);

            migrationBuilder.DeleteData(
                table: "Instructions",
                keyColumn: "InstructionId",
                keyValue: 53);

            migrationBuilder.DeleteData(
                table: "Instructions",
                keyColumn: "InstructionId",
                keyValue: 54);

            migrationBuilder.DeleteData(
                table: "Instructions",
                keyColumn: "InstructionId",
                keyValue: 55);

            migrationBuilder.DeleteData(
                table: "Instructions",
                keyColumn: "InstructionId",
                keyValue: 56);

            migrationBuilder.DeleteData(
                table: "Instructions",
                keyColumn: "InstructionId",
                keyValue: 57);

            migrationBuilder.DeleteData(
                table: "Instructions",
                keyColumn: "InstructionId",
                keyValue: 58);

            migrationBuilder.DeleteData(
                table: "Instructions",
                keyColumn: "InstructionId",
                keyValue: 59);

            migrationBuilder.DeleteData(
                table: "Instructions",
                keyColumn: "InstructionId",
                keyValue: 60);

            migrationBuilder.DeleteData(
                table: "Instructions",
                keyColumn: "InstructionId",
                keyValue: 61);

            migrationBuilder.DeleteData(
                table: "Instructions",
                keyColumn: "InstructionId",
                keyValue: 62);

            migrationBuilder.DeleteData(
                table: "Instructions",
                keyColumn: "InstructionId",
                keyValue: 63);

            migrationBuilder.DeleteData(
                table: "Instructions",
                keyColumn: "InstructionId",
                keyValue: 64);

            migrationBuilder.DeleteData(
                table: "Instructions",
                keyColumn: "InstructionId",
                keyValue: 65);

            migrationBuilder.DeleteData(
                table: "Instructions",
                keyColumn: "InstructionId",
                keyValue: 66);

            migrationBuilder.DeleteData(
                table: "Instructions",
                keyColumn: "InstructionId",
                keyValue: 67);

            migrationBuilder.DeleteData(
                table: "Instructions",
                keyColumn: "InstructionId",
                keyValue: 68);

            migrationBuilder.DeleteData(
                table: "Instructions",
                keyColumn: "InstructionId",
                keyValue: 69);

            migrationBuilder.DeleteData(
                table: "Instructions",
                keyColumn: "InstructionId",
                keyValue: 70);

            migrationBuilder.DeleteData(
                table: "Instructions",
                keyColumn: "InstructionId",
                keyValue: 71);

            migrationBuilder.DeleteData(
                table: "Instructions",
                keyColumn: "InstructionId",
                keyValue: 72);

            migrationBuilder.DeleteData(
                table: "Instructions",
                keyColumn: "InstructionId",
                keyValue: 73);

            migrationBuilder.DeleteData(
                table: "Instructions",
                keyColumn: "InstructionId",
                keyValue: 74);

            migrationBuilder.DeleteData(
                table: "Instructions",
                keyColumn: "InstructionId",
                keyValue: 75);

            migrationBuilder.DeleteData(
                table: "Instructions",
                keyColumn: "InstructionId",
                keyValue: 76);

            migrationBuilder.DeleteData(
                table: "Instructions",
                keyColumn: "InstructionId",
                keyValue: 77);

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
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Recipes",
                keyColumn: "RecipeId",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Recipes",
                keyColumn: "RecipeId",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Recipes",
                keyColumn: "RecipeId",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Recipes",
                keyColumn: "RecipeId",
                keyValue: 19);

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

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "RecipeType",
                table: "Recipes");
        }
    }
}
