using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuantityMeasurementAppRepositories.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "quantity_measurements",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    operation = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    first_value = table.Column<double>(type: "float", nullable: false),
                    first_unit = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    second_value = table.Column<double>(type: "float", nullable: false),
                    second_unit = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    result_value = table.Column<double>(type: "float", nullable: false),
                    measurement_type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    is_error = table.Column<bool>(type: "bit", nullable: false),
                    error_message = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_quantity_measurements", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_quantity_measurements_is_error",
                table: "quantity_measurements",
                column: "is_error");

            migrationBuilder.CreateIndex(
                name: "IX_quantity_measurements_measurement_type",
                table: "quantity_measurements",
                column: "measurement_type");

            migrationBuilder.CreateIndex(
                name: "IX_quantity_measurements_operation",
                table: "quantity_measurements",
                column: "operation");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "quantity_measurements");
        }
    }
}
