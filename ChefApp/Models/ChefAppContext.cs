using ChefApp.Extensions;
using ChefApp.Models.DbModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ChefApp.Models
{
    public class ChefAppContext : DbContext
    {
        private readonly IConfiguration _config;
        private readonly ISqlSettings _sqlSettings;

        public DbSet<Chef> Chefs { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Instruction> Instructions { get; set; }

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
            } else
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

            modelBuilder.SeedAppData();
        }
    }
}
