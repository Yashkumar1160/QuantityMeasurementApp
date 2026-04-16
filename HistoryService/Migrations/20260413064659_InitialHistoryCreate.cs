using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HistoryService.Migrations
{
    /// <inheritdoc />
    public partial class InitialHistoryCreate : Migration
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
                    user_id = table.Column<long>(type: "bigint", nullable: false),
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
                name: "IX_quantity_measurements_operation",
                table: "quantity_measurements",
                column: "operation");

            migrationBuilder.CreateIndex(
                name: "IX_quantity_measurements_user_id",
                table: "quantity_measurements",
                column: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "quantity_measurements");
        }
    }
}
