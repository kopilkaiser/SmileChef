using ChefApp.Models.DbModels;
using Microsoft.EntityFrameworkCore;
using SmileChef.Models.DbModels;
using SmileChef.Models.Enums;

namespace ChefApp.Extensions
{
    public static class ModelBuilderExtensions
    {
        public static void SeedAppData(this ModelBuilder modelBuilder)
        {
            // Seed User
            modelBuilder.Entity<User>().HasData(
                new User { UserId = 1, Email = "gordan@gmail.com", Password = "123" },
                new User { UserId = 2, Email = "jamie@gmail.com", Password = "123" },
                new User { UserId = 3, Email = "wolfgang@gmail.com", Password = "123" },
                new User { UserId = 4, Email = "alice@gmail.com", Password = "123" },
                new User { UserId = 5, Email = "thomas@gmail.com", Password = "123" },
                new User { UserId = 6, Email = "emeril@gmail.com", Password = "123" }
            );

            // Seed Chefs
            modelBuilder.Entity<Chef>().HasData(
                new Chef { ChefId = 1, FirstName = "Gordon", LastName = "Ramsay", UserId = 1, Rating = 3, SubscriptionCost = 12.99m, AccountBalance = 10000m },
                new Chef { ChefId = 2, FirstName = "Jamie", LastName = "Oliver", UserId = 2, Rating = 1, SubscriptionCost = 11.99m, AccountBalance = 5500m },
                new Chef { ChefId = 3, FirstName = "Wolfgang", LastName = "Puck", UserId = 3, Rating = 5, SubscriptionCost = 5.99m, AccountBalance = 9000m },
                new Chef { ChefId = 4, FirstName = "Alice", LastName = "Waters", UserId = 4, Rating = 4, SubscriptionCost = 6.99m, AccountBalance = 15000m },
                new Chef { ChefId = 5, FirstName = "Thomas", LastName = "Keller", UserId = 5, Rating = 2, SubscriptionCost = 15.99m, AccountBalance = 8000m },
                new Chef { ChefId = 6, FirstName = "Emeril", LastName = "Lagasse", UserId = 6, Rating = 5, SubscriptionCost = 10.99m, AccountBalance = 7000m }
            );

            // Seed Basic Recipes
            modelBuilder.Entity<Recipe>().HasData(
                new Recipe { RecipeId = 1, Name = "Beef Wellington", ChefId = 1, RecipeType = RecipeType.Basic },
                new Recipe { RecipeId = 2, Name = "Pasta Carbonara", ChefId = 2, RecipeType = RecipeType.Basic },
                new Recipe { RecipeId = 3, Name = "Spicy Tuna Rolls", ChefId = 3, RecipeType = RecipeType.Basic },
                new Recipe { RecipeId = 4, Name = "Lentil Soup", ChefId = 4, RecipeType = RecipeType.Basic },
                new Recipe { RecipeId = 5, Name = "Roast Chicken", ChefId = 5, RecipeType = RecipeType.Basic },
                new Recipe { RecipeId = 6, Name = "Bananas Foster", ChefId = 6, RecipeType = RecipeType.Basic },
                new Recipe { RecipeId = 7, Name = "Beef Bolognese", ChefId = 1, RecipeType = RecipeType.Basic },
                new Recipe { RecipeId = 8, Name = "Chicken Mushroom Soup", ChefId = 1, RecipeType = RecipeType.Basic },
                new Recipe { RecipeId = 15, Name = "Classic Margherita Pizza", ChefId = 2, RecipeType = RecipeType.Basic }, // New basic recipe
                new Recipe { RecipeId = 16, Name = "Seafood Paella", ChefId = 3, RecipeType = RecipeType.Basic }, // New basic recipe
                new Recipe { RecipeId = 17, Name = "Vegetarian Stir Fry", ChefId = 4, RecipeType = RecipeType.Basic }, // New basic recipe
                new Recipe { RecipeId = 18, Name = "Slow Cooked Lamb Shank", ChefId = 5, RecipeType = RecipeType.Basic }, // New basic recipe
                new Recipe { RecipeId = 19, Name = "Creole Gumbo", ChefId = 6, RecipeType = RecipeType.Basic } // New basic recipe
            );

            // Seed Premium Recipes
            modelBuilder.Entity<PremiumRecipe>().HasData(
                new PremiumRecipe { RecipeId = 9, Name = "Gourmet Beef Wellington", ChefId = 1, Price = 49.99f, RecipeType = RecipeType.Premium },
                new PremiumRecipe { RecipeId = 10, Name = "Truffle Risotto", ChefId = 2, Price = 39.99f, RecipeType = RecipeType.Premium },
                new PremiumRecipe { RecipeId = 11, Name = "Sushi Deluxe", ChefId = 3, Price = 29.99f, RecipeType = RecipeType.Premium },
                new PremiumRecipe { RecipeId = 12, Name = "Organic Farm-to-Table Salad", ChefId = 4, Price = 19.99f, RecipeType = RecipeType.Premium },
                new PremiumRecipe { RecipeId = 13, Name = "Foie Gras Terrine", ChefId = 5, Price = 59.99f, RecipeType = RecipeType.Premium },
                new PremiumRecipe { RecipeId = 14, Name = "Emeril's Essence Special", ChefId = 6, Price = 24.99f, RecipeType = RecipeType.Premium },
                new PremiumRecipe { RecipeId = 20, Name = "Lobster Thermidor", ChefId = 1, Price = 69.99f, RecipeType = RecipeType.Premium }, // New premium recipe
                new PremiumRecipe { RecipeId = 21, Name = "Caviar Blinis", ChefId = 2, Price = 99.99f, RecipeType = RecipeType.Premium }, // New premium recipe
                new PremiumRecipe { RecipeId = 22, Name = "Wagyu Beef Sushi", ChefId = 3, Price = 89.99f, RecipeType = RecipeType.Premium }, // New premium recipe
                new PremiumRecipe { RecipeId = 23, Name = "Truffle Mushroom Soup", ChefId = 4, Price = 34.99f, RecipeType = RecipeType.Premium }, // New premium recipe
                new PremiumRecipe { RecipeId = 24, Name = "Duck à l'Orange", ChefId = 5, Price = 54.99f, RecipeType = RecipeType.Premium }, // New premium recipe
                new PremiumRecipe { RecipeId = 25, Name = "Oysters Rockefeller", ChefId = 6, Price = 44.99f, RecipeType = RecipeType.Premium } // New premium recipe
            );

            // Seed Instructions with optional Time field
            modelBuilder.Entity<Instruction>().HasData(
                new Instruction { InstructionId = 1, Description = "Prepare beef", RecipeId = 1, OrderId = 1, Duration = TimeSpan.FromMinutes(10) },
                new Instruction { InstructionId = 2, Description = "Wrap in puff pastry", RecipeId = 1, OrderId = 2 },
                new Instruction { InstructionId = 3, Description = "Bake", RecipeId = 1, OrderId = 3, Duration = TimeSpan.FromMinutes(20) },
                new Instruction { InstructionId = 4, Description = "Cook pasta", RecipeId = 2, OrderId = 1, Duration = TimeSpan.FromMinutes(15) },
                new Instruction { InstructionId = 5, Description = "Prepare sauce", RecipeId = 2, OrderId = 2 },
                new Instruction { InstructionId = 6, Description = "Combine and serve", RecipeId = 2, OrderId = 3, Duration = TimeSpan.FromMinutes(3) },
                new Instruction { InstructionId = 7, Description = "Prepare tuna", RecipeId = 3, OrderId = 1 },
                new Instruction { InstructionId = 8, Description = "Roll sushi", RecipeId = 3, OrderId = 2, Duration = TimeSpan.FromMinutes(5) },
                new Instruction { InstructionId = 9, Description = "Serve with soy sauce", RecipeId = 3, OrderId = 3 },
                new Instruction { InstructionId = 10, Description = "Prepare lentils", RecipeId = 4, OrderId = 1 },
                new Instruction { InstructionId = 11, Description = "Cook lentils", RecipeId = 4, OrderId = 2, Duration = TimeSpan.FromMinutes(15) },
                new Instruction { InstructionId = 12, Description = "Serve hot", RecipeId = 4, OrderId = 3 },
                new Instruction { InstructionId = 13, Description = "Season chicken", RecipeId = 5, OrderId = 1 },
                new Instruction { InstructionId = 14, Description = "Roast chicken", RecipeId = 5, OrderId = 2, Duration = TimeSpan.FromMinutes(45) },
                new Instruction { InstructionId = 15, Description = "Serve with vegetables", RecipeId = 5, OrderId = 3 },
                new Instruction { InstructionId = 16, Description = "Prepare bananas", RecipeId = 6, OrderId = 1, Duration = TimeSpan.FromMinutes(15) },
                new Instruction { InstructionId = 17, Description = "Cook with butter and sugar", RecipeId = 6, OrderId = 2 },
                new Instruction { InstructionId = 18, Description = "Serve with ice cream", RecipeId = 6, OrderId = 3 },
                new Instruction { InstructionId = 19, Description = "Cook Mince Beef", RecipeId = 7, OrderId = 1, Duration = TimeSpan.FromMinutes(10) },
                new Instruction { InstructionId = 20, Description = "Cook Tomato Sauce", RecipeId = 7, OrderId = 2, Duration = TimeSpan.FromMinutes(25) },
                new Instruction { InstructionId = 21, Description = "Boil Sphagetti", RecipeId = 7, OrderId = 3, Duration = TimeSpan.FromMinutes(15) },
                new Instruction { InstructionId = 22, Description = "Mix Cooked Beef, Suace, and Sphagetti", RecipeId = 7, OrderId = 4 },
                new Instruction { InstructionId = 23, Description = "Cook Chicken Soup", RecipeId = 8, OrderId = 1, Duration = TimeSpan.FromMinutes(35) },
                new Instruction { InstructionId = 24, Description = "Add chopped Mushroom, Cillantro, Corriander", RecipeId = 8, OrderId = 2, Duration = TimeSpan.FromMinutes(2) },
                new Instruction { InstructionId = 25, Description = "Simmer the ingredients", RecipeId = 8, OrderId = 3, Duration = TimeSpan.FromMinutes(5) },
                new Instruction { InstructionId = 26, Description = "Plate the soup with sprinkled corriander and chillies", RecipeId = 8, OrderId = 4 },
                new Instruction { InstructionId = 27, Description = "Prepare gourmet beef", RecipeId = 9, OrderId = 1, Duration = TimeSpan.FromMinutes(15) },
                new Instruction { InstructionId = 28, Description = "Wrap with prosciutto and puff pastry", RecipeId = 9, OrderId = 2 },
                new Instruction { InstructionId = 29, Description = "Bake to perfection", RecipeId = 9, OrderId = 3, Duration = TimeSpan.FromMinutes(25) },
                new Instruction { InstructionId = 30, Description = "Prepare truffle stock", RecipeId = 10, OrderId = 1, Duration = TimeSpan.FromMinutes(10) },
                new Instruction { InstructionId = 31, Description = "Cook Arborio rice", RecipeId = 10, OrderId = 2, Duration = TimeSpan.FromMinutes(18) },
                new Instruction { InstructionId = 32, Description = "Add truffle and parmesan", RecipeId = 10, OrderId = 3 },
                new Instruction { InstructionId = 33, Description = "Prepare sushi-grade tuna", RecipeId = 11, OrderId = 1 },
                new Instruction { InstructionId = 34, Description = "Roll with sushi rice and nori", RecipeId = 11, OrderId = 2, Duration = TimeSpan.FromMinutes(8) },
                new Instruction { InstructionId = 35, Description = "Garnish with caviar", RecipeId = 11, OrderId = 3 },
                new Instruction { InstructionId = 36, Description = "Chop organic vegetables", RecipeId = 12, OrderId = 1, Duration = TimeSpan.FromMinutes(12) },
                new Instruction { InstructionId = 37, Description = "Prepare vinaigrette dressing", RecipeId = 12, OrderId = 2 },
                new Instruction { InstructionId = 38, Description = "Toss and serve fresh", RecipeId = 12, OrderId = 3 },
                new Instruction { InstructionId = 39, Description = "Prepare foie gras", RecipeId = 13, OrderId = 1, Duration = TimeSpan.FromMinutes(15) },
                new Instruction { InstructionId = 40, Description = "Cook terrine with truffles", RecipeId = 13, OrderId = 2, Duration = TimeSpan.FromMinutes(30) },
                new Instruction { InstructionId = 41, Description = "Serve with toasted brioche", RecipeId = 13, OrderId = 3 },
                new Instruction { InstructionId = 42, Description = "Mix Emeril's special spices", RecipeId = 14, OrderId = 1 },
                new Instruction { InstructionId = 43, Description = "Marinate the meat", RecipeId = 14, OrderId = 2, Duration = TimeSpan.FromMinutes(20) },
                new Instruction { InstructionId = 44, Description = "Cook to desired doneness", RecipeId = 14, OrderId = 3 },
                // Instructions for new basic recipes
                new Instruction { InstructionId = 45, Description = "Prepare pizza dough", RecipeId = 15, OrderId = 1, Duration = TimeSpan.FromMinutes(15) },
                new Instruction { InstructionId = 46, Description = "Add tomato sauce and mozzarella", RecipeId = 15, OrderId = 2 },
                new Instruction { InstructionId = 47, Description = "Bake in oven", RecipeId = 15, OrderId = 3, Duration = TimeSpan.FromMinutes(10) },
                new Instruction { InstructionId = 48, Description = "Cook seafood", RecipeId = 16, OrderId = 1, Duration = TimeSpan.FromMinutes(15) },
                new Instruction { InstructionId = 49, Description = "Prepare paella rice", RecipeId = 16, OrderId = 2 },
                new Instruction { InstructionId = 50, Description = "Combine seafood with rice", RecipeId = 16, OrderId = 3, Duration = TimeSpan.FromMinutes(20) },
                new Instruction { InstructionId = 51, Description = "Chop vegetables", RecipeId = 17, OrderId = 1, Duration = TimeSpan.FromMinutes(10) },
                new Instruction { InstructionId = 52, Description = "Stir fry in wok", RecipeId = 17, OrderId = 2 },
                new Instruction { InstructionId = 53, Description = "Serve with soy sauce", RecipeId = 17, OrderId = 3, Duration = TimeSpan.FromMinutes(5) },
                new Instruction { InstructionId = 54, Description = "Prepare lamb shank", RecipeId = 18, OrderId = 1, Duration = TimeSpan.FromMinutes(15) },
                new Instruction { InstructionId = 55, Description = "Slow cook for 4 hours", RecipeId = 18, OrderId = 2, Duration = TimeSpan.FromHours(4) },
                new Instruction { InstructionId = 56, Description = "Serve with mashed potatoes", RecipeId = 18, OrderId = 3 },
                new Instruction { InstructionId = 57, Description = "Prepare roux", RecipeId = 19, OrderId = 1, Duration = TimeSpan.FromMinutes(15) },
                new Instruction { InstructionId = 58, Description = "Add seafood and sausage", RecipeId = 19, OrderId = 2 },
                new Instruction { InstructionId = 59, Description = "Simmer and serve over rice", RecipeId = 19, OrderId = 3, Duration = TimeSpan.FromMinutes(20) },
                // Instructions for new premium recipes
                new Instruction { InstructionId = 60, Description = "Prepare lobster", RecipeId = 20, OrderId = 1, Duration = TimeSpan.FromMinutes(15) },
                new Instruction { InstructionId = 61, Description = "Cook thermidor sauce", RecipeId = 20, OrderId = 2, Duration = TimeSpan.FromMinutes(10) },
                new Instruction { InstructionId = 62, Description = "Bake lobster with sauce", RecipeId = 20, OrderId = 3, Duration = TimeSpan.FromMinutes(20) },
                new Instruction { InstructionId = 63, Description = "Prepare blini batter", RecipeId = 21, OrderId = 1, Duration = TimeSpan.FromMinutes(10) },
                new Instruction { InstructionId = 64, Description = "Cook blinis", RecipeId = 21, OrderId = 2, Duration = TimeSpan.FromMinutes(5) },
                new Instruction { InstructionId = 65, Description = "Top with caviar", RecipeId = 21, OrderId = 3 },
                new Instruction { InstructionId = 66, Description = "Prepare wagyu beef", RecipeId = 22, OrderId = 1, Duration = TimeSpan.FromMinutes(10) },
                new Instruction { InstructionId = 67, Description = "Roll sushi with wagyu and rice", RecipeId = 22, OrderId = 2 },
                new Instruction { InstructionId = 68, Description = "Serve with soy sauce", RecipeId = 22, OrderId = 3 },
                new Instruction { InstructionId = 69, Description = "Prepare mushroom soup base", RecipeId = 23, OrderId = 1, Duration = TimeSpan.FromMinutes(15) },
                new Instruction { InstructionId = 70, Description = "Add truffle oil and cream", RecipeId = 23, OrderId = 2 },
                new Instruction { InstructionId = 71, Description = "Simmer and serve", RecipeId = 23, OrderId = 3, Duration = TimeSpan.FromMinutes(10) },
                new Instruction { InstructionId = 72, Description = "Prepare duck", RecipeId = 24, OrderId = 1, Duration = TimeSpan.FromMinutes(20) },
                new Instruction { InstructionId = 73, Description = "Cook orange sauce", RecipeId = 24, OrderId = 2 },
                new Instruction { InstructionId = 74, Description = "Serve duck with sauce", RecipeId = 24, OrderId = 3, Duration = TimeSpan.FromMinutes(15) },
                new Instruction { InstructionId = 75, Description = "Prepare oysters", RecipeId = 25, OrderId = 1, Duration = TimeSpan.FromMinutes(10) },
                new Instruction { InstructionId = 76, Description = "Top with Rockefeller sauce", RecipeId = 25, OrderId = 2 },
                new Instruction { InstructionId = 77, Description = "Bake oysters", RecipeId = 25, OrderId = 3, Duration = TimeSpan.FromMinutes(8) }
            );

            // Fixed DateTime value
            DateTime fixedDateTime = new DateTime(2024, 5, 17, 0, 0, 0, DateTimeKind.Local);

            // Seed Subscriptions (3 other chefs are subscribers to 2 main chefs)
            modelBuilder.Entity<Subscription>().HasData(
                // Chef 3, 4, and 5 subscribe to Chef 1
                new Subscription
                {
                    SubscriptionId = 1,
                    SubscriberId = 3,
                    PublisherId = 1,
                    SubscriptionDate = fixedDateTime,
                    Amount = 99.99m,
                    SubscriptionType = SubscriptionType.Monthly
                },
                new Subscription
                {
                    SubscriptionId = 2,
                    SubscriberId = 4,
                    PublisherId = 1,
                    SubscriptionDate = fixedDateTime,
                    Amount = 99.99m,
                    SubscriptionType = SubscriptionType.Monthly
                },
                new Subscription
                {
                    SubscriptionId = 3,
                    SubscriberId = 5,
                    PublisherId = 1,
                    SubscriptionDate = fixedDateTime,
                    Amount = 99.99m,
                    SubscriptionType = SubscriptionType.Monthly
                },
                // Chef 4, 5, and 6 subscribe to Chef 2
                new Subscription
                {
                    SubscriptionId = 4,
                    SubscriberId = 4,
                    PublisherId = 2,
                    SubscriptionDate = fixedDateTime,
                    Amount = 199.99m,
                    SubscriptionType = SubscriptionType.Yearly
                },
                new Subscription
                {
                    SubscriptionId = 5,
                    SubscriberId = 5,
                    PublisherId = 2,
                    SubscriptionDate = fixedDateTime,
                    Amount = 199.99m,
                    SubscriptionType = SubscriptionType.Yearly
                },
                new Subscription
                {
                    SubscriptionId = 6,
                    SubscriberId = 6,
                    PublisherId = 2,
                    SubscriptionDate = fixedDateTime,
                    Amount = 199.99m,
                    SubscriptionType = SubscriptionType.Yearly
                },
                // Chef 1 subscribes to other chefs
                new Subscription
                {
                    SubscriptionId = 7,
                    SubscriberId = 1,
                    PublisherId = 2,
                    SubscriptionDate = fixedDateTime,
                    Amount = 50.00m,
                    SubscriptionType = SubscriptionType.Monthly
                },
                new Subscription
                {
                    SubscriptionId = 8,
                    SubscriberId = 1,
                    PublisherId = 3,
                    SubscriptionDate = fixedDateTime,
                    Amount = 75.00m,
                    SubscriptionType = SubscriptionType.Monthly
                },
                new Subscription
                {
                    SubscriptionId = 9,
                    SubscriberId = 1,
                    PublisherId = 4,
                    SubscriptionDate = fixedDateTime,
                    Amount = 60.00m,
                    SubscriptionType = SubscriptionType.Monthly
                },
                new Subscription
                {
                    SubscriptionId = 10,
                    SubscriberId = 1,
                    PublisherId = 5,
                    SubscriptionDate = fixedDateTime,
                    Amount = 80.00m,
                    SubscriptionType = SubscriptionType.Monthly
                }
            );

            // Seed Restaurants
            modelBuilder.Entity<Restaurant>().HasData(
                new Restaurant { RestaurantId = 1, Lat = 51.40785634278187, Lng = -0.29675218001524584, Location = "Located in Kingston Upon Thames", Title = "Gordon Restaurant", Phone = "+447745566123", OperatingTime = "09:00 - 17:00", ChefId = 1 },
                new Restaurant { RestaurantId = 2, Lat = 51.48230385097871, Lng = 0.16090573471658554, Location = "Located in Erith", Title = "Oliver Cake shop", Phone = "+447711223334", OperatingTime = "09:00 - 18:00", ChefId = 2 },
                new Restaurant { RestaurantId = 3, Lat = 51.50968065297982, Lng = -0.3060218904224092, Location = "Located in Ealing", Title = "Wolfgang Barbeque Zone", Phone = "+447733332222", OperatingTime = "09:00 - 15:30", ChefId = 3 },
                new Restaurant { RestaurantId = 4, Lat = 51.41133622965335, Lng = 0.014899074168950227, Location = "Located in Bromley", Title = "Alice Supermarket", Phone = "+447799991111", OperatingTime = "09:00 - 16:30", ChefId = 4 },
                new Restaurant { RestaurantId = 5, Lat = 51.61474228028355, Lng = -0.25151937991059076, Location = "Located in Edgeware", Title = "Thomas Yummy Restaurant", Phone = "+447723456789", OperatingTime = "09:00 - 18:30", ChefId = 5 },
                new Restaurant { RestaurantId = 6, Lat = 51.55244958656883, Lng = 0.07257729361227812, Location = "Located in Illford", Title = "Emeril Dirty Icecreams", Phone = "+447766662345", OperatingTime = "09:00 - 18:30", ChefId = 6 }
            );
        }
    }
}
