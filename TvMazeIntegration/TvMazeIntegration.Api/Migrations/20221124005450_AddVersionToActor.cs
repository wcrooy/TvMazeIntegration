using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TvMazeIntegration.Api.Migrations
{
    public partial class AddVersionToActor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "Version",
                table: "Actors",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Version",
                table: "Actors");
        }
    }
}
