﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SmileChef.Migrations
{
    /// <inheritdoc />
    public partial class Fixed_SeedData_Values_ForNew_PremiumRecipes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
          
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
                    { 1, 1, "recipe1.jpeg", "Turmeric Rice Chicken", null, "Basic" },
                    { 7, 1, "recipe7.jpeg", "Aubergine Curry Vegetables", null, "Basic" },
                    { 19, 6, "recipe13.jpeg", "Crispy Shredded Beef", null, "Basic" }
                });
        }
    }
}
