using ChefApp.Models.DbModels;
using Microsoft.EntityFrameworkCore;
using SmileChef.Models.DbModels;

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

            // Seed Recipes
            modelBuilder.Entity<Recipe>().HasData(
                new Recipe { RecipeId = 1, Name = "Beef Wellington", ChefId = 1 },
                new Recipe { RecipeId = 2, Name = "Pasta Carbonara", ChefId = 2 },
                new Recipe { RecipeId = 3, Name = "Spicy Tuna Rolls", ChefId = 3 },
                new Recipe { RecipeId = 4, Name = "Lentil Soup", ChefId = 4 },
                new Recipe { RecipeId = 5, Name = "Roast Chicken", ChefId = 5 },
                new Recipe { RecipeId = 6, Name = "Bananas Foster", ChefId = 6 },
                 new Recipe { RecipeId = 7, Name = "Beef Bolognese", ChefId = 1 },
                new Recipe { RecipeId = 8, Name = "Chicken Mushroom Soup", ChefId = 1 }
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
                new Instruction { InstructionId = 26, Description = "Plate the soup with sprinkled corriander and chillies", RecipeId = 8, OrderId = 4 }
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
        }
    }
}
