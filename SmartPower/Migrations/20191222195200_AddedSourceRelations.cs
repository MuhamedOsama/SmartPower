using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartPower.Migrations
{
    public partial class AddedSourceRelations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SourceTypeTypeId",
                table: "PrimarySource",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "powAvg",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    loadId = table.Column<int>(nullable: false),
                    pAvg1 = table.Column<decimal>(nullable: false),
                    pAvg2 = table.Column<decimal>(nullable: false),
                    pAvg3 = table.Column<decimal>(nullable: false),
                    date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_powAvg", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Type",
                columns: table => new
                {
                    TypeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TypeName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Type", x => x.TypeId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PrimarySource_SourceTypeTypeId",
                table: "PrimarySource",
                column: "SourceTypeTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_PrimarySource_Type_SourceTypeTypeId",
                table: "PrimarySource",
                column: "SourceTypeTypeId",
                principalTable: "Type",
                principalColumn: "TypeId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PrimarySource_Type_SourceTypeTypeId",
                table: "PrimarySource");

            migrationBuilder.DropTable(
                name: "powAvg");

            migrationBuilder.DropTable(
                name: "Type");

            migrationBuilder.DropIndex(
                name: "IX_PrimarySource_SourceTypeTypeId",
                table: "PrimarySource");

            migrationBuilder.DropColumn(
                name: "SourceTypeTypeId",
                table: "PrimarySource");
        }
    }
}
