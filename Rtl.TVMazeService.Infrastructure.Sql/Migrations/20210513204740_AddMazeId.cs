using Microsoft.EntityFrameworkCore.Migrations;

namespace Rtl.TVMazeService.Infrastructure.Sql.Migrations
{
    public partial class AddMazeId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MazeId",
                table: "Shows",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MazeId",
                table: "CastMembers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Shows_MazeId",
                table: "Shows",
                column: "MazeId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CastMembers_MazeId",
                table: "CastMembers",
                column: "MazeId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Shows_MazeId",
                table: "Shows");

            migrationBuilder.DropIndex(
                name: "IX_CastMembers_MazeId",
                table: "CastMembers");

            migrationBuilder.DropColumn(
                name: "MazeId",
                table: "Shows");

            migrationBuilder.DropColumn(
                name: "MazeId",
                table: "CastMembers");
        }
    }
}
