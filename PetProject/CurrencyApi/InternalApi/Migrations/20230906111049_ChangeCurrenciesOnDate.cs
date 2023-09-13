using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InternalApi.Migrations
{
    /// <inheritdoc />
    public partial class ChangeCurrenciesOnDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "currencies_as_json",
                schema: "cur",
                table: "currencies_on_dates",
                newName: "currencies");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "currencies",
                schema: "cur",
                table: "currencies_on_dates",
                newName: "currencies_as_json");
        }
    }
}
