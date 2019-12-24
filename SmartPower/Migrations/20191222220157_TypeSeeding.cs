using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartPower.Migrations
{
    public partial class TypeSeeding : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PrimarySource_Type_SourceTypeTypeId",
                table: "PrimarySource");

            migrationBuilder.DropTable(
                name: "Type");

            migrationBuilder.CreateTable(
                name: "SourceType",
                columns: table => new
                {
                    TypeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TypeName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SourceType", x => x.TypeId);
                });

            migrationBuilder.InsertData(
                table: "SourceType",
                columns: new[] { "TypeId", "TypeName" },
                values: new object[] { 1, "Transformer" });

            migrationBuilder.InsertData(
                table: "SourceType",
                columns: new[] { "TypeId", "TypeName" },
                values: new object[] { 2, "Machine" });

            migrationBuilder.InsertData(
                table: "SourceType",
                columns: new[] { "TypeId", "TypeName" },
                values: new object[] { 3, "SubMachine" });

            migrationBuilder.AddForeignKey(
                name: "FK_PrimarySource_SourceType_SourceTypeTypeId",
                table: "PrimarySource",
                column: "SourceTypeTypeId",
                principalTable: "SourceType",
                principalColumn: "TypeId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PrimarySource_SourceType_SourceTypeTypeId",
                table: "PrimarySource");

            migrationBuilder.DropTable(
                name: "SourceType");

            migrationBuilder.CreateTable(
                name: "Type",
                columns: table => new
                {
                    TypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TypeName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Type", x => x.TypeId);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_PrimarySource_Type_SourceTypeTypeId",
                table: "PrimarySource",
                column: "SourceTypeTypeId",
                principalTable: "Type",
                principalColumn: "TypeId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
