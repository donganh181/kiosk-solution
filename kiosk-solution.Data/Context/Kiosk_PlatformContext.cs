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

        public virtual DbSet<TblAdmin> TblAdmins { get; set; }
        public virtual DbSet<TblAppCategory> TblAppCategories { get; set; }
        public virtual DbSet<TblApplicationMarket> TblApplicationMarkets { get; set; }
        public virtual DbSet<TblEvent> TblEvents { get; set; }
        public virtual DbSet<TblImage> TblImages { get; set; }
        public virtual DbSet<TblKiosk> TblKiosks { get; set; }
        public virtual DbSet<TblKioskLocation> TblKioskLocations { get; set; }
        public virtual DbSet<TblLocationOwner> TblLocationOwners { get; set; }
        public virtual DbSet<TblLocationOwnerKioskLocation> TblLocationOwnerKioskLocations { get; set; }
        public virtual DbSet<TblLocationOwnerServiceApplication> TblLocationOwnerServiceApplications { get; set; }
        public virtual DbSet<TblOrder> TblOrders { get; set; }
        public virtual DbSet<TblPoi> TblPois { get; set; }
        public virtual DbSet<TblPosition> TblPositions { get; set; }
        public virtual DbSet<TblSchedule> TblSchedules { get; set; }
        public virtual DbSet<TblScheduleTemplate> TblScheduleTemplates { get; set; }
        public virtual DbSet<TblServiceApplication> TblServiceApplications { get; set; }
        public virtual DbSet<TblServiceProvider> TblServiceProviders { get; set; }
        public virtual DbSet<TblTemplate> TblTemplates { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<TblAdmin>(entity =>
            {
                entity.ToTable("tblAdmin");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.DateOfBirth).HasColumnType("date");

                entity.Property(e => e.Email)
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
            });

            modelBuilder.Entity<TblAppCategory>(entity =>
            {
                entity.ToTable("tblAppCategory");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Name).HasMaxLength(255);
            });

            modelBuilder.Entity<TblApplicationMarket>(entity =>
            {
                entity.ToTable("tblApplicationMarket");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Logo).IsUnicode(false);

                entity.Property(e => e.Name).HasMaxLength(255);

                entity.Property(e => e.Status)
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblEvent>(entity =>
            {
                entity.ToTable("tblEvent");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.CreatorType)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Name).HasMaxLength(255);

                entity.Property(e => e.Status)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.TemplateId)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.TimeEnd).HasColumnType("datetime");

                entity.Property(e => e.TimeStart).HasColumnType("datetime");

                entity.Property(e => e.Type)
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblImage>(entity =>
            {
                entity.ToTable("tblImage");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.KeyType)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Link).IsUnicode(false);
            });

            modelBuilder.Entity<TblKiosk>(entity =>
            {
                entity.ToTable("tblKiosk");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.CreatDate).HasColumnType("datetime");

                entity.Property(e => e.Name).HasMaxLength(255);

                entity.Property(e => e.Status)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.KioskLocation)
                    .WithMany(p => p.TblKiosks)
                    .HasForeignKey(d => d.KioskLocationId)
                    .HasConstraintName("FK__tblKiosk__KioskL__3D5E1FD2");

                entity.HasOne(d => d.LocationOwner)
                    .WithMany(p => p.TblKiosks)
                    .HasForeignKey(d => d.LocationOwnerId)
                    .HasConstraintName("FK__tblKiosk__Locati__3E52440B");
            });

            modelBuilder.Entity<TblKioskLocation>(entity =>
            {
                entity.ToTable("tblKioskLocation");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.City).HasMaxLength(100);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.District).HasMaxLength(100);

                entity.Property(e => e.Latitude)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Longtitude)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Name).HasMaxLength(255);

                entity.Property(e => e.Province).HasMaxLength(100);

                entity.Property(e => e.Status)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Street).HasMaxLength(100);

                entity.Property(e => e.Ward).HasMaxLength(100);
            });

            modelBuilder.Entity<TblLocationOwner>(entity =>
            {
                entity.ToTable("tblLocationOwner");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.CreatetorType)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CreatorId)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.DateOfBirth).HasColumnType("date");

                entity.Property(e => e.Email)
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
            });

            modelBuilder.Entity<TblLocationOwnerKioskLocation>(entity =>
            {
                entity.ToTable("tblLocationOwner_KioskLocation");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.HasOne(d => d.KioskLocation)
                    .WithMany(p => p.TblLocationOwnerKioskLocations)
                    .HasForeignKey(d => d.KioskLocationId)
                    .HasConstraintName("FK__tblLocati__Kiosk__6754599E");

                entity.HasOne(d => d.LocationOwner)
                    .WithMany(p => p.TblLocationOwnerKioskLocations)
                    .HasForeignKey(d => d.LocationOwnerId)
                    .HasConstraintName("FK__tblLocati__Locat__66603565");
            });

            modelBuilder.Entity<TblLocationOwnerServiceApplication>(entity =>
            {
                entity.ToTable("tblLocationOwner_ServiceApplication");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.HasOne(d => d.LocationOwner)
                    .WithMany(p => p.TblLocationOwnerServiceApplications)
                    .HasForeignKey(d => d.LocationOwnerId)
                    .HasConstraintName("FK__tblLocati__Locat__6B24EA82");

                entity.HasOne(d => d.ServiceApplication)
                    .WithMany(p => p.TblLocationOwnerServiceApplications)
                    .HasForeignKey(d => d.ServiceApplicationId)
                    .HasConstraintName("FK__tblLocati__Servi__6C190EBB");
            });

            modelBuilder.Entity<TblOrder>(entity =>
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
                    .WithMany(p => p.TblOrders)
                    .HasForeignKey(d => d.KioskId)
                    .HasConstraintName("FK__tblOrder__KioskI__797309D9");

                entity.HasOne(d => d.ServiceApplication)
                    .WithMany(p => p.TblOrders)
                    .HasForeignKey(d => d.ServiceApplicationId)
                    .HasConstraintName("FK__tblOrder__Servic__778AC167");

                entity.HasOne(d => d.ServiceProvider)
                    .WithMany(p => p.TblOrders)
                    .HasForeignKey(d => d.ServiceProviderId)
                    .HasConstraintName("FK__tblOrder__Servic__787EE5A0");
            });

            modelBuilder.Entity<TblPoi>(entity =>
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
            });

            modelBuilder.Entity<TblPosition>(entity =>
            {
                entity.ToTable("tblPosition");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.Status)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.LocationOwner)
                    .WithMany(p => p.TblPositions)
                    .HasForeignKey(d => d.LocationOwnerId)
                    .HasConstraintName("FK__tblPositi__Locat__4AB81AF0");

                entity.HasOne(d => d.Template)
                    .WithMany(p => p.TblPositions)
                    .HasForeignKey(d => d.TemplateId)
                    .HasConstraintName("FK__tblPositi__Templ__4BAC3F29");
            });

            modelBuilder.Entity<TblSchedule>(entity =>
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

                entity.HasOne(d => d.Kiosk)
                    .WithMany(p => p.TblSchedules)
                    .HasForeignKey(d => d.KioskId)
                    .HasConstraintName("FK__tblSchedu__Kiosk__4222D4EF");

                entity.HasOne(d => d.LocationOwner)
                    .WithMany(p => p.TblSchedules)
                    .HasForeignKey(d => d.LocationOwnerId)
                    .HasConstraintName("FK__tblSchedu__Locat__4316F928");
            });

            modelBuilder.Entity<TblScheduleTemplate>(entity =>
            {
                entity.ToTable("tblSchedule_Template");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.HasOne(d => d.Schedule)
                    .WithMany(p => p.TblScheduleTemplates)
                    .HasForeignKey(d => d.ScheduleId)
                    .HasConstraintName("FK__tblSchedu__Sched__6FE99F9F");

                entity.HasOne(d => d.Template)
                    .WithMany(p => p.TblScheduleTemplates)
                    .HasForeignKey(d => d.TemplateId)
                    .HasConstraintName("FK__tblSchedu__Templ__70DDC3D8");
            });

            modelBuilder.Entity<TblServiceApplication>(entity =>
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
                    .WithMany(p => p.TblServiceApplications)
                    .HasForeignKey(d => d.AppCategoryId)
                    .HasConstraintName("FK__tblServic__AppCa__5DCAEF64");

                entity.HasOne(d => d.ApplicationMarket)
                    .WithMany(p => p.TblServiceApplications)
                    .HasForeignKey(d => d.ApplicationMarketId)
                    .HasConstraintName("FK__tblServic__Appli__5FB337D6");

                entity.HasOne(d => d.ServiceProvider)
                    .WithMany(p => p.TblServiceApplications)
                    .HasForeignKey(d => d.ServiceProviderId)
                    .HasConstraintName("FK__tblServic__Servi__5EBF139D");
            });

            modelBuilder.Entity<TblServiceProvider>(entity =>
            {
                entity.ToTable("tblServiceProvider");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.CreatorType)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.DateOfBirth).HasColumnType("date");

                entity.Property(e => e.Email)
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
            });

            modelBuilder.Entity<TblTemplate>(entity =>
            {
                entity.ToTable("tblTemplate");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.Name).HasMaxLength(255);

                entity.Property(e => e.Status)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.LocationOwner)
                    .WithMany(p => p.TblTemplates)
                    .HasForeignKey(d => d.LocationOwnerId)
                    .HasConstraintName("FK__tblTempla__Locat__46E78A0C");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
