using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SmileChef.Migrations
{
    /// <inheritdoc />
    public partial class Added_fewSubscribed_Subscriptions_for_ChefId_1_ : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Subscriptions",
                columns: new[] { "SubscriptionId", "Amount", "PublisherId", "SubscriberId", "SubscriptionDate", "SubscriptionType" },
                values: new object[,]
                {
                    { 7, 50.00m, 2, 1, new DateTime(2024, 5, 17, 0, 0, 0, 0, DateTimeKind.Local), "Monthly" },
                    { 8, 75.00m, 3, 1, new DateTime(2024, 5, 17, 0, 0, 0, 0, DateTimeKind.Local), "Monthly" },
                    { 9, 60.00m, 4, 1, new DateTime(2024, 5, 17, 0, 0, 0, 0, DateTimeKind.Local), "Monthly" },
                    { 10, 80.00m, 5, 1, new DateTime(2024, 5, 17, 0, 0, 0, 0, DateTimeKind.Local), "Monthly" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Subscriptions",
                keyColumn: "SubscriptionId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Subscriptions",
                keyColumn: "SubscriptionId",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Subscriptions",
                keyColumn: "SubscriptionId",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Subscriptions",
                keyColumn: "SubscriptionId",
                keyValue: 10);
        }
    }
}
