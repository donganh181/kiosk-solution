using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using kiosk_solution.Data.Models;

#nullable disable

namespace kiosk_solution.Data.Context
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
        public virtual DbSet<AppCategoryPosition> AppCategoryPositions { get; set; }
        public virtual DbSet<ApplicationMarket> ApplicationMarkets { get; set; }
        public virtual DbSet<Event> Events { get; set; }
        public virtual DbSet<EventPosition> EventPositions { get; set; }
        public virtual DbSet<Image> Images { get; set; }
        public virtual DbSet<Kiosk> Kiosks { get; set; }
        public virtual DbSet<KioskLocation> KioskLocations { get; set; }
        public virtual DbSet<KioskSchedule> KioskSchedules { get; set; }
        public virtual DbSet<Notification> Notifications { get; set; }
        public virtual DbSet<Party> Parties { get; set; }
        public virtual DbSet<PartyNotification> PartyNotifications { get; set; }
        public virtual DbSet<PartyServiceApplication> PartyServiceApplications { get; set; }
        public virtual DbSet<Poi> Pois { get; set; }
        public virtual DbSet<Poicategory> Poicategories { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Schedule> Schedules { get; set; }
        public virtual DbSet<ScheduleTemplate> ScheduleTemplates { get; set; }
        public virtual DbSet<ServiceApplication> ServiceApplications { get; set; }
        public virtual DbSet<ServiceApplicationPublishRequest> ServiceApplicationPublishRequests { get; set; }
        public virtual DbSet<ServiceOrder> ServiceOrders { get; set; }
        public virtual DbSet<Template> Templates { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<AppCategory>(entity =>
            {
                entity.ToTable("AppCategory");

                entity.HasIndex(e => e.Name, "UQ__AppCateg__737584F69F3613E8")
                    .IsUnique();

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Logo).IsUnicode(false);

                entity.Property(e => e.Name).HasMaxLength(255);
            });

            modelBuilder.Entity<AppCategoryPosition>(entity =>
            {
                entity.ToTable("AppCategoryPosition");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.HasOne(d => d.AppCategory)
                    .WithMany(p => p.AppCategoryPositions)
                    .HasForeignKey(d => d.AppCategoryId)
                    .HasConstraintName("FK__AppCatego__AppCa__5535A963");

                entity.HasOne(d => d.Template)
                    .WithMany(p => p.AppCategoryPositions)
                    .HasForeignKey(d => d.TemplateId)
                    .HasConstraintName("FK__AppCatego__Templ__5629CD9C");
            });

            modelBuilder.Entity<ApplicationMarket>(entity =>
            {
                entity.ToTable("ApplicationMarket");

                entity.HasIndex(e => e.Name, "UQ__Applicat__737584F6A435BF70")
                    .IsUnique();

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Logo).IsUnicode(false);

                entity.Property(e => e.Name).HasMaxLength(255);

                entity.Property(e => e.Status)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Type)
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Event>(entity =>
            {
                entity.ToTable("Event");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.City).HasMaxLength(50);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.District).HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(255);

                entity.Property(e => e.Status)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.TimeEnd).HasColumnType("datetime");

                entity.Property(e => e.TimeStart).HasColumnType("datetime");

                entity.Property(e => e.Type)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Ward).HasMaxLength(50);

                entity.HasOne(d => d.Creator)
                    .WithMany(p => p.Events)
                    .HasForeignKey(d => d.CreatorId)
                    .HasConstraintName("FK__Event__CreatorId__5165187F");
            });

            modelBuilder.Entity<EventPosition>(entity =>
            {
                entity.ToTable("EventPosition");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Status)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.Event)
                    .WithMany(p => p.EventPositions)
                    .HasForeignKey(d => d.EventId)
                    .HasConstraintName("FK__EventPosi__Event__59FA5E80");

                entity.HasOne(d => d.Template)
                    .WithMany(p => p.EventPositions)
                    .HasForeignKey(d => d.TemplateId)
                    .HasConstraintName("FK__EventPosi__Templ__5AEE82B9");
            });

            modelBuilder.Entity<Image>(entity =>
            {
                entity.ToTable("Image");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.KeyType)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Link).IsUnicode(false);
            });

            modelBuilder.Entity<Kiosk>(entity =>
            {
                entity.ToTable("Kiosk");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.Name).HasMaxLength(255);

                entity.Property(e => e.Status)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.KioskLocation)
                    .WithMany(p => p.Kiosks)
                    .HasForeignKey(d => d.KioskLocationId)
                    .HasConstraintName("FK__Kiosk__KioskLoca__31EC6D26");

                entity.HasOne(d => d.Party)
                    .WithMany(p => p.Kiosks)
                    .HasForeignKey(d => d.PartyId)
                    .HasConstraintName("FK__Kiosk__PartyId__32E0915F");
            });

            modelBuilder.Entity<KioskLocation>(entity =>
            {
                entity.ToTable("KioskLocation");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.District).HasMaxLength(100);

                entity.Property(e => e.Province).HasMaxLength(100);

                entity.Property(e => e.Status)
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<KioskSchedule>(entity =>
            {
                entity.ToTable("KioskSchedule");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.HasOne(d => d.Kiosk)
                    .WithMany(p => p.KioskSchedules)
                    .HasForeignKey(d => d.KioskId)
                    .HasConstraintName("FK__KioskSche__Kiosk__48CFD27E");

                entity.HasOne(d => d.Schedule)
                    .WithMany(p => p.KioskSchedules)
                    .HasForeignKey(d => d.ScheduleId)
                    .HasConstraintName("FK__KioskSche__Sched__49C3F6B7");
            });

            modelBuilder.Entity<Notification>(entity =>
            {
                entity.ToTable("Notification");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.Status)
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Party>(entity =>
            {
                entity.ToTable("Party");

                entity.HasIndex(e => e.PhoneNumber, "UQ__Party__85FB4E3886BF3FEA")
                    .IsUnique();

                entity.HasIndex(e => e.Email, "UQ__Party__A9D105347E298C3D")
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
                    .HasConstraintName("FK__Party__CreatorId__2A4B4B5E");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Parties)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK__Party__RoleId__2B3F6F97");
            });

            modelBuilder.Entity<PartyNotification>(entity =>
            {
                entity.ToTable("PartyNotification");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Status)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.Notification)
                    .WithMany(p => p.PartyNotifications)
                    .HasForeignKey(d => d.NotificationId)
                    .HasConstraintName("FK__PartyNoti__Notif__02084FDA");

                entity.HasOne(d => d.Party)
                    .WithMany(p => p.PartyNotifications)
                    .HasForeignKey(d => d.PartyId)
                    .HasConstraintName("FK__PartyNoti__Party__01142BA1");
            });

            modelBuilder.Entity<PartyServiceApplication>(entity =>
            {
                entity.ToTable("PartyServiceApplication");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.HasOne(d => d.Party)
                    .WithMany(p => p.PartyServiceApplications)
                    .HasForeignKey(d => d.PartyId)
                    .HasConstraintName("FK__PartyServ__Party__66603565");

                entity.HasOne(d => d.ServiceApplication)
                    .WithMany(p => p.PartyServiceApplications)
                    .HasForeignKey(d => d.ServiceApplicationId)
                    .HasConstraintName("FK__PartyServ__Servi__6754599E");
            });

            modelBuilder.Entity<Poi>(entity =>
            {
                entity.ToTable("POI");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.City).HasMaxLength(50);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.DayOfWeek)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.District).HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(255);

                entity.Property(e => e.PoicategoryId).HasColumnName("POICategoryId");

                entity.Property(e => e.Status)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Type)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Ward).HasMaxLength(50);

                entity.HasOne(d => d.Creator)
                    .WithMany(p => p.Pois)
                    .HasForeignKey(d => d.CreatorId)
                    .HasConstraintName("FK__POI__Type__619B8048");

                entity.HasOne(d => d.Poicategory)
                    .WithMany(p => p.Pois)
                    .HasForeignKey(d => d.PoicategoryId)
                    .HasConstraintName("FK__POI__POICategory__628FA481");
            });

            modelBuilder.Entity<Poicategory>(entity =>
            {
                entity.ToTable("POICategory");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Logo).IsUnicode(false);

                entity.Property(e => e.Name).HasMaxLength(255);
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Role");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Name).HasMaxLength(255);
            });

            modelBuilder.Entity<Schedule>(entity =>
            {
                entity.ToTable("Schedule");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.DateEnd).HasColumnType("datetime");

                entity.Property(e => e.DateStart).HasColumnType("datetime");

                entity.Property(e => e.DayOfWeek)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Name).HasMaxLength(255);

                entity.Property(e => e.Status)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.Party)
                    .WithMany(p => p.Schedules)
                    .HasForeignKey(d => d.PartyId)
                    .HasConstraintName("FK__Schedule__PartyI__44FF419A");
            });

            modelBuilder.Entity<ScheduleTemplate>(entity =>
            {
                entity.ToTable("ScheduleTemplate");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.HasOne(d => d.Schedule)
                    .WithMany(p => p.ScheduleTemplates)
                    .HasForeignKey(d => d.ScheduleId)
                    .HasConstraintName("FK__ScheduleT__Sched__6B24EA82");

                entity.HasOne(d => d.Template)
                    .WithMany(p => p.ScheduleTemplates)
                    .HasForeignKey(d => d.TemplateId)
                    .HasConstraintName("FK__ScheduleT__Templ__6C190EBB");
            });

            modelBuilder.Entity<ServiceApplication>(entity =>
            {
                entity.ToTable("ServiceApplication");

                entity.HasIndex(e => e.Name, "UQ__ServiceA__737584F6A84012A4")
                    .IsUnique();

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.Link).IsUnicode(false);

                entity.Property(e => e.Logo).IsUnicode(false);

                entity.Property(e => e.Name).HasMaxLength(255);

                entity.Property(e => e.Status)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.AppCategory)
                    .WithMany(p => p.ServiceApplications)
                    .HasForeignKey(d => d.AppCategoryId)
                    .HasConstraintName("FK__ServiceAp__AppCa__3F466844");

                entity.HasOne(d => d.ApplicationMarket)
                    .WithMany(p => p.ServiceApplications)
                    .HasForeignKey(d => d.ApplicationMarketId)
                    .HasConstraintName("FK__ServiceAp__Appli__412EB0B6");

                entity.HasOne(d => d.Party)
                    .WithMany(p => p.ServiceApplications)
                    .HasForeignKey(d => d.PartyId)
                    .HasConstraintName("FK__ServiceAp__Party__403A8C7D");
            });

            modelBuilder.Entity<ServiceApplicationPublishRequest>(entity =>
            {
                entity.ToTable("ServiceApplicationPublishRequest");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.Status)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.Creator)
                    .WithMany(p => p.ServiceApplicationPublishRequestCreators)
                    .HasForeignKey(d => d.CreatorId)
                    .HasConstraintName("FK__ServiceAp__Creat__797309D9");

                entity.HasOne(d => d.Handler)
                    .WithMany(p => p.ServiceApplicationPublishRequestHandlers)
                    .HasForeignKey(d => d.HandlerId)
                    .HasConstraintName("FK__ServiceAp__Handl__7A672E12");

                entity.HasOne(d => d.ServiceApplication)
                    .WithMany(p => p.ServiceApplicationPublishRequests)
                    .HasForeignKey(d => d.ServiceApplicationId)
                    .HasConstraintName("FK__ServiceAp__Servi__787EE5A0");
            });

            modelBuilder.Entity<ServiceOrder>(entity =>
            {
                entity.ToTable("ServiceOrder");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.Income).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.OrderDetail).IsUnicode(false);

                entity.Property(e => e.Status)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.SubmitDate).HasColumnType("datetime");

                entity.HasOne(d => d.Kiosk)
                    .WithMany(p => p.ServiceOrders)
                    .HasForeignKey(d => d.KioskId)
                    .HasConstraintName("FK__ServiceOr__Kiosk__74AE54BC");

                entity.HasOne(d => d.Party)
                    .WithMany(p => p.ServiceOrders)
                    .HasForeignKey(d => d.PartyId)
                    .HasConstraintName("FK__ServiceOr__Party__73BA3083");

                entity.HasOne(d => d.ServiceApplication)
                    .WithMany(p => p.ServiceOrders)
                    .HasForeignKey(d => d.ServiceApplicationId)
                    .HasConstraintName("FK__ServiceOr__Servi__72C60C4A");
            });

            modelBuilder.Entity<Template>(entity =>
            {
                entity.ToTable("Template");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.Name).HasMaxLength(255);

                entity.Property(e => e.Status)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.Party)
                    .WithMany(p => p.Templates)
                    .HasForeignKey(d => d.PartyId)
                    .HasConstraintName("FK__Template__PartyI__4D94879B");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
