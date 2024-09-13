using ChefApp.Extensions;
using ChefApp.Models.DbModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SmileChef.Models.DbModels;
using SmileChef.Models.Enums;

namespace ChefApp.Models
{
    public class ChefAppContext : DbContext
    {
        private readonly IConfiguration _config;
        private readonly ISqlSettings _sqlSettings;

        public DbSet<Chef> Chefs { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<PremiumRecipe> PremiumRecipes { get; set; }

        public DbSet<Instruction> Instructions { get; set; }
        public DbSet<NotifySubscribers> NotifySubsribers { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<RecipeBookmark> RecipeBookmarks { get; set; }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderLine> OrderLines { get; set; }
        public DbSet<Restaurant> Restaurants { get; set; }

        public DbSet<SupportMessage> SupportMessages { get; set; }
        public ChefAppContext(DbContextOptions<ChefAppContext> options, IConfiguration config, ISqlSettings sqlSettings) : base(options)
        {
            _config = config;
            _sqlSettings = sqlSettings;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (_sqlSettings.InMemory)
            {
                optionsBuilder.UseInMemoryDatabase("chefAppDbInMemory");
                return;
            }
            else
            {
                var connString = _sqlSettings.ConnectionString;
                optionsBuilder.UseSqlServer(connString);
            }

            // Enable sensitive data logging
            optionsBuilder.EnableSensitiveDataLogging();

            // Optional: Enable detailed error messages
            optionsBuilder.EnableDetailedErrors();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Define the value converter
            var subscriptionTypeConverter = new ValueConverter<SubscriptionType, string>(
                v => v.ToString(),
                v => (SubscriptionType)Enum.Parse(typeof(SubscriptionType), v));

            // Apply the converter to the Subscription model
            modelBuilder.Entity<Subscription>()
                .Property(s => s.SubscriptionType)
                .HasConversion(subscriptionTypeConverter)
                .HasColumnType("nvarchar(25)"); // Adjust the length as needed

            modelBuilder.Entity<Subscription>()
                        .HasOne(s => s.Subscriber)
                        .WithMany(c => c.SubscribedTo)
                        .HasForeignKey(s => s.SubscriberId)
                        .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Subscription>()
                       .HasOne(s => s.Publisher)
                       .WithMany(c => c.PublishedSubscriptions)
                       .HasForeignKey(s => s.PublisherId)
                       .OnDelete(DeleteBehavior.NoAction);

            // Configure inheritance for Recipe and PremiumRecipe using RecipeType as discriminator
            modelBuilder.Entity<Recipe>()
               .HasDiscriminator<RecipeType>("RecipeType")
               .HasValue<Recipe>(RecipeType.Basic)
               .HasValue<PremiumRecipe>(RecipeType.Premium);

            modelBuilder.Entity<Recipe>()
                .Property(r => r.RecipeType)
                .HasConversion<string>();  // This line ensures that the enum is stored as a string in the database.

            modelBuilder.Entity<SupportMessage>()
                .HasOne(sm => sm.Sender)
                .WithMany(c => c.SupportMessages)  // A Chef can have many SupportMessages
                .HasForeignKey(sm => sm.ChefId)
                .OnDelete(DeleteBehavior.NoAction); // or Cascade depending on your requirements

            modelBuilder.Entity<SupportMessage>()
                .HasOne(sm => sm.AdminUser)
                .WithMany(u => u.HandledMessages) // AdminUser can handle many SupportMessages
                .HasForeignKey(sm => sm.UserId)
                .OnDelete(DeleteBehavior.NoAction); // or Cascade depending on your requirements

            modelBuilder.Entity<SupportMessage>()
                .Property(sm => sm.SupportType)
                .HasConversion<string>();

            modelBuilder.Entity<SupportMessage>()
               .Property(sm => sm.SupportStatus)
               .HasConversion<string>();

            modelBuilder.SeedAppData();
        }
    }
}
