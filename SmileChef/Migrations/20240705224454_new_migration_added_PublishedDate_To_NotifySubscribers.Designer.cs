﻿// <auto-generated />
using System;
using ChefApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace SmileChef.Migrations
{
    [DbContext(typeof(ChefAppContext))]
    [Migration("20240705224454_new_migration_added_PublishedDate_To_NotifySubscribers")]
    partial class new_migration_added_PublishedDate_To_NotifySubscribers
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ChefApp.Models.DbModels.Chef", b =>
                {
                    b.Property<int>("ChefId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ChefId"));

                    b.Property<decimal?>("AccountBalance")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Rating")
                        .HasMaxLength(5)
                        .HasColumnType("int");

                    b.Property<decimal?>("SubscriptionCost")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("ChefId");

                    b.HasIndex("UserId");

                    b.ToTable("Chefs");

                    b.HasData(
                        new
                        {
                            ChefId = 1,
                            AccountBalance = 10000m,
                            FirstName = "Gordon",
                            LastName = "Ramsay",
                            Rating = 3,
                            SubscriptionCost = 12.99m,
                            UserId = 1
                        },
                        new
                        {
                            ChefId = 2,
                            AccountBalance = 5500m,
                            FirstName = "Jamie",
                            LastName = "Oliver",
                            Rating = 1,
                            SubscriptionCost = 11.99m,
                            UserId = 2
                        },
                        new
                        {
                            ChefId = 3,
                            AccountBalance = 9000m,
                            FirstName = "Wolfgang",
                            LastName = "Puck",
                            Rating = 5,
                            SubscriptionCost = 5.99m,
                            UserId = 3
                        },
                        new
                        {
                            ChefId = 4,
                            AccountBalance = 15000m,
                            FirstName = "Alice",
                            LastName = "Waters",
                            Rating = 4,
                            SubscriptionCost = 6.99m,
                            UserId = 4
                        },
                        new
                        {
                            ChefId = 5,
                            AccountBalance = 8000m,
                            FirstName = "Thomas",
                            LastName = "Keller",
                            Rating = 2,
                            SubscriptionCost = 15.99m,
                            UserId = 5
                        },
                        new
                        {
                            ChefId = 6,
                            AccountBalance = 7000m,
                            FirstName = "Emeril",
                            LastName = "Lagasse",
                            Rating = 5,
                            SubscriptionCost = 10.99m,
                            UserId = 6
                        });
                });

            modelBuilder.Entity("ChefApp.Models.DbModels.Instruction", b =>
                {
                    b.Property<int>("InstructionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("InstructionId"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<TimeSpan?>("Duration")
                        .HasColumnType("time");

                    b.Property<int>("OrderId")
                        .HasColumnType("int");

                    b.Property<int>("RecipeId")
                        .HasColumnType("int");

                    b.HasKey("InstructionId");

                    b.HasIndex("RecipeId");

                    b.ToTable("Instructions");

                    b.HasData(
                        new
                        {
                            InstructionId = 1,
                            Description = "Prepare beef",
                            Duration = new TimeSpan(0, 0, 10, 0, 0),
                            OrderId = 1,
                            RecipeId = 1
                        },
                        new
                        {
                            InstructionId = 2,
                            Description = "Wrap in puff pastry",
                            OrderId = 2,
                            RecipeId = 1
                        },
                        new
                        {
                            InstructionId = 3,
                            Description = "Bake",
                            Duration = new TimeSpan(0, 0, 20, 0, 0),
                            OrderId = 3,
                            RecipeId = 1
                        },
                        new
                        {
                            InstructionId = 4,
                            Description = "Cook pasta",
                            Duration = new TimeSpan(0, 0, 15, 0, 0),
                            OrderId = 1,
                            RecipeId = 2
                        },
                        new
                        {
                            InstructionId = 5,
                            Description = "Prepare sauce",
                            OrderId = 2,
                            RecipeId = 2
                        },
                        new
                        {
                            InstructionId = 6,
                            Description = "Combine and serve",
                            Duration = new TimeSpan(0, 0, 3, 0, 0),
                            OrderId = 3,
                            RecipeId = 2
                        },
                        new
                        {
                            InstructionId = 7,
                            Description = "Prepare tuna",
                            OrderId = 1,
                            RecipeId = 3
                        },
                        new
                        {
                            InstructionId = 8,
                            Description = "Roll sushi",
                            Duration = new TimeSpan(0, 0, 5, 0, 0),
                            OrderId = 2,
                            RecipeId = 3
                        },
                        new
                        {
                            InstructionId = 9,
                            Description = "Serve with soy sauce",
                            OrderId = 3,
                            RecipeId = 3
                        },
                        new
                        {
                            InstructionId = 10,
                            Description = "Prepare lentils",
                            OrderId = 1,
                            RecipeId = 4
                        },
                        new
                        {
                            InstructionId = 11,
                            Description = "Cook lentils",
                            Duration = new TimeSpan(0, 0, 15, 0, 0),
                            OrderId = 2,
                            RecipeId = 4
                        },
                        new
                        {
                            InstructionId = 12,
                            Description = "Serve hot",
                            OrderId = 3,
                            RecipeId = 4
                        },
                        new
                        {
                            InstructionId = 13,
                            Description = "Season chicken",
                            OrderId = 1,
                            RecipeId = 5
                        },
                        new
                        {
                            InstructionId = 14,
                            Description = "Roast chicken",
                            Duration = new TimeSpan(0, 0, 45, 0, 0),
                            OrderId = 2,
                            RecipeId = 5
                        },
                        new
                        {
                            InstructionId = 15,
                            Description = "Serve with vegetables",
                            OrderId = 3,
                            RecipeId = 5
                        },
                        new
                        {
                            InstructionId = 16,
                            Description = "Prepare bananas",
                            Duration = new TimeSpan(0, 0, 15, 0, 0),
                            OrderId = 1,
                            RecipeId = 6
                        },
                        new
                        {
                            InstructionId = 17,
                            Description = "Cook with butter and sugar",
                            OrderId = 2,
                            RecipeId = 6
                        },
                        new
                        {
                            InstructionId = 18,
                            Description = "Serve with ice cream",
                            OrderId = 3,
                            RecipeId = 6
                        },
                        new
                        {
                            InstructionId = 19,
                            Description = "Cook Mince Beef",
                            Duration = new TimeSpan(0, 0, 10, 0, 0),
                            OrderId = 1,
                            RecipeId = 7
                        },
                        new
                        {
                            InstructionId = 20,
                            Description = "Cook Tomato Sauce",
                            Duration = new TimeSpan(0, 0, 25, 0, 0),
                            OrderId = 2,
                            RecipeId = 7
                        },
                        new
                        {
                            InstructionId = 21,
                            Description = "Boil Sphagetti",
                            Duration = new TimeSpan(0, 0, 15, 0, 0),
                            OrderId = 3,
                            RecipeId = 7
                        },
                        new
                        {
                            InstructionId = 22,
                            Description = "Mix Cooked Beef, Suace, and Sphagetti",
                            OrderId = 4,
                            RecipeId = 7
                        },
                        new
                        {
                            InstructionId = 23,
                            Description = "Cook Chicken Soup",
                            Duration = new TimeSpan(0, 0, 35, 0, 0),
                            OrderId = 1,
                            RecipeId = 8
                        },
                        new
                        {
                            InstructionId = 24,
                            Description = "Add chopped Mushroom, Cillantro, Corriander",
                            Duration = new TimeSpan(0, 0, 2, 0, 0),
                            OrderId = 2,
                            RecipeId = 8
                        },
                        new
                        {
                            InstructionId = 25,
                            Description = "Simmer the ingredients",
                            Duration = new TimeSpan(0, 0, 5, 0, 0),
                            OrderId = 3,
                            RecipeId = 8
                        },
                        new
                        {
                            InstructionId = 26,
                            Description = "Plate the soup with sprinkled corriander and chillies",
                            OrderId = 4,
                            RecipeId = 8
                        });
                });

            modelBuilder.Entity("ChefApp.Models.DbModels.Recipe", b =>
                {
                    b.Property<int>("RecipeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RecipeId"));

                    b.Property<int>("ChefId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("RecipeId");

                    b.HasIndex("ChefId");

                    b.ToTable("Recipes");

                    b.HasData(
                        new
                        {
                            RecipeId = 1,
                            ChefId = 1,
                            Name = "Beef Wellington"
                        },
                        new
                        {
                            RecipeId = 2,
                            ChefId = 2,
                            Name = "Pasta Carbonara"
                        },
                        new
                        {
                            RecipeId = 3,
                            ChefId = 3,
                            Name = "Spicy Tuna Rolls"
                        },
                        new
                        {
                            RecipeId = 4,
                            ChefId = 4,
                            Name = "Lentil Soup"
                        },
                        new
                        {
                            RecipeId = 5,
                            ChefId = 5,
                            Name = "Roast Chicken"
                        },
                        new
                        {
                            RecipeId = 6,
                            ChefId = 6,
                            Name = "Bananas Foster"
                        },
                        new
                        {
                            RecipeId = 7,
                            ChefId = 1,
                            Name = "Beef Bolognese"
                        },
                        new
                        {
                            RecipeId = 8,
                            ChefId = 1,
                            Name = "Chicken Mushroom Soup"
                        });
                });

            modelBuilder.Entity("ChefApp.Models.DbModels.Subscription", b =>
                {
                    b.Property<int>("SubscriptionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnOrder(0);

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SubscriptionId"));

                    b.Property<decimal>("Amount")
                        .HasPrecision(8, 2)
                        .HasColumnType("decimal(8,2)")
                        .HasColumnOrder(4);

                    b.Property<int>("PublisherId")
                        .HasColumnType("int")
                        .HasColumnOrder(2);

                    b.Property<int>("SubscriberId")
                        .HasColumnType("int")
                        .HasColumnOrder(1);

                    b.Property<DateTime>("SubscriptionDate")
                        .HasColumnType("datetime2")
                        .HasColumnOrder(3);

                    b.Property<string>("SubscriptionType")
                        .IsRequired()
                        .HasColumnType("nvarchar(25)")
                        .HasColumnOrder(5);

                    b.HasKey("SubscriptionId");

                    b.HasIndex("PublisherId");

                    b.HasIndex("SubscriberId");

                    b.ToTable("Subscriptions");

                    b.HasData(
                        new
                        {
                            SubscriptionId = 1,
                            Amount = 99.99m,
                            PublisherId = 1,
                            SubscriberId = 3,
                            SubscriptionDate = new DateTime(2024, 5, 17, 0, 0, 0, 0, DateTimeKind.Local),
                            SubscriptionType = "Monthly"
                        },
                        new
                        {
                            SubscriptionId = 2,
                            Amount = 99.99m,
                            PublisherId = 1,
                            SubscriberId = 4,
                            SubscriptionDate = new DateTime(2024, 5, 17, 0, 0, 0, 0, DateTimeKind.Local),
                            SubscriptionType = "Monthly"
                        },
                        new
                        {
                            SubscriptionId = 3,
                            Amount = 99.99m,
                            PublisherId = 1,
                            SubscriberId = 5,
                            SubscriptionDate = new DateTime(2024, 5, 17, 0, 0, 0, 0, DateTimeKind.Local),
                            SubscriptionType = "Monthly"
                        },
                        new
                        {
                            SubscriptionId = 4,
                            Amount = 199.99m,
                            PublisherId = 2,
                            SubscriberId = 4,
                            SubscriptionDate = new DateTime(2024, 5, 17, 0, 0, 0, 0, DateTimeKind.Local),
                            SubscriptionType = "Yearly"
                        },
                        new
                        {
                            SubscriptionId = 5,
                            Amount = 199.99m,
                            PublisherId = 2,
                            SubscriberId = 5,
                            SubscriptionDate = new DateTime(2024, 5, 17, 0, 0, 0, 0, DateTimeKind.Local),
                            SubscriptionType = "Yearly"
                        },
                        new
                        {
                            SubscriptionId = 6,
                            Amount = 199.99m,
                            PublisherId = 2,
                            SubscriberId = 6,
                            SubscriptionDate = new DateTime(2024, 5, 17, 0, 0, 0, 0, DateTimeKind.Local),
                            SubscriptionType = "Yearly"
                        });
                });

            modelBuilder.Entity("SmileChef.Models.DbModels.NotifySubscribers", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("Notified")
                        .HasColumnType("bit");

                    b.Property<DateTime>("PublishedDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("PublisherId")
                        .HasColumnType("int");

                    b.Property<int>("RecipeId")
                        .HasColumnType("int");

                    b.Property<int>("SubscriberId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PublisherId");

                    b.HasIndex("RecipeId");

                    b.HasIndex("SubscriberId");

                    b.ToTable("NotifySubscribers");
                });

            modelBuilder.Entity("SmileChef.Models.DbModels.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserId"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.HasKey("UserId");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            UserId = 1,
                            Email = "gordan@gmail.com",
                            Password = "123"
                        },
                        new
                        {
                            UserId = 2,
                            Email = "jamie@gmail.com",
                            Password = "123"
                        },
                        new
                        {
                            UserId = 3,
                            Email = "wolfgang@gmail.com",
                            Password = "123"
                        },
                        new
                        {
                            UserId = 4,
                            Email = "alice@gmail.com",
                            Password = "123"
                        },
                        new
                        {
                            UserId = 5,
                            Email = "thomas@gmail.com",
                            Password = "123"
                        },
                        new
                        {
                            UserId = 6,
                            Email = "emeril@gmail.com",
                            Password = "123"
                        });
                });

            modelBuilder.Entity("ChefApp.Models.DbModels.Chef", b =>
                {
                    b.HasOne("SmileChef.Models.DbModels.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("ChefApp.Models.DbModels.Instruction", b =>
                {
                    b.HasOne("ChefApp.Models.DbModels.Recipe", "Recipe")
                        .WithMany("Instructions")
                        .HasForeignKey("RecipeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Recipe");
                });

            modelBuilder.Entity("ChefApp.Models.DbModels.Recipe", b =>
                {
                    b.HasOne("ChefApp.Models.DbModels.Chef", "Chef")
                        .WithMany("Recipes")
                        .HasForeignKey("ChefId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Chef");
                });

            modelBuilder.Entity("ChefApp.Models.DbModels.Subscription", b =>
                {
                    b.HasOne("ChefApp.Models.DbModels.Chef", "Publisher")
                        .WithMany("PublishedSubscriptions")
                        .HasForeignKey("PublisherId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("ChefApp.Models.DbModels.Chef", "Subscriber")
                        .WithMany("SubscribedTo")
                        .HasForeignKey("SubscriberId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Publisher");

                    b.Navigation("Subscriber");
                });

            modelBuilder.Entity("SmileChef.Models.DbModels.NotifySubscribers", b =>
                {
                    b.HasOne("ChefApp.Models.DbModels.Chef", "Publisher")
                        .WithMany()
                        .HasForeignKey("PublisherId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ChefApp.Models.DbModels.Recipe", "Recipe")
                        .WithMany()
                        .HasForeignKey("RecipeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ChefApp.Models.DbModels.Chef", "Subscriber")
                        .WithMany()
                        .HasForeignKey("SubscriberId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Publisher");

                    b.Navigation("Recipe");

                    b.Navigation("Subscriber");
                });

            modelBuilder.Entity("ChefApp.Models.DbModels.Chef", b =>
                {
                    b.Navigation("PublishedSubscriptions");

                    b.Navigation("Recipes");

                    b.Navigation("SubscribedTo");
                });

            modelBuilder.Entity("ChefApp.Models.DbModels.Recipe", b =>
                {
                    b.Navigation("Instructions");
                });
#pragma warning restore 612, 618
        }
    }
}
