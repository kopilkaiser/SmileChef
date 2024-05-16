using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChefApp.Migrations
{
    /// <inheritdoc />
    public partial class added_Duration_Column_To_Instructions_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<TimeOnly>(
                name: "Duration",
                table: "Instructions",
                type: "time",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Instructions",
                keyColumn: "InstructionId",
                keyValue: 1,
                column: "Duration",
                value: null);

            migrationBuilder.UpdateData(
                table: "Instructions",
                keyColumn: "InstructionId",
                keyValue: 2,
                column: "Duration",
                value: null);

            migrationBuilder.UpdateData(
                table: "Instructions",
                keyColumn: "InstructionId",
                keyValue: 3,
                column: "Duration",
                value: null);

            migrationBuilder.UpdateData(
                table: "Instructions",
                keyColumn: "InstructionId",
                keyValue: 4,
                column: "Duration",
                value: null);

            migrationBuilder.UpdateData(
                table: "Instructions",
                keyColumn: "InstructionId",
                keyValue: 5,
                column: "Duration",
                value: null);

            migrationBuilder.UpdateData(
                table: "Instructions",
                keyColumn: "InstructionId",
                keyValue: 6,
                column: "Duration",
                value: null);

            migrationBuilder.UpdateData(
                table: "Instructions",
                keyColumn: "InstructionId",
                keyValue: 7,
                column: "Duration",
                value: null);

            migrationBuilder.UpdateData(
                table: "Instructions",
                keyColumn: "InstructionId",
                keyValue: 8,
                column: "Duration",
                value: null);

            migrationBuilder.UpdateData(
                table: "Instructions",
                keyColumn: "InstructionId",
                keyValue: 9,
                column: "Duration",
                value: null);

            migrationBuilder.UpdateData(
                table: "Instructions",
                keyColumn: "InstructionId",
                keyValue: 10,
                column: "Duration",
                value: null);

            migrationBuilder.UpdateData(
                table: "Instructions",
                keyColumn: "InstructionId",
                keyValue: 11,
                column: "Duration",
                value: null);

            migrationBuilder.UpdateData(
                table: "Instructions",
                keyColumn: "InstructionId",
                keyValue: 12,
                column: "Duration",
                value: null);

            migrationBuilder.UpdateData(
                table: "Instructions",
                keyColumn: "InstructionId",
                keyValue: 13,
                column: "Duration",
                value: null);

            migrationBuilder.UpdateData(
                table: "Instructions",
                keyColumn: "InstructionId",
                keyValue: 14,
                column: "Duration",
                value: null);

            migrationBuilder.UpdateData(
                table: "Instructions",
                keyColumn: "InstructionId",
                keyValue: 15,
                column: "Duration",
                value: null);

            migrationBuilder.UpdateData(
                table: "Instructions",
                keyColumn: "InstructionId",
                keyValue: 16,
                column: "Duration",
                value: null);

            migrationBuilder.UpdateData(
                table: "Instructions",
                keyColumn: "InstructionId",
                keyValue: 17,
                column: "Duration",
                value: null);

            migrationBuilder.UpdateData(
                table: "Instructions",
                keyColumn: "InstructionId",
                keyValue: 18,
                column: "Duration",
                value: null);

            migrationBuilder.UpdateData(
                table: "Subscriptions",
                keyColumn: "SubscriptionId",
                keyValue: 1,
                column: "SubscriptionDate",
                value: new DateTime(2024, 5, 17, 0, 13, 14, 967, DateTimeKind.Local).AddTicks(7809));

            migrationBuilder.UpdateData(
                table: "Subscriptions",
                keyColumn: "SubscriptionId",
                keyValue: 2,
                column: "SubscriptionDate",
                value: new DateTime(2024, 5, 17, 0, 13, 14, 967, DateTimeKind.Local).AddTicks(7871));

            migrationBuilder.UpdateData(
                table: "Subscriptions",
                keyColumn: "SubscriptionId",
                keyValue: 3,
                column: "SubscriptionDate",
                value: new DateTime(2024, 5, 17, 0, 13, 14, 967, DateTimeKind.Local).AddTicks(7873));

            migrationBuilder.UpdateData(
                table: "Subscriptions",
                keyColumn: "SubscriptionId",
                keyValue: 4,
                column: "SubscriptionDate",
                value: new DateTime(2024, 5, 17, 0, 13, 14, 967, DateTimeKind.Local).AddTicks(7875));

            migrationBuilder.UpdateData(
                table: "Subscriptions",
                keyColumn: "SubscriptionId",
                keyValue: 5,
                column: "SubscriptionDate",
                value: new DateTime(2024, 5, 17, 0, 13, 14, 967, DateTimeKind.Local).AddTicks(7877));

            migrationBuilder.UpdateData(
                table: "Subscriptions",
                keyColumn: "SubscriptionId",
                keyValue: 6,
                column: "SubscriptionDate",
                value: new DateTime(2024, 5, 17, 0, 13, 14, 967, DateTimeKind.Local).AddTicks(7879));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Duration",
                table: "Instructions");

            migrationBuilder.UpdateData(
                table: "Subscriptions",
                keyColumn: "SubscriptionId",
                keyValue: 1,
                column: "SubscriptionDate",
                value: new DateTime(2024, 5, 17, 0, 10, 44, 159, DateTimeKind.Local).AddTicks(3432));

            migrationBuilder.UpdateData(
                table: "Subscriptions",
                keyColumn: "SubscriptionId",
                keyValue: 2,
                column: "SubscriptionDate",
                value: new DateTime(2024, 5, 17, 0, 10, 44, 159, DateTimeKind.Local).AddTicks(3492));

            migrationBuilder.UpdateData(
                table: "Subscriptions",
                keyColumn: "SubscriptionId",
                keyValue: 3,
                column: "SubscriptionDate",
                value: new DateTime(2024, 5, 17, 0, 10, 44, 159, DateTimeKind.Local).AddTicks(3494));

            migrationBuilder.UpdateData(
                table: "Subscriptions",
                keyColumn: "SubscriptionId",
                keyValue: 4,
                column: "SubscriptionDate",
                value: new DateTime(2024, 5, 17, 0, 10, 44, 159, DateTimeKind.Local).AddTicks(3496));

            migrationBuilder.UpdateData(
                table: "Subscriptions",
                keyColumn: "SubscriptionId",
                keyValue: 5,
                column: "SubscriptionDate",
                value: new DateTime(2024, 5, 17, 0, 10, 44, 159, DateTimeKind.Local).AddTicks(3498));

            migrationBuilder.UpdateData(
                table: "Subscriptions",
                keyColumn: "SubscriptionId",
                keyValue: 6,
                column: "SubscriptionDate",
                value: new DateTime(2024, 5, 17, 0, 10, 44, 159, DateTimeKind.Local).AddTicks(3500));
        }
    }
}
