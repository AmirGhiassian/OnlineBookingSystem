using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineBookingSystem.Migrations
{
    /// <inheritdoc />
    public partial class Didthing : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Restaurants_RestaurantResturantId",
                table: "Reservations");

            migrationBuilder.DropIndex(
                name: "IX_Reservations_RestaurantResturantId",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "RestaurantResturantId",
                table: "Reservations");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateTime",
                table: "Reservations",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "RestaurantId",
                table: "Reservations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Customers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateTime",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "RestaurantId",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "Customers");

            migrationBuilder.AddColumn<string>(
                name: "Date",
                table: "Reservations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "RestaurantResturantId",
                table: "Reservations",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_RestaurantResturantId",
                table: "Reservations",
                column: "RestaurantResturantId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Restaurants_RestaurantResturantId",
                table: "Reservations",
                column: "RestaurantResturantId",
                principalTable: "Restaurants",
                principalColumn: "ResturantId");
        }
    }
}
