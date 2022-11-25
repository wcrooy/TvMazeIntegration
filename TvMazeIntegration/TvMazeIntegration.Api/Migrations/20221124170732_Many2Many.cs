using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TvMazeIntegration.Api.Migrations
{
    public partial class Many2Many : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActorShow_Actors_CastId",
                table: "ActorShow");

            migrationBuilder.DropForeignKey(
                name: "FK_ActorShow_Shows_ShowsId",
                table: "ActorShow");

            migrationBuilder.RenameColumn(
                name: "ShowsId",
                table: "ActorShow",
                newName: "ShowId");

            migrationBuilder.RenameColumn(
                name: "CastId",
                table: "ActorShow",
                newName: "ActorId");

            migrationBuilder.RenameIndex(
                name: "IX_ActorShow_ShowsId",
                table: "ActorShow",
                newName: "IX_ActorShow_ShowId");

            migrationBuilder.AddForeignKey(
                name: "FK_ActorShow_Actors_ActorId",
                table: "ActorShow",
                column: "ActorId",
                principalTable: "Actors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ActorShow_Shows_ShowId",
                table: "ActorShow",
                column: "ShowId",
                principalTable: "Shows",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActorShow_Actors_ActorId",
                table: "ActorShow");

            migrationBuilder.DropForeignKey(
                name: "FK_ActorShow_Shows_ShowId",
                table: "ActorShow");

            migrationBuilder.RenameColumn(
                name: "ShowId",
                table: "ActorShow",
                newName: "ShowsId");

            migrationBuilder.RenameColumn(
                name: "ActorId",
                table: "ActorShow",
                newName: "CastId");

            migrationBuilder.RenameIndex(
                name: "IX_ActorShow_ShowId",
                table: "ActorShow",
                newName: "IX_ActorShow_ShowsId");

            migrationBuilder.AddForeignKey(
                name: "FK_ActorShow_Actors_CastId",
                table: "ActorShow",
                column: "CastId",
                principalTable: "Actors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ActorShow_Shows_ShowsId",
                table: "ActorShow",
                column: "ShowsId",
                principalTable: "Shows",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
