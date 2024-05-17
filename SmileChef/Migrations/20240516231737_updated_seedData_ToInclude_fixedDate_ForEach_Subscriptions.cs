using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChefApp.Migrations
{
    /// <inheritdoc />
    public partial class updated_seedData_ToInclude_fixedDate_ForEach_Subscriptions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Instructions",
                keyColumn: "InstructionId",
                keyValue: 1,
                column: "Duration",
                value: new TimeOnly(0, 30, 0));

            migrationBuilder.UpdateData(
                table: "Instructions",
                keyColumn: "InstructionId",
                keyValue: 2,
                column: "Duration",
                value: new TimeOnly(0, 15, 0));

            migrationBuilder.UpdateData(
                table: "Instructions",
                keyColumn: "InstructionId",
                keyValue: 3,
                column: "Duration",
                value: new TimeOnly(1, 0, 0));

            migrationBuilder.UpdateData(
                table: "Instructions",
                keyColumn: "InstructionId",
                keyValue: 4,
                column: "Duration",
                value: new TimeOnly(0, 10, 0));

            migrationBuilder.UpdateData(
                table: "Instructions",
                keyColumn: "InstructionId",
                keyValue: 6,
                column: "Duration",
                value: new TimeOnly(0, 5, 0));

            migrationBuilder.UpdateData(
                table: "Instructions",
                keyColumn: "InstructionId",
                keyValue: 8,
                column: "Duration",
                value: new TimeOnly(0, 20, 0));

            migrationBuilder.UpdateData(
                table: "Instructions",
                keyColumn: "InstructionId",
                keyValue: 13,
                column: "Duration",
                value: new TimeOnly(0, 10, 0));

            migrationBuilder.UpdateData(
                table: "Instructions",
                keyColumn: "InstructionId",
                keyValue: 14,
                column: "Duration",
                value: new TimeOnly(1, 0, 0));

            migrationBuilder.UpdateData(
                table: "Subscriptions",
                keyColumn: "SubscriptionId",
                keyValue: 1,
                column: "SubscriptionDate",
                value: new DateTime(2024, 5, 17, 0, 0, 0, 0, DateTimeKind.Local));

            migrationBuilder.UpdateData(
                table: "Subscriptions",
                keyColumn: "SubscriptionId",
                keyValue: 2,
                column: "SubscriptionDate",
                value: new DateTime(2024, 5, 17, 0, 0, 0, 0, DateTimeKind.Local));

            migrationBuilder.UpdateData(
                table: "Subscriptions",
                keyColumn: "SubscriptionId",
                keyValue: 3,
                column: "SubscriptionDate",
                value: new DateTime(2024, 5, 17, 0, 0, 0, 0, DateTimeKind.Local));

            migrationBuilder.UpdateData(
                table: "Subscriptions",
                keyColumn: "SubscriptionId",
                keyValue: 4,
                column: "SubscriptionDate",
                value: new DateTime(2024, 5, 17, 0, 0, 0, 0, DateTimeKind.Local));

            migrationBuilder.UpdateData(
                table: "Subscriptions",
                keyColumn: "SubscriptionId",
                keyValue: 5,
                column: "SubscriptionDate",
                value: new DateTime(2024, 5, 17, 0, 0, 0, 0, DateTimeKind.Local));

            migrationBuilder.UpdateData(
                table: "Subscriptions",
                keyColumn: "SubscriptionId",
                keyValue: 6,
                column: "SubscriptionDate",
                value: new DateTime(2024, 5, 17, 0, 0, 0, 0, DateTimeKind.Local));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
                keyValue: 6,
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
    }
}
