using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SmileChef.Migrations
{
    /// <inheritdoc />
    public partial class Initial_Create : Migration
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
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Rating = table.Column<int>(type: "int", maxLength: 5, nullable: true),
                    SubscriptionCost = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    AccountBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
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
                name: "Orders",
                columns: table => new
                {
                    OrderId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ChefId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.OrderId);
                    table.ForeignKey(
                        name: "FK_Orders_Chefs_ChefId",
                        column: x => x.ChefId,
                        principalTable: "Chefs",
                        principalColumn: "ChefId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Recipes",
                columns: table => new
                {
                    RecipeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ChefId = table.Column<int>(type: "int", nullable: false),
                    RecipeType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<float>(type: "real", nullable: true)
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

            migrationBuilder.CreateTable(
                name: "NotifySubscribers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PublisherId = table.Column<int>(type: "int", nullable: false),
                    RecipeId = table.Column<int>(type: "int", nullable: false),
                    SubscriberId = table.Column<int>(type: "int", nullable: false),
                    Notified = table.Column<bool>(type: "bit", nullable: false),
                    PublishedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotifySubscribers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NotifySubscribers_Chefs_PublisherId",
                        column: x => x.PublisherId,
                        principalTable: "Chefs",
                        principalColumn: "ChefId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NotifySubscribers_Chefs_SubscriberId",
                        column: x => x.SubscriberId,
                        principalTable: "Chefs",
                        principalColumn: "ChefId",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_NotifySubscribers_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipes",
                        principalColumn: "RecipeId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "OrderLines",
                columns: table => new
                {
                    OrderLineId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    RecipeId = table.Column<int>(type: "int", nullable: false),
                    RecipeName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderLines", x => x.OrderLineId);
                    table.ForeignKey(
                        name: "FK_OrderLines_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderLines_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipes",
                        principalColumn: "RecipeId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "RecipeBookmarks",
                columns: table => new
                {
                    RecipeBookmarkId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecipeId = table.Column<int>(type: "int", nullable: false),
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

            migrationBuilder.CreateTable(
                name: "Reviews",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReviewerId = table.Column<int>(type: "int", nullable: false),
                    RecipeId = table.Column<int>(type: "int", nullable: false),
                    ReviewDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reviews_Chefs_ReviewerId",
                        column: x => x.ReviewerId,
                        principalTable: "Chefs",
                        principalColumn: "ChefId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reviews_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipes",
                        principalColumn: "RecipeId",
                        onDelete: ReferentialAction.NoAction);
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
                columns: new[] { "ChefId", "AccountBalance", "FirstName", "LastName", "Rating", "SubscriptionCost", "UserId" },
                values: new object[,]
                {
                    { 1, 10000m, "Gordon", "Ramsay", 3, 12.99m, 1 },
                    { 2, 5500m, "Jamie", "Oliver", 1, 11.99m, 2 },
                    { 3, 9000m, "Wolfgang", "Puck", 5, 5.99m, 3 },
                    { 4, 15000m, "Alice", "Waters", 4, 6.99m, 4 },
                    { 5, 8000m, "Thomas", "Keller", 2, 15.99m, 5 },
                    { 6, 7000m, "Emeril", "Lagasse", 5, 10.99m, 6 }
                });

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
                table: "Subscriptions",
                columns: new[] { "SubscriptionId", "Amount", "PublisherId", "SubscriberId", "SubscriptionDate", "SubscriptionType" },
                values: new object[,]
                {
                    { 1, 99.99m, 1, 3, new DateTime(2024, 5, 17, 0, 0, 0, 0, DateTimeKind.Local), "Monthly" },
                    { 2, 99.99m, 1, 4, new DateTime(2024, 5, 17, 0, 0, 0, 0, DateTimeKind.Local), "Monthly" },
                    { 3, 99.99m, 1, 5, new DateTime(2024, 5, 17, 0, 0, 0, 0, DateTimeKind.Local), "Monthly" },
                    { 4, 199.99m, 2, 4, new DateTime(2024, 5, 17, 0, 0, 0, 0, DateTimeKind.Local), "Yearly" },
                    { 5, 199.99m, 2, 5, new DateTime(2024, 5, 17, 0, 0, 0, 0, DateTimeKind.Local), "Yearly" },
                    { 6, 199.99m, 2, 6, new DateTime(2024, 5, 17, 0, 0, 0, 0, DateTimeKind.Local), "Yearly" },
                    { 7, 50.00m, 2, 1, new DateTime(2024, 5, 17, 0, 0, 0, 0, DateTimeKind.Local), "Monthly" },
                    { 8, 75.00m, 3, 1, new DateTime(2024, 5, 17, 0, 0, 0, 0, DateTimeKind.Local), "Monthly" },
                    { 9, 60.00m, 4, 1, new DateTime(2024, 5, 17, 0, 0, 0, 0, DateTimeKind.Local), "Monthly" },
                    { 10, 80.00m, 5, 1, new DateTime(2024, 5, 17, 0, 0, 0, 0, DateTimeKind.Local), "Monthly" }
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
                    { 26, "Plate the soup with sprinkled corriander and chillies", null, 4, 8 },
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

            migrationBuilder.CreateIndex(
                name: "IX_Chefs_UserId",
                table: "Chefs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Instructions_RecipeId",
                table: "Instructions",
                column: "RecipeId");

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

            migrationBuilder.CreateIndex(
                name: "IX_OrderLines_OrderId",
                table: "OrderLines",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderLines_RecipeId",
                table: "OrderLines",
                column: "RecipeId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ChefId",
                table: "Orders",
                column: "ChefId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeBookmarks_ChefId",
                table: "RecipeBookmarks",
                column: "ChefId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeBookmarks_RecipeId",
                table: "RecipeBookmarks",
                column: "RecipeId");

            migrationBuilder.CreateIndex(
                name: "IX_Recipes_ChefId",
                table: "Recipes",
                column: "ChefId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_RecipeId",
                table: "Reviews",
                column: "RecipeId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_ReviewerId",
                table: "Reviews",
                column: "ReviewerId");

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
                name: "NotifySubscribers");

            migrationBuilder.DropTable(
                name: "OrderLines");

            migrationBuilder.DropTable(
                name: "RecipeBookmarks");

            migrationBuilder.DropTable(
                name: "Reviews");

            migrationBuilder.DropTable(
                name: "Subscriptions");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Recipes");

            migrationBuilder.DropTable(
                name: "Chefs");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
