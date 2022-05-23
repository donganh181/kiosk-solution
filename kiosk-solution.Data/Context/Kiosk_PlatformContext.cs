using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace kiosk_solution.Data.Models
{
    public partial class Kiosk_PlatformContext : DbContext
    {
        public Kiosk_PlatformContext()
        {
        }

        public Kiosk_PlatformContext(DbContextOptions<Kiosk_PlatformContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AppCategory> AppCategories { get; set; }
        public virtual DbSet<ApplicationMarket> ApplicationMarkets { get; set; }
        public virtual DbSet<Event> Events { get; set; }
        public virtual DbSet<Image> Images { get; set; }
        public virtual DbSet<Kiosk> Kiosks { get; set; }
        public virtual DbSet<KioskLocation> KioskLocations { get; set; }
        public virtual DbSet<KioskSchedule> KioskSchedules { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Party> Parties { get; set; }
        public virtual DbSet<PartyKioskLocation> PartyKioskLocations { get; set; }
        public virtual DbSet<PartyServiceApplication> PartyServiceApplications { get; set; }
        public virtual DbSet<Poi> Pois { get; set; }
        public virtual DbSet<Position> Positions { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Schedule> Schedules { get; set; }
        public virtual DbSet<ScheduleTemplate> ScheduleTemplates { get; set; }
        public virtual DbSet<ServiceApplication> ServiceApplications { get; set; }
        public virtual DbSet<Template> Templates { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<AppCategory>(entity =>
            {
                entity.ToTable("tblAppCategory");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Name).HasMaxLength(255);
            });

            modelBuilder.Entity<ApplicationMarket>(entity =>
            {
                entity.ToTable("tblApplicationMarket");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Logo).IsUnicode(false);

                entity.Property(e => e.Name).HasMaxLength(255);

                entity.Property(e => e.Status)
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Event>(entity =>
            {
                entity.ToTable("tblEvent");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Name).HasMaxLength(255);

                entity.Property(e => e.Status)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.TimeEnd).HasColumnType("datetime");

                entity.Property(e => e.TimeStart).HasColumnType("datetime");

                entity.Property(e => e.Type)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.Creator)
                    .WithMany(p => p.Events)
                    .HasForeignKey(d => d.CreatorId);

                entity.HasOne(d => d.Template)
                    .WithMany(p => p.Events)
                    .HasForeignKey(d => d.TemplateId);
            });

            modelBuilder.Entity<Image>(entity =>
            {
                entity.ToTable("tblImage");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.KeyType)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Link).IsUnicode(false);
            });

            modelBuilder.Entity<Kiosk>(entity =>
            {
                entity.ToTable("tblKiosk");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.Name).HasMaxLength(255);

                entity.Property(e => e.Status)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Latitude)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Longtitude)
                    .HasMaxLength(100)
                    .IsUnicode(false);
                entity.HasOne(d => d.KioskLocation)
                    .WithMany(p => p.Kiosks)
                    .HasForeignKey(d => d.KioskLocationId);

                entity.HasOne(d => d.Party)
                    .WithMany(p => p.Kiosks)
                    .HasForeignKey(d => d.PartyId);
            });

            modelBuilder.Entity<KioskLocation>(entity =>
            {
                entity.ToTable("tblKioskLocation");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.City).HasMaxLength(100);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.District).HasMaxLength(100);

                entity.Property(e => e.Name).HasMaxLength(255);

                entity.Property(e => e.Province).HasMaxLength(100);

                entity.Property(e => e.Status)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Street).HasMaxLength(100);

                entity.Property(e => e.Ward).HasMaxLength(100);
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("tblOrder");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.Income).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.OrderDetail).IsUnicode(false);

                entity.Property(e => e.Status)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.SubmitDate).HasColumnType("datetime");

                entity.HasOne(d => d.Kiosk)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.KioskId)
                    .HasConstraintName("FK__tblOrder__KioskI__7D439ABD");

                entity.HasOne(d => d.Party)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.PartyId);

                entity.HasOne(d => d.ServiceApplication)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.ServiceApplicationId);
            });

            modelBuilder.Entity<Party>(entity =>
            {
                entity.ToTable("tblParty");

                entity.HasIndex(e => e.PhoneNumber)
                    .IsUnique();

                entity.HasIndex(e => e.Email)
                    .IsUnique();

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.DateOfBirth).HasColumnType("date");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName).HasMaxLength(255);

                entity.Property(e => e.LastName).HasMaxLength(255);

                entity.Property(e => e.Password)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Status)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.Creator)
                    .WithMany(p => p.InverseCreator)
                    .HasForeignKey(d => d.CreatorId);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Parties)
                    .HasForeignKey(d => d.RoleId);
            });

            modelBuilder.Entity<PartyKioskLocation>(entity =>
            {
                entity.ToTable("tblParty_KioskLocation");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.HasOne(d => d.KioskLocation)
                    .WithMany(p => p.PartyKioskLocations)
                    .HasForeignKey(d => d.KioskLocationId);

                entity.HasOne(d => d.Party)
                    .WithMany(p => p.PartyKioskLocations)
                    .HasForeignKey(d => d.PartyId);
            });

            modelBuilder.Entity<PartyServiceApplication>(entity =>
            {
                entity.ToTable("tblParty_ServiceApplication");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.HasOne(d => d.Party)
                    .WithMany(p => p.PartyServiceApplications)
                    .HasForeignKey(d => d.PartyId);

                entity.HasOne(d => d.ServiceApplication)
                    .WithMany(p => p.PartyServiceApplications)
                    .HasForeignKey(d => d.ServiceApplicationId);
            });

            modelBuilder.Entity<Poi>(entity =>
            {
                entity.ToTable("tblPOI");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.City).HasMaxLength(50);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.DayOfWeek)
                    .HasMaxLength(12)
                    .IsUnicode(false);

                entity.Property(e => e.District).HasMaxLength(50);

                entity.Property(e => e.Latitude)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Longtitude)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Name).HasMaxLength(255);

                entity.Property(e => e.OpenTime).HasColumnType("datetime");

                entity.Property(e => e.Province).HasMaxLength(50);

                entity.Property(e => e.Status)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Street).HasMaxLength(50);

                entity.Property(e => e.Ward).HasMaxLength(50);

                entity.HasOne(d => d.Creator)
                    .WithMany(p => p.Pois)
                    .HasForeignKey(d => d.CreatorId);
            });

            modelBuilder.Entity<Position>(entity =>
            {
                entity.ToTable("tblPosition");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.Status)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.ServiceApplication)
                    .WithMany(p => p.Positions)
                    .HasForeignKey(d => d.ServiceApplicationId);

                entity.HasOne(d => d.Template)
                    .WithMany(p => p.Positions)
                    .HasForeignKey(d => d.TemplateId);
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("tblRole");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Name).HasMaxLength(255);
            });

            modelBuilder.Entity<Schedule>(entity =>
            {
                entity.ToTable("tblSchedule");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.DayOfWeek)
                    .HasMaxLength(12)
                    .IsUnicode(false);

                entity.Property(e => e.Name).HasMaxLength(255);

                entity.Property(e => e.Status)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.TimeEnd).HasColumnType("datetime");

                entity.Property(e => e.TimeStart).HasColumnType("datetime");

                entity.HasOne(d => d.Party)
                    .WithMany(p => p.Schedules)
                    .HasForeignKey(d => d.PartyId);

            });

            modelBuilder.Entity<ScheduleTemplate>(entity =>
            {
                entity.ToTable("tblSchedule_Template");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.HasOne(d => d.Schedule)
                    .WithMany(p => p.ScheduleTemplates)
                    .HasForeignKey(d => d.ScheduleId);

                entity.HasOne(d => d.Template)
                    .WithMany(p => p.ScheduleTemplates)
                    .HasForeignKey(d => d.TemplateId);
            });

            modelBuilder.Entity<ServiceApplication>(entity =>
            {
                entity.ToTable("tblServiceApplication");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Link).IsUnicode(false);

                entity.Property(e => e.Logo).IsUnicode(false);

                entity.Property(e => e.Name).HasMaxLength(255);

                entity.Property(e => e.Status)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.AppCategory)
                    .WithMany(p => p.ServiceApplications)
                    .HasForeignKey(d => d.AppCategoryId);

                entity.HasOne(d => d.ApplicationMarket)
                    .WithMany(p => p.ServiceApplications)
                    .HasForeignKey(d => d.ApplicationMarketId);

                entity.HasOne(d => d.Party)
                    .WithMany(p => p.ServiceApplications)
                    .HasForeignKey(d => d.PartyId);
            });

            modelBuilder.Entity<Template>(entity =>
            {
                entity.ToTable("tblTemplate");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.Name).HasMaxLength(255);

                entity.Property(e => e.Status)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.Party)
                    .WithMany(p => p.Templates)
                    .HasForeignKey(d => d.PartyId);
            });
            modelBuilder.Entity<KioskSchedule>(entity =>
            {
                entity.ToTable("tblKiosk_Schedule");
                
                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
                
                entity.Property(e => e.KioskId);
                
                entity.Property(e => e.ScheduleId);
                
                entity.HasOne(d => d.Kiosk)
                    .WithMany(p => p.KioskSchedules)
                    .HasForeignKey(d => d.KioskId);
                
                entity.HasOne(d => d.Schedule)
                    .WithMany(p => p.KioskSchedules)
                    .HasForeignKey(d => d.ScheduleId);
            });
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}