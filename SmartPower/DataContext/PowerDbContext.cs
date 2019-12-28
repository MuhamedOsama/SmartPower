using Microsoft.EntityFrameworkCore;
using SmartPower.Models;
using SmartPower.Models.report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace SmartPower.DataContext
{
    public class PowerDbContext:DbContext
    {
        public DbSet<Factory> Factory { get; set; }
        public DbSet<SourceReading> SourceReading { get; set; }
        public DbSet<PrimarySource> PrimarySource { get; set; }
        public DbSet<secondarySource> secondarySource { get; set; }
        public DbSet<ReportConstant> ReportConstants { get; set; }
        public DbSet<CurrentSpikeLogs> CurrentSpikeLogs { get; set; }
        public DbSet<SourceAvg> SourceAvgs { get; set; }
        public DbSet<Function> Functions { get; set; }

        public DbSet<PowAvg> powAvg { get; set; }
        public DbSet<Load> Load { get; set; }
        public DbSet<LoadReading> LoadReading { get; set; }
        public DbSet<Loadparameter> Loadparameter { get; set; }
        public DbSet<Wire> Wires { get; set; }

        public DbSet<powerPeak> powerPeak { get; set; }
        public DbSet<PowerAvg> PowerAvg { get; set; }
        public DbSet<Production> Production { get; set; }
        public DbSet<SourceType> sourceType { get; set; }
       
        public DbSet<PhasesConnection> PhasesConnection { get; set; }
        public DbSet<BusinessTypeFac> businessTypeFac { get; set; }
        public DbSet<SourceRelation> SourceRelations { get; set; }
        public PowerDbContext(DbContextOptions<PowerDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SourceType>().HasData(
                new SourceType { Id = 1, TypeName = "Transformer" },
                new SourceType { Id = 2, TypeName = "Machine" },
                new SourceType { Id = 3, TypeName = "SubMachine" }
                );
        }
    }
}
