﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SmartPower.DataContext;

namespace SmartPower.Migrations
{
    [DbContext(typeof(PowerDbContext))]
    [Migration("20191222220157_TypeSeeding")]
    partial class TypeSeeding
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("SmartPower.Models.BusinessTypeFac", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("busnisstype")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("businessTypeFac");
                });

            modelBuilder.Entity("SmartPower.Models.CurrentSpikeLogs", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("PhaseName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SourceCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Time")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("Value")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.ToTable("CurrentSpikeLogs");
                });

            modelBuilder.Entity("SmartPower.Models.Factory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(255)")
                        .HasMaxLength(255);

                    b.Property<string>("Scale")
                        .IsRequired()
                        .HasColumnType("nvarchar(255)")
                        .HasMaxLength(255);

                    b.Property<string>("businessType")
                        .HasColumnType("nvarchar(255)")
                        .HasMaxLength(255);

                    b.HasKey("Id");

                    b.ToTable("Factory");
                });

            modelBuilder.Entity("SmartPower.Models.Function", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("FunctionName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Functions");
                });

            modelBuilder.Entity("SmartPower.Models.Load", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Connection")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Function")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("LoadInfoId")
                        .HasColumnType("int");

                    b.Property<string>("PhaseType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SourceId")
                        .HasColumnType("int");

                    b.Property<string>("Type")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("code")
                        .HasColumnType("int");

                    b.Property<string>("name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("secondarySourceId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("LoadInfoId");

                    b.HasIndex("secondarySourceId");

                    b.ToTable("Load");
                });

            modelBuilder.Entity("SmartPower.Models.LoadReading", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("LoadId")
                        .HasColumnType("int");

                    b.Property<decimal>("Speed")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("Temperature")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("TimeStamp")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("Vibration")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("LoadId");

                    b.ToTable("LoadReading");
                });

            modelBuilder.Entity("SmartPower.Models.Loadparameter", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<decimal>("Power")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("PowerFactor")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("RatingCurrent")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("RatingTemp")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("RatingVoltage")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Type")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Loadparameter");
                });

            modelBuilder.Entity("SmartPower.Models.PhasesConnection", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("DestinationType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SourceId")
                        .HasColumnType("int");

                    b.Property<string>("SourceType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("dN1")
                        .HasColumnType("int");

                    b.Property<int>("dN2")
                        .HasColumnType("int");

                    b.Property<int>("dN3")
                        .HasColumnType("int");

                    b.Property<string>("sN1")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("sN2")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("sN3")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("PhasesConnection");
                });

            modelBuilder.Entity("SmartPower.Models.PowAvg", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("date")
                        .HasColumnType("datetime2");

                    b.Property<int>("loadId")
                        .HasColumnType("int");

                    b.Property<decimal>("pAvg1")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("pAvg2")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("pAvg3")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.ToTable("powAvg");
                });

            modelBuilder.Entity("SmartPower.Models.PrimarySource", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Code")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("DesignValue")
                        .HasColumnType("int");

                    b.Property<int>("FactoryId")
                        .HasColumnType("int");

                    b.Property<int>("MaxCurrent")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("SourceTypeTypeId")
                        .HasColumnType("int");

                    b.Property<string>("Topology")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Type")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("FactoryId");

                    b.HasIndex("SourceTypeTypeId");

                    b.ToTable("PrimarySource");
                });

            modelBuilder.Entity("SmartPower.Models.Production", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<int>("FacId")
                        .HasColumnType("int");

                    b.Property<double>("Quantity")
                        .HasColumnType("float");

                    b.Property<string>("Type")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Production");
                });

            modelBuilder.Entity("SmartPower.Models.ReportConstant", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Value")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.ToTable("ReportConstants");
                });

            modelBuilder.Entity("SmartPower.Models.SourceAvg", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<decimal>("Current1Avg")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("Current2Avg")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("Current3Avg")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int?>("PrimarySourceId")
                        .HasColumnType("int");

                    b.Property<int?>("SecondarySourceId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Time")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("PrimarySourceId");

                    b.HasIndex("SecondarySourceId");

                    b.ToTable("SourceAvgs");
                });

            modelBuilder.Entity("SmartPower.Models.SourceReading", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<decimal>("Current1")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("Current2")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("Current3")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("Fac_Id")
                        .HasColumnType("int");

                    b.Property<decimal>("HarmonicOrder")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("HarmonicOrder1")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("HarmonicOrder2")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("HarmonicOrder3")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("Power1")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("Power2")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("Power3")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("PowerFactor1")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("PowerFactor2")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("PowerFactor3")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int?>("PrimarySourceId")
                        .HasColumnType("int");

                    b.Property<decimal>("ReturnCurrent")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int?>("SecondarySourceId")
                        .HasColumnType("int");

                    b.Property<DateTime>("TimeStamp")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("Voltage1")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("Voltage2")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("Voltage3")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("frequency1")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("frequency2")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("frequency3")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("mCurrent1")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("mCurrent2")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("mCurrent3")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("PrimarySourceId");

                    b.HasIndex("SecondarySourceId");

                    b.ToTable("SourceReading");
                });

            modelBuilder.Entity("SmartPower.Models.SourceType", b =>
                {
                    b.Property<int>("TypeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("TypeName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("TypeId");

                    b.ToTable("SourceType");

                    b.HasData(
                        new
                        {
                            TypeId = 1,
                            TypeName = "Transformer"
                        },
                        new
                        {
                            TypeId = 2,
                            TypeName = "Machine"
                        },
                        new
                        {
                            TypeId = 3,
                            TypeName = "SubMachine"
                        });
                });

            modelBuilder.Entity("SmartPower.Models.Wire", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<decimal>("Lenght")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("LoadId")
                        .HasColumnType("int");

                    b.Property<int?>("SecondarySourceId")
                        .HasColumnType("int");

                    b.Property<string>("Vendor")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("LoadId");

                    b.HasIndex("SecondarySourceId");

                    b.ToTable("Wires");
                });

            modelBuilder.Entity("SmartPower.Models.report.PowerAvg", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<decimal>("P1")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("P2")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("P3")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("loadId")
                        .HasColumnType("int");

                    b.Property<int?>("primarySourceId")
                        .HasColumnType("int");

                    b.Property<DateTime>("readingDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("secondrySourceId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("PowerAvg");
                });

            modelBuilder.Entity("SmartPower.Models.report.powerPeak", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("dateP1")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("dateP2")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("dateP3")
                        .HasColumnType("datetime2");

                    b.Property<int>("loadId")
                        .HasColumnType("int");

                    b.Property<decimal>("peakP1")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("peakP2")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("peakP3")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("primarySourceId")
                        .HasColumnType("int");

                    b.Property<int>("secondrySourceId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("powerPeak");
                });

            modelBuilder.Entity("SmartPower.Models.secondarySource", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Code")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("DesignValue")
                        .HasColumnType("int");

                    b.Property<int>("Fac_Id")
                        .HasColumnType("int");

                    b.Property<int>("MaxCurrent")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PrimarySourceId")
                        .HasColumnType("int");

                    b.Property<string>("Topology")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Type")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("PrimarySourceId");

                    b.ToTable("secondarySource");
                });

            modelBuilder.Entity("SmartPower.Models.Load", b =>
                {
                    b.HasOne("SmartPower.Models.Loadparameter", "LoadInfo")
                        .WithMany()
                        .HasForeignKey("LoadInfoId");

                    b.HasOne("SmartPower.Models.secondarySource", null)
                        .WithMany("Loads")
                        .HasForeignKey("secondarySourceId");
                });

            modelBuilder.Entity("SmartPower.Models.LoadReading", b =>
                {
                    b.HasOne("SmartPower.Models.Load", null)
                        .WithMany("LoadLogs")
                        .HasForeignKey("LoadId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SmartPower.Models.PrimarySource", b =>
                {
                    b.HasOne("SmartPower.Models.Factory", "Factory")
                        .WithMany("PrimarySources")
                        .HasForeignKey("FactoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SmartPower.Models.SourceType", "SourceType")
                        .WithMany()
                        .HasForeignKey("SourceTypeTypeId");
                });

            modelBuilder.Entity("SmartPower.Models.SourceAvg", b =>
                {
                    b.HasOne("SmartPower.Models.PrimarySource", "PrimarySource")
                        .WithMany()
                        .HasForeignKey("PrimarySourceId");

                    b.HasOne("SmartPower.Models.secondarySource", "SecondarySource")
                        .WithMany()
                        .HasForeignKey("SecondarySourceId");
                });

            modelBuilder.Entity("SmartPower.Models.SourceReading", b =>
                {
                    b.HasOne("SmartPower.Models.PrimarySource", null)
                        .WithMany("SourceLogs")
                        .HasForeignKey("PrimarySourceId");

                    b.HasOne("SmartPower.Models.secondarySource", null)
                        .WithMany("SourceLogs")
                        .HasForeignKey("SecondarySourceId");
                });

            modelBuilder.Entity("SmartPower.Models.Wire", b =>
                {
                    b.HasOne("SmartPower.Models.Load", "Load")
                        .WithMany()
                        .HasForeignKey("LoadId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SmartPower.Models.secondarySource", "SecondarySource")
                        .WithMany()
                        .HasForeignKey("SecondarySourceId");
                });

            modelBuilder.Entity("SmartPower.Models.secondarySource", b =>
                {
                    b.HasOne("SmartPower.Models.PrimarySource", "PrimarySource")
                        .WithMany("secondarySources")
                        .HasForeignKey("PrimarySourceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}