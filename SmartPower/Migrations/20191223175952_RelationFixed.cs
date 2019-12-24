using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartPower.Migrations
{
    public partial class RelationFixed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PrimarySource_SourceType_SourceTypeTypeId",
                table: "PrimarySource");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SourceType",
                table: "SourceType");

            migrationBuilder.RenameTable(
                name: "SourceType",
                newName: "sourceType");

            migrationBuilder.AddPrimaryKey(
                name: "PK_sourceType",
                table: "sourceType",
                column: "TypeId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_PrimarySource_sourceType_SourceTypeTypeId",
                table: "PrimarySource",
                column: "SourceTypeTypeId",
                principalTable: "sourceType",
                principalColumn: "TypeId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PrimarySource_sourceType_SourceTypeTypeId",
                table: "PrimarySource");

            migrationBuilder.DropTable(
                name: "SourceRelations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_sourceType",
                table: "sourceType");

            migrationBuilder.RenameTable(
                name: "sourceType",
                newName: "SourceType");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SourceType",
                table: "SourceType",
                column: "TypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_PrimarySource_SourceType_SourceTypeTypeId",
                table: "PrimarySource",
                column: "SourceTypeTypeId",
                principalTable: "SourceType",
                principalColumn: "TypeId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
