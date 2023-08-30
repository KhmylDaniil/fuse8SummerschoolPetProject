using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InternalApi.Migrations
{
    /// <inheritdoc />
    public partial class AddChangeCacheTask : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "change_cache_tasks",
                schema: "cur",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    creation_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    new_base_currency = table.Column<int>(type: "integer", nullable: false),
                    cache_task_status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_change_cache_tasks", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "change_cache_tasks",
                schema: "cur");
        }
    }
}
