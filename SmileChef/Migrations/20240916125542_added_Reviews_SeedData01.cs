using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SmileChef.Migrations
{
    /// <inheritdoc />
    public partial class added_Reviews_SeedData01 : Migration
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

            migrationBuilder.InsertData(
                table: "Recipes",
                columns: new[] { "RecipeId", "ChefId", "ImageUrl", "Name", "Price", "RecipeType" },
                values: new object[,]
                {
                    { 1, 1, "recipe1.jpeg", "Turmeric Rice Chicken", null, "Premium" },
                    { 7, 1, "recipe7.jpeg", "Aubergine Curry Vegetables", null, "Premium" },
                    { 19, 6, "recipe13.jpeg", "Crispy Shredded Beef", null, "Premium" }
                });

            migrationBuilder.InsertData(
                table: "Reviews",
                columns: new[] { "Id", "Message", "RecipeId", "ReviewDate", "ReviewerId" },
                values: new object[,]
                {
                    { 12, "I encountered bugs when trying to pay for a recipe. Really inconvenient!", 22, new DateTime(2024, 9, 16, 12, 27, 28, 0, DateTimeKind.Unspecified), 1 },
                    { 13, "The notification system is helpful. I get alerts whenever a new recipe is published by my favorite chef.", 22, new DateTime(2024, 9, 16, 12, 27, 28, 0, DateTimeKind.Unspecified), 1 },
                    { 14, "The recipe categorization could be better. It’s hard to find specific types of dishes.", 22, new DateTime(2024, 9, 16, 12, 27, 28, 0, DateTimeKind.Unspecified), 1 },
                    { 15, "The balance management feature for chefs is really neat. It's easy to track earnings.", 22, new DateTime(2024, 9, 16, 12, 27, 28, 0, DateTimeKind.Unspecified), 1 },
                    { 16, "The app’s design is sleek and modern. It makes using it enjoyable.", 22, new DateTime(2024, 9, 16, 12, 27, 28, 0, DateTimeKind.Unspecified), 1 },
                    { 17, "The service was excellent, and the recipe suggestions were spot on. My family loved the meals!", 10, new DateTime(2024, 9, 16, 12, 27, 28, 0, DateTimeKind.Unspecified), 1 },
                    { 18, "The chef locator feature is incredibly useful. Found a great chef nearby within minutes!", 10, new DateTime(2024, 9, 16, 12, 27, 28, 0, DateTimeKind.Unspecified), 1 },
                    { 19, "The customer service is slow. I sent a query two days ago and still haven't heard back.", 10, new DateTime(2024, 9, 16, 12, 27, 28, 0, DateTimeKind.Unspecified), 1 },
                    { 20, "The variety of recipes available is amazing. I’ve learned so many new dishes!", 10, new DateTime(2024, 9, 16, 12, 27, 28, 0, DateTimeKind.Unspecified), 1 },
                    { 21, "The image recognition feature is fantastic. It nailed the recipe suggestion from just a picture.", 10, new DateTime(2024, 9, 16, 12, 27, 28, 0, DateTimeKind.Unspecified), 1 },
                    { 22, "I found the app super intuitive and easy to use. Great for both beginners and experienced cooks.", 10, new DateTime(2024, 9, 16, 12, 27, 28, 0, DateTimeKind.Unspecified), 1 },
                    { 23, "The subscription system works smoothly, and I get great value from my chef's content.", 10, new DateTime(2024, 9, 16, 12, 27, 28, 0, DateTimeKind.Unspecified), 1 },
                    { 24, "The notification system is helpful. I get alerts whenever a new recipe is published by my favorite chef.", 21, new DateTime(2024, 9, 16, 12, 27, 28, 0, DateTimeKind.Unspecified), 1 },
                    { 25, "The app’s design is sleek and modern. It makes using it enjoyable.", 21, new DateTime(2024, 9, 16, 12, 27, 28, 0, DateTimeKind.Unspecified), 1 },
                    { 26, "The app’s design is sleek and modern. It makes using it enjoyable.", 21, new DateTime(2024, 9, 16, 12, 27, 28, 0, DateTimeKind.Unspecified), 1 },
                    { 27, "The chicken curry was delicious, with just the right amount of spice. Loved it!", 23, new DateTime(2024, 9, 16, 12, 27, 28, 0, DateTimeKind.Unspecified), 1 },
                    { 28, "The salad was bland, with wilted greens and barely any dressing.", 23, new DateTime(2024, 9, 16, 12, 27, 28, 0, DateTimeKind.Unspecified), 1 },
                    { 1, "Tasty sushi ever posted", 1, new DateTime(2024, 9, 16, 12, 27, 28, 0, DateTimeKind.Unspecified), 1 },
                    { 2, "This beef sushi is yummy and tender", 1, new DateTime(2024, 9, 16, 12, 27, 28, 0, DateTimeKind.Unspecified), 1 },
                    { 3, "This chicken and rice goes so well. I enjoyed cooking and eating it", 1, new DateTime(2024, 9, 16, 12, 27, 28, 0, DateTimeKind.Unspecified), 1 },
                    { 4, "Indian foods are indeed very delicious. They are full of spice and adventure.", 1, new DateTime(2024, 9, 16, 12, 27, 28, 0, DateTimeKind.Unspecified), 1 },
                    { 5, "I would surely cook this recipe and share it in my catering events", 1, new DateTime(2024, 9, 16, 12, 27, 28, 0, DateTimeKind.Unspecified), 1 },
                    { 6, "Some recipes are way too expensive for what they offer. Not worth the price.", 1, new DateTime(2024, 9, 16, 12, 27, 28, 0, DateTimeKind.Unspecified), 1 },
                    { 7, "The chef locator feature is incredibly useful. Found a great chef nearby within minutes!", 1, new DateTime(2024, 9, 16, 12, 27, 28, 0, DateTimeKind.Unspecified), 1 },
                    { 8, "The app crashed multiple times while I was browsing recipes. Very frustrating!", 1, new DateTime(2024, 9, 16, 12, 27, 28, 0, DateTimeKind.Unspecified), 1 },
                    { 9, "The service was excellent, and the recipe suggestions were spot on. My family loved the meals!", 1, new DateTime(2024, 9, 16, 12, 27, 28, 0, DateTimeKind.Unspecified), 1 },
                    { 10, "The variety of recipes available is amazing. I’ve learned so many new dishes!", 1, new DateTime(2024, 9, 16, 12, 27, 28, 0, DateTimeKind.Unspecified), 1 },
                    { 11, "The image recognition feature is fantastic. It nailed the recipe suggestion from just a picture.", 1, new DateTime(2024, 9, 16, 12, 27, 28, 0, DateTimeKind.Unspecified), 1 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Recipes",
                keyColumn: "RecipeId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Recipes",
                keyColumn: "RecipeId",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "Recipes",
                keyColumn: "RecipeId",
                keyValue: 1);

            migrationBuilder.InsertData(
                table: "Recipes",
                columns: new[] { "RecipeId", "ChefId", "ImageUrl", "Name", "Price", "RecipeType" },
                values: new object[,]
                {
                    { 1, 1, "recipe1.jpeg", "Turmeric Rice Chicken", null, "Basic" },
                    { 7, 1, "recipe7.jpeg", "Aubergine Curry Vegetables", null, "Basic" },
                    { 19, 6, "recipe13.jpeg", "Crispy Shredded Beef", null, "Basic" }
                });
        }
    }
}
