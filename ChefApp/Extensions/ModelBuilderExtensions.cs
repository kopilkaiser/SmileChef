using ChefApp.Models.DbModels;
using Microsoft.EntityFrameworkCore;

namespace ChefApp.Extensions
{
    public static class ModelBuilderExtensions
    {
        public static void SeedAppData(this ModelBuilder modelBuilder)
        {

            // Seed Chefs
            modelBuilder.Entity<Chef>().HasData(
                new Chef { ChefId = 1, FirstName = "Gordon", LastName = "Ramsay" },
                new Chef { ChefId = 2, FirstName = "Jamie", LastName = "Oliver" },
                new Chef { ChefId = 3, FirstName = "Wolfgang", LastName = "Puck" },
                new Chef { ChefId = 4, FirstName = "Alice", LastName = "Waters" },
                new Chef { ChefId = 5, FirstName = "Thomas", LastName = "Keller" },
                new Chef { ChefId = 6, FirstName = "Emeril", LastName = "Lagasse" }
            );

            // Seed Recipes
            modelBuilder.Entity<Recipe>().HasData(
                new Recipe { RecipeId = 1, Name = "Beef Wellington", ChefId = 1 },
                new Recipe { RecipeId = 2, Name = "Pasta Carbonara", ChefId = 2 },
                new Recipe { RecipeId = 3, Name = "Spicy Tuna Rolls", ChefId = 3 },
                new Recipe { RecipeId = 4, Name = "Lentil Soup", ChefId = 4 },
                new Recipe { RecipeId = 5, Name = "Roast Chicken", ChefId = 5 },
                new Recipe { RecipeId = 6, Name = "Bananas Foster", ChefId = 6 }
            );

            // Seed Instructions with optional Time field
            modelBuilder.Entity<Instruction>().HasData(
                new Instruction { InstructionId = 1, Description = "Prepare beef", RecipeId = 1, OrderId = 1, Duration = new TimeOnly(0, 30) },
                new Instruction { InstructionId = 2, Description = "Wrap in puff pastry", RecipeId = 1, OrderId = 2, Duration = new TimeOnly(0, 15) },
                new Instruction { InstructionId = 3, Description = "Bake", RecipeId = 1, OrderId = 3, Duration = new TimeOnly(1, 0) },
                new Instruction { InstructionId = 4, Description = "Cook pasta", RecipeId = 2, OrderId = 1, Duration = new TimeOnly(0, 10) },
                new Instruction { InstructionId = 5, Description = "Prepare sauce", RecipeId = 2, OrderId = 2 },
                new Instruction { InstructionId = 6, Description = "Combine and serve", RecipeId = 2, OrderId = 3, Duration = new TimeOnly(0, 5) },
                new Instruction { InstructionId = 7, Description = "Prepare tuna", RecipeId = 3, OrderId = 1 },
                new Instruction { InstructionId = 8, Description = "Roll sushi", RecipeId = 3, OrderId = 2, Duration = new TimeOnly(0, 20) },
                new Instruction { InstructionId = 9, Description = "Serve with soy sauce", RecipeId = 3, OrderId = 3 },
                new Instruction { InstructionId = 10, Description = "Prepare lentils", RecipeId = 4, OrderId = 1 },
                new Instruction { InstructionId = 11, Description = "Cook lentils", RecipeId = 4, OrderId = 2 },
                new Instruction { InstructionId = 12, Description = "Serve hot", RecipeId = 4, OrderId = 3 },
                new Instruction { InstructionId = 13, Description = "Season chicken", RecipeId = 5, OrderId = 1, Duration = new TimeOnly(0, 10) },
                new Instruction { InstructionId = 14, Description = "Roast chicken", RecipeId = 5, OrderId = 2, Duration = new TimeOnly(1, 0) },
                new Instruction { InstructionId = 15, Description = "Serve with vegetables", RecipeId = 5, OrderId = 3 },
                new Instruction { InstructionId = 16, Description = "Prepare bananas", RecipeId = 6, OrderId = 1 },
                new Instruction { InstructionId = 17, Description = "Cook with butter and sugar", RecipeId = 6, OrderId = 2 },
                new Instruction { InstructionId = 18, Description = "Serve with ice cream", RecipeId = 6, OrderId = 3 }
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
                }
            );
        }
    }
}
