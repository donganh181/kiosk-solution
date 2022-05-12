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

        public virtual DbSet<TblAppCategory> TblAppCategories { get; set; }
        public virtual DbSet<TblApplicationMarket> TblApplicationMarkets { get; set; }
        public virtual DbSet<TblEvent> TblEvents { get; set; }
        public virtual DbSet<TblImage> TblImages { get; set; }
        public virtual DbSet<TblKiosk> TblKiosks { get; set; }
        public virtual DbSet<TblKioskLocation> TblKioskLocations { get; set; }
        public virtual DbSet<TblOrder> TblOrders { get; set; }
        public virtual DbSet<TblParty> TblParties { get; set; }
        public virtual DbSet<TblPartyKioskLocation> TblPartyKioskLocations { get; set; }
        public virtual DbSet<TblPartyServiceApplication> TblPartyServiceApplications { get; set; }
        public virtual DbSet<TblPoi> TblPois { get; set; }
        public virtual DbSet<TblPosition> TblPositions { get; set; }
        public virtual DbSet<TblRole> TblRoles { get; set; }
        public virtual DbSet<TblSchedule> TblSchedules { get; set; }
        public virtual DbSet<TblScheduleTemplate> TblScheduleTemplates { get; set; }
        public virtual DbSet<TblServiceApplication> TblServiceApplications { get; set; }
        public virtual DbSet<TblTemplate> TblTemplates { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

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
                    .WithMany(p => p.TblEvents)
                    .HasForeignKey(d => d.CreatorId)
                    .HasConstraintName("FK__tblEvent__Creato__628FA481");

                entity.HasOne(d => d.Template)
                    .WithMany(p => p.TblEvents)
                    .HasForeignKey(d => d.TemplateId)
                    .HasConstraintName("FK__tblEvent__Status__619B8048");
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

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.Name).HasMaxLength(255);

                entity.Property(e => e.Status)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.KioskLocation)
                    .WithMany(p => p.TblKiosks)
                    .HasForeignKey(d => d.KioskLocationId)
                    .HasConstraintName("FK__tblKiosk__KioskL__440B1D61");

                entity.HasOne(d => d.Party)
                    .WithMany(p => p.TblKiosks)
                    .HasForeignKey(d => d.PartyId)
                    .HasConstraintName("FK__tblKiosk__PartyI__44FF419A");
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
                    .HasConstraintName("FK__tblOrder__KioskI__7D439ABD");

                entity.HasOne(d => d.Party)
                    .WithMany(p => p.TblOrders)
                    .HasForeignKey(d => d.PartyId)
                    .HasConstraintName("FK__tblOrder__PartyI__7C4F7684");

                entity.HasOne(d => d.ServiceApplication)
                    .WithMany(p => p.TblOrders)
                    .HasForeignKey(d => d.ServiceApplicationId)
                    .HasConstraintName("FK__tblOrder__Servic__7B5B524B");
            });

            modelBuilder.Entity<TblParty>(entity =>
            {
                entity.ToTable("tblParty");

                entity.HasIndex(e => e.PhoneNumber, "UQ__tblParty__85FB4E38163B70EC")
                    .IsUnique();

                entity.HasIndex(e => e.Email, "UQ__tblParty__A9D1053412552AB1")
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
                    .HasForeignKey(d => d.CreatorId)
                    .HasConstraintName("FK__tblParty__Creato__3C69FB99");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.TblParties)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK__tblParty__RoleId__3D5E1FD2");
            });

            modelBuilder.Entity<TblPartyKioskLocation>(entity =>
            {
                entity.ToTable("tblParty_KioskLocation");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.HasOne(d => d.KioskLocation)
                    .WithMany(p => p.TblPartyKioskLocations)
                    .HasForeignKey(d => d.KioskLocationId)
                    .HasConstraintName("FK__tblParty___Kiosk__6B24EA82");

                entity.HasOne(d => d.Party)
                    .WithMany(p => p.TblPartyKioskLocations)
                    .HasForeignKey(d => d.PartyId)
                    .HasConstraintName("FK__tblParty___Party__6A30C649");
            });

            modelBuilder.Entity<TblPartyServiceApplication>(entity =>
            {
                entity.ToTable("tblParty_ServiceApplication");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.HasOne(d => d.Party)
                    .WithMany(p => p.TblPartyServiceApplications)
                    .HasForeignKey(d => d.PartyId)
                    .HasConstraintName("FK__tblParty___Party__6EF57B66");

                entity.HasOne(d => d.ServiceApplication)
                    .WithMany(p => p.TblPartyServiceApplications)
                    .HasForeignKey(d => d.ServiceApplicationId)
                    .HasConstraintName("FK__tblParty___Servi__6FE99F9F");
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

                entity.HasOne(d => d.Creator)
                    .WithMany(p => p.TblPois)
                    .HasForeignKey(d => d.CreatorId)
                    .HasConstraintName("FK__tblPOI__CreatorI__66603565");
            });

            modelBuilder.Entity<TblPosition>(entity =>
            {
                entity.ToTable("tblPosition");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.Status)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.ServiceApplication)
                    .WithMany(p => p.TblPositions)
                    .HasForeignKey(d => d.ServiceApplicationId)
                    .HasConstraintName("FK__tblPositi__Servi__5CD6CB2B");

                entity.HasOne(d => d.Template)
                    .WithMany(p => p.TblPositions)
                    .HasForeignKey(d => d.TemplateId)
                    .HasConstraintName("FK__tblPositi__Templ__5DCAEF64");
            });

            modelBuilder.Entity<TblRole>(entity =>
            {
                entity.ToTable("tblRole");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Name).HasMaxLength(255);
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
                    .HasConstraintName("FK__tblSchedu__Kiosk__5441852A");

                entity.HasOne(d => d.Party)
                    .WithMany(p => p.TblSchedules)
                    .HasForeignKey(d => d.PartyId)
                    .HasConstraintName("FK__tblSchedu__Party__5535A963");
            });

            modelBuilder.Entity<TblScheduleTemplate>(entity =>
            {
                entity.ToTable("tblSchedule_Template");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.HasOne(d => d.Schedule)
                    .WithMany(p => p.TblScheduleTemplates)
                    .HasForeignKey(d => d.ScheduleId)
                    .HasConstraintName("FK__tblSchedu__Sched__73BA3083");

                entity.HasOne(d => d.Template)
                    .WithMany(p => p.TblScheduleTemplates)
                    .HasForeignKey(d => d.TemplateId)
                    .HasConstraintName("FK__tblSchedu__Templ__74AE54BC");
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
                    .HasConstraintName("FK__tblServic__AppCa__4E88ABD4");

                entity.HasOne(d => d.ApplicationMarket)
                    .WithMany(p => p.TblServiceApplications)
                    .HasForeignKey(d => d.ApplicationMarketId)
                    .HasConstraintName("FK__tblServic__Appli__5070F446");

                entity.HasOne(d => d.Party)
                    .WithMany(p => p.TblServiceApplications)
                    .HasForeignKey(d => d.PartyId)
                    .HasConstraintName("FK__tblServic__Party__4F7CD00D");
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

                entity.HasOne(d => d.Party)
                    .WithMany(p => p.TblTemplates)
                    .HasForeignKey(d => d.PartyId)
                    .HasConstraintName("FK__tblTempla__Party__59063A47");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
