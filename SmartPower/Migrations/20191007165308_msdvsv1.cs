using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace SmartPower.Migrations
{
    public partial class msdvsv1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "businessTypeFac",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    busnisstype = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_businessTypeFac", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CurrentSpikeLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PhaseName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SourceCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Time = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Value = table.Column<decimal>(type: "decimal(18, 2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrentSpikeLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Factory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Scale = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    businessType = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Factory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Functions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FunctionName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Functions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Loadparameter",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Power = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    PowerFactor = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    RatingCurrent = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    RatingTemp = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    RatingVoltage = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Loadparameter", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PhasesConnection",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DestinationType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SourceId = table.Column<int>(type: "int", nullable: false),
                    SourceType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    dN1 = table.Column<int>(type: "int", nullable: false),
                    dN2 = table.Column<int>(type: "int", nullable: false),
                    dN3 = table.Column<int>(type: "int", nullable: false),
                    sN1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sN2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sN3 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhasesConnection", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "PowerAvg",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    P1 = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    P2 = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    P3 = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    loadId = table.Column<int>(type: "int", nullable: false),
                    primarySourceId = table.Column<int>(type: "int", nullable: true),
                    readingDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    secondrySourceId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PowerAvg", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "powerPeak",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    dateP1 = table.Column<DateTime>(type: "datetime2", nullable: false),
                    dateP2 = table.Column<DateTime>(type: "datetime2", nullable: false),
                    dateP3 = table.Column<DateTime>(type: "datetime2", nullable: false),
                    loadId = table.Column<int>(type: "int", nullable: false),
                    peakP1 = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    peakP2 = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    peakP3 = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    primarySourceId = table.Column<int>(type: "int", nullable: false),
                    secondrySourceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_powerPeak", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Production",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FacId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<double>(type: "float", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Production", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReportConstants",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Value = table.Column<decimal>(type: "decimal(18, 2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportConstants", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PrimarySource",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DesignValue = table.Column<int>(type: "int", nullable: false),
                    FactoryId = table.Column<int>(type: "int", nullable: false),
                    MaxCurrent = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Topology = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrimarySource", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PrimarySource_Factory_FactoryId",
                        column: x => x.FactoryId,
                        principalTable: "Factory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "secondarySource",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DesignValue = table.Column<int>(type: "int", nullable: false),
                    Fac_Id = table.Column<int>(type: "int", nullable: false),
                    MaxCurrent = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrimarySourceId = table.Column<int>(type: "int", nullable: false),
                    Topology = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_secondarySource", x => x.Id);
                    table.ForeignKey(
                        name: "FK_secondarySource_PrimarySource_PrimarySourceId",
                        column: x => x.PrimarySourceId,
                        principalTable: "PrimarySource",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Load",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Connection = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Function = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FunctionNameId = table.Column<int>(type: "int", nullable: true),
                    LoadInfoId = table.Column<int>(type: "int", nullable: true),
                    PhaseType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SourceId = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    code = table.Column<int>(type: "int", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    secondarySourceId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Load", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Load_Functions_FunctionNameId",
                        column: x => x.FunctionNameId,
                        principalTable: "Functions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Load_Loadparameter_LoadInfoId",
                        column: x => x.LoadInfoId,
                        principalTable: "Loadparameter",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Load_secondarySource_secondarySourceId",
                        column: x => x.secondarySourceId,
                        principalTable: "secondarySource",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SourceAvgs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Current1Avg = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    Current2Avg = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    Current3Avg = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    PrimarySourceId = table.Column<int>(type: "int", nullable: true),
                    SecondarySourceId = table.Column<int>(type: "int", nullable: true),
                    Time = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SourceAvgs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SourceAvgs_PrimarySource_PrimarySourceId",
                        column: x => x.PrimarySourceId,
                        principalTable: "PrimarySource",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SourceAvgs_secondarySource_SecondarySourceId",
                        column: x => x.SecondarySourceId,
                        principalTable: "secondarySource",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SourceReading",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Current1 = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    Current2 = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    Current3 = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    Fac_Id = table.Column<int>(type: "int", nullable: false),
                    HarmonicOrder = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    HarmonicOrder1 = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    HarmonicOrder2 = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    HarmonicOrder3 = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    Power1 = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    Power2 = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    Power3 = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    PowerFactor1 = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    PowerFactor2 = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    PowerFactor3 = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    PrimarySourceId = table.Column<int>(type: "int", nullable: true),
                    ReturnCurrent = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    SecondarySourceId = table.Column<int>(type: "int", nullable: true),
                    TimeStamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Voltage1 = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    Voltage2 = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    Voltage3 = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    frequency1 = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    frequency2 = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    frequency3 = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    mCurrent1 = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    mCurrent2 = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    mCurrent3 = table.Column<decimal>(type: "decimal(18, 2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SourceReading", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SourceReading_PrimarySource_PrimarySourceId",
                        column: x => x.PrimarySourceId,
                        principalTable: "PrimarySource",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SourceReading_secondarySource_SecondarySourceId",
                        column: x => x.SecondarySourceId,
                        principalTable: "secondarySource",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LoadReading",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    LoadId = table.Column<int>(type: "int", nullable: false),
                    Speed = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    Temperature = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    TimeStamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Vibration = table.Column<decimal>(type: "decimal(18, 2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoadReading", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LoadReading_Load_LoadId",
                        column: x => x.LoadId,
                        principalTable: "Load",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Wires",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Lenght = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    LoadId = table.Column<int>(type: "int", nullable: false),
                    SecondarySourceId = table.Column<int>(type: "int", nullable: true),
                    Vendor = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wires", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Wires_Load_LoadId",
                        column: x => x.LoadId,
                        principalTable: "Load",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Wires_secondarySource_SecondarySourceId",
                        column: x => x.SecondarySourceId,
                        principalTable: "secondarySource",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Load_FunctionNameId",
                table: "Load",
                column: "FunctionNameId");

            migrationBuilder.CreateIndex(
                name: "IX_Load_LoadInfoId",
                table: "Load",
                column: "LoadInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_Load_secondarySourceId",
                table: "Load",
                column: "secondarySourceId");

            migrationBuilder.CreateIndex(
                name: "IX_LoadReading_LoadId",
                table: "LoadReading",
                column: "LoadId");

            migrationBuilder.CreateIndex(
                name: "IX_PrimarySource_FactoryId",
                table: "PrimarySource",
                column: "FactoryId");

            migrationBuilder.CreateIndex(
                name: "IX_secondarySource_PrimarySourceId",
                table: "secondarySource",
                column: "PrimarySourceId");

            migrationBuilder.CreateIndex(
                name: "IX_SourceAvgs_PrimarySourceId",
                table: "SourceAvgs",
                column: "PrimarySourceId");

            migrationBuilder.CreateIndex(
                name: "IX_SourceAvgs_SecondarySourceId",
                table: "SourceAvgs",
                column: "SecondarySourceId");

            migrationBuilder.CreateIndex(
                name: "IX_SourceReading_PrimarySourceId",
                table: "SourceReading",
                column: "PrimarySourceId");

            migrationBuilder.CreateIndex(
                name: "IX_SourceReading_SecondarySourceId",
                table: "SourceReading",
                column: "SecondarySourceId");

            migrationBuilder.CreateIndex(
                name: "IX_Wires_LoadId",
                table: "Wires",
                column: "LoadId");

            migrationBuilder.CreateIndex(
                name: "IX_Wires_SecondarySourceId",
                table: "Wires",
                column: "SecondarySourceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "businessTypeFac");

            migrationBuilder.DropTable(
                name: "CurrentSpikeLogs");

            migrationBuilder.DropTable(
                name: "LoadReading");

            migrationBuilder.DropTable(
                name: "PhasesConnection");

            migrationBuilder.DropTable(
                name: "PowerAvg");

            migrationBuilder.DropTable(
                name: "powerPeak");

            migrationBuilder.DropTable(
                name: "Production");

            migrationBuilder.DropTable(
                name: "ReportConstants");

            migrationBuilder.DropTable(
                name: "SourceAvgs");

            migrationBuilder.DropTable(
                name: "SourceReading");

            migrationBuilder.DropTable(
                name: "Wires");

            migrationBuilder.DropTable(
                name: "Load");

            migrationBuilder.DropTable(
                name: "Functions");

            migrationBuilder.DropTable(
                name: "Loadparameter");

            migrationBuilder.DropTable(
                name: "secondarySource");

            migrationBuilder.DropTable(
                name: "PrimarySource");

            migrationBuilder.DropTable(
                name: "Factory");
        }
    }
}
