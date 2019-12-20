using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartPower.Migrations
{
    public partial class mighh : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Load_Functions_FunctionNameId",
                table: "Load");

            migrationBuilder.DropIndex(
                name: "IX_Load_FunctionNameId",
                table: "Load");

            migrationBuilder.DropColumn(
                name: "FunctionNameId",
                table: "Load");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FunctionNameId",
                table: "Load",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Load_FunctionNameId",
                table: "Load",
                column: "FunctionNameId");

            migrationBuilder.AddForeignKey(
                name: "FK_Load_Functions_FunctionNameId",
                table: "Load",
                column: "FunctionNameId",
                principalTable: "Functions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
