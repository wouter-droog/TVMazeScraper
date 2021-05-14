using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Rtl.TVMazeService.Infrastructure.Sql.Migrations
{
    public partial class AddBirthday : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "BirthDay",
                table: "CastMembers",
                type: "date",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CastMembers_BirthDay",
                table: "CastMembers",
                column: "BirthDay");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CastMembers_BirthDay",
                table: "CastMembers");

            migrationBuilder.DropColumn(
                name: "BirthDay",
                table: "CastMembers");
        }
    }
}
