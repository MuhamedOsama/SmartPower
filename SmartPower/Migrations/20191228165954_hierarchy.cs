using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartPower.Migrations
{
    public partial class hierarchy : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SourceTypeId",
                table: "PrimarySource",
                nullable: false,
                defaultValue: 0);

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
                name: "SourceRelations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChildId = table.Column<int>(nullable: false),
                    ParentId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SourceRelations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "sourceType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TypeName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sourceType", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "sourceType",
                columns: new[] { "Id", "TypeName" },
                values: new object[] { 1, "Transformer" });

            migrationBuilder.InsertData(
                table: "sourceType",
                columns: new[] { "Id", "TypeName" },
                values: new object[] { 2, "Machine" });

            migrationBuilder.InsertData(
                table: "sourceType",
                columns: new[] { "Id", "TypeName" },
                values: new object[] { 3, "SubMachine" });

            migrationBuilder.CreateIndex(
                name: "IX_PrimarySource_SourceTypeId",
                table: "PrimarySource",
                column: "SourceTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_PrimarySource_sourceType_SourceTypeId",
                table: "PrimarySource",
                column: "SourceTypeId",
                principalTable: "sourceType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PrimarySource_sourceType_SourceTypeId",
                table: "PrimarySource");

            migrationBuilder.DropTable(
                name: "powAvg");

            migrationBuilder.DropTable(
                name: "SourceRelations");

            migrationBuilder.DropTable(
                name: "sourceType");

            migrationBuilder.DropIndex(
                name: "IX_PrimarySource_SourceTypeId",
                table: "PrimarySource");

            migrationBuilder.DropColumn(
                name: "SourceTypeId",
                table: "PrimarySource");
        }
    }
}
