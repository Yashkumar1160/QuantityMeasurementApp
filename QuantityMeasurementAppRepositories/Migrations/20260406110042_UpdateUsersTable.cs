using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuantityMeasurementAppRepositories.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUsersTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_quantity_measurements_is_error",
                table: "quantity_measurements");

            migrationBuilder.DropIndex(
                name: "IX_quantity_measurements_measurement_type",
                table: "quantity_measurements");

            migrationBuilder.AddColumn<string>(
                name: "role",
                table: "users",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "User");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "role",
                table: "users");

            migrationBuilder.CreateIndex(
                name: "IX_quantity_measurements_is_error",
                table: "quantity_measurements",
                column: "is_error");

            migrationBuilder.CreateIndex(
                name: "IX_quantity_measurements_measurement_type",
                table: "quantity_measurements",
                column: "measurement_type");
        }
    }
}
