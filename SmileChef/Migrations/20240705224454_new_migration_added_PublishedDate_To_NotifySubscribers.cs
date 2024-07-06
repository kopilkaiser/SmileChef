using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmileChef.Migrations
{
    /// <inheritdoc />
    public partial class new_migration_added_PublishedDate_To_NotifySubscribers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "PublishedDate",
                table: "NotifySubscribers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PublishedDate",
                table: "NotifySubscribers");
        }
    }
}
