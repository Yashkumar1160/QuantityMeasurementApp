using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuantityMeasurementAppRepositories.Migrations
{
    /// <inheritdoc />
    public partial class AddUserIdToQuantityMeasurements : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "user_id",
                table: "users",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "user_id",
                table: "quantity_measurements",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_quantity_measurements_user_id",
                table: "quantity_measurements",
                column: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_quantity_measurements_user_id",
                table: "quantity_measurements");

            migrationBuilder.DropColumn(
                name: "user_id",
                table: "users");

            migrationBuilder.DropColumn(
                name: "user_id",
                table: "quantity_measurements");
        }
    }
}
