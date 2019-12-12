﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using SmartPower.DataContext;
using System;

namespace SmartPower.Migrations
{
    [DbContext(typeof(PowerDbContext))]
    [Migration("20191007165308_msdvsv1")]
    partial class msdvsv1
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.0-rtm-26452")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("SmartPower.Models.BusinessTypeFac", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("busnisstype");

                    b.HasKey("Id");

                    b.ToTable("businessTypeFac");
                });

            modelBuilder.Entity("SmartPower.Models.CurrentSpikeLogs", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("PhaseName");

                    b.Property<string>("SourceCode");

                    b.Property<DateTime>("Time");

                    b.Property<decimal>("Value");

                    b.HasKey("Id");

                    b.ToTable("CurrentSpikeLogs");
                });

            modelBuilder.Entity("SmartPower.Models.Factory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<string>("Scale")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<string>("businessType")
                        .HasMaxLength(255);

                    b.HasKey("Id");

                    b.ToTable("Factory");
                });

            modelBuilder.Entity("SmartPower.Models.Function", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("FunctionName");

                    b.HasKey("Id");

                    b.ToTable("Functions");
                });

            modelBuilder.Entity("SmartPower.Models.Load", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Connection");

                    b.Property<string>("Function");

                    b.Property<int?>("FunctionNameId");

                    b.Property<int?>("LoadInfoId");

                    b.Property<string>("PhaseType");

                    b.Property<int>("SourceId");

                    b.Property<string>("Type");

                    b.Property<int>("code");

                    b.Property<string>("name");

                    b.Property<int?>("secondarySourceId");

                    b.HasKey("Id");

                    b.HasIndex("FunctionNameId");

                    b.HasIndex("LoadInfoId");

                    b.HasIndex("secondarySourceId");

                    b.ToTable("Load");
                });

            modelBuilder.Entity("SmartPower.Models.Loadparameter", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<decimal>("Power");

                    b.Property<decimal>("PowerFactor");

                    b.Property<decimal>("RatingCurrent");

                    b.Property<decimal>("RatingTemp");

                    b.Property<decimal>("RatingVoltage");

                    b.Property<string>("Type");

                    b.HasKey("Id");

                    b.ToTable("Loadparameter");
                });

            modelBuilder.Entity("SmartPower.Models.LoadReading", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("LoadId");

                    b.Property<decimal>("Speed");

                    b.Property<decimal>("Temperature");

                    b.Property<DateTime>("TimeStamp");

                    b.Property<decimal>("Vibration");

                    b.HasKey("Id");

                    b.HasIndex("LoadId");

                    b.ToTable("LoadReading");
                });

            modelBuilder.Entity("SmartPower.Models.PhasesConnection", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("DestinationType");

                    b.Property<int>("SourceId");

                    b.Property<string>("SourceType");

                    b.Property<int>("dN1");

                    b.Property<int>("dN2");

                    b.Property<int>("dN3");

                    b.Property<string>("sN1");

                    b.Property<string>("sN2");

                    b.Property<string>("sN3");

                    b.HasKey("ID");

                    b.ToTable("PhasesConnection");
                });

            modelBuilder.Entity("SmartPower.Models.PrimarySource", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Code");

                    b.Property<int>("DesignValue");

                    b.Property<int>("FactoryId");

                    b.Property<int>("MaxCurrent");

                    b.Property<string>("Name");

                    b.Property<string>("Topology");

                    b.Property<string>("Type");

                    b.HasKey("Id");

                    b.HasIndex("FactoryId");

                    b.ToTable("PrimarySource");
                });

            modelBuilder.Entity("SmartPower.Models.Production", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Date");

                    b.Property<int>("FacId");

                    b.Property<double>("Quantity");

                    b.Property<string>("Type");

                    b.HasKey("Id");

                    b.ToTable("Production");
                });

            modelBuilder.Entity("SmartPower.Models.report.PowerAvg", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<decimal>("P1");

                    b.Property<decimal>("P2");

                    b.Property<decimal>("P3");

                    b.Property<int>("loadId");

                    b.Property<int?>("primarySourceId");

                    b.Property<DateTime>("readingDate");

                    b.Property<int?>("secondrySourceId");

                    b.HasKey("Id");

                    b.ToTable("PowerAvg");
                });

            modelBuilder.Entity("SmartPower.Models.report.powerPeak", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("dateP1");

                    b.Property<DateTime>("dateP2");

                    b.Property<DateTime>("dateP3");

                    b.Property<int>("loadId");

                    b.Property<decimal>("peakP1");

                    b.Property<decimal>("peakP2");

                    b.Property<decimal>("peakP3");

                    b.Property<int>("primarySourceId");

                    b.Property<int>("secondrySourceId");

                    b.HasKey("Id");

                    b.ToTable("powerPeak");
                });

            modelBuilder.Entity("SmartPower.Models.ReportConstant", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.Property<decimal>("Value");

                    b.HasKey("Id");

                    b.ToTable("ReportConstants");
                });

            modelBuilder.Entity("SmartPower.Models.secondarySource", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Code");

                    b.Property<int>("DesignValue");

                    b.Property<int>("Fac_Id");

                    b.Property<int>("MaxCurrent");

                    b.Property<string>("Name");

                    b.Property<int>("PrimarySourceId");

                    b.Property<string>("Topology");

                    b.Property<string>("Type");

                    b.HasKey("Id");

                    b.HasIndex("PrimarySourceId");

                    b.ToTable("secondarySource");
                });

            modelBuilder.Entity("SmartPower.Models.SourceAvg", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<decimal>("Current1Avg");

                    b.Property<decimal>("Current2Avg");

                    b.Property<decimal>("Current3Avg");

                    b.Property<int?>("PrimarySourceId");

                    b.Property<int?>("SecondarySourceId");

                    b.Property<DateTime>("Time");

                    b.HasKey("Id");

                    b.HasIndex("PrimarySourceId");

                    b.HasIndex("SecondarySourceId");

                    b.ToTable("SourceAvgs");
                });

            modelBuilder.Entity("SmartPower.Models.SourceReading", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<decimal>("Current1");

                    b.Property<decimal>("Current2");

                    b.Property<decimal>("Current3");

                    b.Property<int>("Fac_Id");

                    b.Property<decimal>("HarmonicOrder");

                    b.Property<decimal>("HarmonicOrder1");

                    b.Property<decimal>("HarmonicOrder2");

                    b.Property<decimal>("HarmonicOrder3");

                    b.Property<decimal>("Power1");

                    b.Property<decimal>("Power2");

                    b.Property<decimal>("Power3");

                    b.Property<decimal>("PowerFactor1");

                    b.Property<decimal>("PowerFactor2");

                    b.Property<decimal>("PowerFactor3");

                    b.Property<int?>("PrimarySourceId");

                    b.Property<decimal>("ReturnCurrent");

                    b.Property<int?>("SecondarySourceId");

                    b.Property<DateTime>("TimeStamp");

                    b.Property<decimal>("Voltage1");

                    b.Property<decimal>("Voltage2");

                    b.Property<decimal>("Voltage3");

                    b.Property<decimal>("frequency1");

                    b.Property<decimal>("frequency2");

                    b.Property<decimal>("frequency3");

                    b.Property<decimal>("mCurrent1");

                    b.Property<decimal>("mCurrent2");

                    b.Property<decimal>("mCurrent3");

                    b.HasKey("Id");

                    b.HasIndex("PrimarySourceId");

                    b.HasIndex("SecondarySourceId");

                    b.ToTable("SourceReading");
                });

            modelBuilder.Entity("SmartPower.Models.Wire", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<decimal>("Lenght");

                    b.Property<int>("LoadId");

                    b.Property<int?>("SecondarySourceId");

                    b.Property<string>("Vendor");

                    b.HasKey("Id");

                    b.HasIndex("LoadId");

                    b.HasIndex("SecondarySourceId");

                    b.ToTable("Wires");
                });

            modelBuilder.Entity("SmartPower.Models.Load", b =>
                {
                    b.HasOne("SmartPower.Models.Function", "FunctionName")
                        .WithMany()
                        .HasForeignKey("FunctionNameId");

                    b.HasOne("SmartPower.Models.Loadparameter", "LoadInfo")
                        .WithMany()
                        .HasForeignKey("LoadInfoId");

                    b.HasOne("SmartPower.Models.secondarySource")
                        .WithMany("Loads")
                        .HasForeignKey("secondarySourceId");
                });

            modelBuilder.Entity("SmartPower.Models.LoadReading", b =>
                {
                    b.HasOne("SmartPower.Models.Load")
                        .WithMany("LoadLogs")
                        .HasForeignKey("LoadId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SmartPower.Models.PrimarySource", b =>
                {
                    b.HasOne("SmartPower.Models.Factory", "Factory")
                        .WithMany("PrimarySources")
                        .HasForeignKey("FactoryId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SmartPower.Models.secondarySource", b =>
                {
                    b.HasOne("SmartPower.Models.PrimarySource", "PrimarySource")
                        .WithMany("secondarySources")
                        .HasForeignKey("PrimarySourceId")
                        .OnDelete(DeleteBehavior.Cascade);
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
                    b.HasOne("SmartPower.Models.PrimarySource", "PrimarySource")
                        .WithMany("SourceLogs")
                        .HasForeignKey("PrimarySourceId");

                    b.HasOne("SmartPower.Models.secondarySource", "SecondarySource")
                        .WithMany("SourceLogs")
                        .HasForeignKey("SecondarySourceId");
                });

            modelBuilder.Entity("SmartPower.Models.Wire", b =>
                {
                    b.HasOne("SmartPower.Models.Load", "Load")
                        .WithMany()
                        .HasForeignKey("LoadId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("SmartPower.Models.secondarySource", "SecondarySource")
                        .WithMany()
                        .HasForeignKey("SecondarySourceId");
                });
#pragma warning restore 612, 618
        }
    }
}
