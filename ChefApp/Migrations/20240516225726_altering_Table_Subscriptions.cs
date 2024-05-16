using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChefApp.Migrations
{
    /// <inheritdoc />
    public partial class altering_Table_Subscriptions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TypeOfSubscription",
                table: "Subscriptions");

            migrationBuilder.AddColumn<string>(
                name: "SubscriptionType",
                table: "Subscriptions",
                type: "nvarchar(25)",
                nullable: false,
                defaultValue: "")
                .Annotation("Relational:ColumnOrder", 5);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SubscriptionType",
                table: "Subscriptions");

            migrationBuilder.AddColumn<int>(
                name: "TypeOfSubscription",
                table: "Subscriptions",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("Relational:ColumnOrder", 5);
        }
    }
}
