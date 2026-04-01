using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InParadise.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateBookingTBL : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StripePaymentIntentId",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "StripeSessionId",
                table: "Bookings");

            migrationBuilder.AddColumn<int>(
                name: "RefId",
                table: "Bookings",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RefId",
                table: "Bookings");

            migrationBuilder.AddColumn<string>(
                name: "StripePaymentIntentId",
                table: "Bookings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StripeSessionId",
                table: "Bookings",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
