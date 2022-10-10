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
        public virtual DbSet<Event> Events { get; set; }
        public virtual DbSet<EventPosition> EventPositions { get; set; }
        public virtual DbSet<Image> Images { get; set; }
        public virtual DbSet<Kiosk> Kiosks { get; set; }
        public virtual DbSet<KioskLocation> KioskLocations { get; set; }
        public virtual DbSet<KioskRating> KioskRatings { get; set; }
        public virtual DbSet<KioskScheduleTemplate> KioskScheduleTemplates { get; set; }
        public virtual DbSet<Notification> Notifications { get; set; }
        public virtual DbSet<Party> Parties { get; set; }
        public virtual DbSet<PartyNotification> PartyNotifications { get; set; }
        public virtual DbSet<PartyServiceApplication> PartyServiceApplications { get; set; }
        public virtual DbSet<Poi> Pois { get; set; }
        public virtual DbSet<Poicategory> Poicategories { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Schedule> Schedules { get; set; }
        public virtual DbSet<ServiceApplication> ServiceApplications { get; set; }
        public virtual DbSet<ServiceApplicationFeedBack> ServiceApplicationFeedBacks { get; set; }
        public virtual DbSet<ServiceApplicationPublishRequest> ServiceApplicationPublishRequests { get; set; }
        public virtual DbSet<ServiceOrder> ServiceOrders { get; set; }
        public virtual DbSet<Template> Templates { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<AppCategory>(entity =>
            {
                entity.ToTable("AppCategory");

                entity.HasIndex(e => e.Name, "UQ__AppCateg__737584F6BAC0DFFE")
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
                    .HasConstraintName("FK__AppCatego__AppCa__4BAC3F29");

                entity.HasOne(d => d.Template)
                    .WithMany(p => p.AppCategoryPositions)
                    .HasForeignKey(d => d.TemplateId)
                    .HasConstraintName("FK__AppCatego__Templ__4CA06362");
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
                    .HasConstraintName("FK__Event__CreatorId__47DBAE45");
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
                    .HasConstraintName("FK__EventPosi__Event__5070F446");

                entity.HasOne(d => d.Template)
                    .WithMany(p => p.EventPositions)
                    .HasForeignKey(d => d.TemplateId)
                    .HasConstraintName("FK__EventPosi__Templ__5165187F");
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

                entity.Property(e => e.DeviceId)
                    .IsUnicode(false)
                    .HasColumnName("deviceId");

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

                entity.Property(e => e.HotLine)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Name).HasMaxLength(255);

                entity.Property(e => e.Status)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.Owner)
                    .WithMany(p => p.KioskLocations)
                    .HasForeignKey(d => d.OwnerId)
                    .HasConstraintName("FK__KioskLoca__Owner__18EBB532");
            });

            modelBuilder.Entity<KioskRating>(entity =>
            {
                entity.ToTable("Kiosk_Rating");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.HasOne(d => d.Kiosk)
                    .WithMany(p => p.KioskRatings)
                    .HasForeignKey(d => d.KioskId)
                    .HasConstraintName("FK__Kiosk_Rat__Kiosk__208CD6FA");
            });

            modelBuilder.Entity<KioskScheduleTemplate>(entity =>
            {
                entity.ToTable("KioskScheduleTemplate");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Status)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.Kiosk)
                    .WithMany(p => p.KioskScheduleTemplates)
                    .HasForeignKey(d => d.KioskId)
                    .HasConstraintName("FK__KioskSche__Kiosk__6383C8BA");

                entity.HasOne(d => d.Schedule)
                    .WithMany(p => p.KioskScheduleTemplates)
                    .HasForeignKey(d => d.ScheduleId)
                    .HasConstraintName("FK__KioskSche__Sched__619B8048");

                entity.HasOne(d => d.Template)
                    .WithMany(p => p.KioskScheduleTemplates)
                    .HasForeignKey(d => d.TemplateId)
                    .HasConstraintName("FK__KioskSche__Templ__628FA481");
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

                entity.HasIndex(e => e.PhoneNumber, "UQ__Party__85FB4E381DFCBB24")
                    .IsUnique();

                entity.HasIndex(e => e.Email, "UQ__Party__A9D10534FC3551E9")
                    .IsUnique();

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.DateOfBirth).HasColumnType("date");

                entity.Property(e => e.DeviceId)
                    .IsUnicode(false)
                    .HasColumnName("deviceId");

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

                entity.Property(e => e.VerifyCode)
                    .HasMaxLength(50)
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
                    .HasConstraintName("FK__PartyNoti__Notif__797309D9");

                entity.HasOne(d => d.Party)
                    .WithMany(p => p.PartyNotifications)
                    .HasForeignKey(d => d.PartyId)
                    .HasConstraintName("FK__PartyNoti__Party__787EE5A0");
            });

            modelBuilder.Entity<PartyServiceApplication>(entity =>
            {
                entity.ToTable("PartyServiceApplication");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Status)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.Party)
                    .WithMany(p => p.PartyServiceApplications)
                    .HasForeignKey(d => d.PartyId)
                    .HasConstraintName("FK__PartyServ__Party__5CD6CB2B");

                entity.HasOne(d => d.ServiceApplication)
                    .WithMany(p => p.PartyServiceApplications)
                    .HasForeignKey(d => d.ServiceApplicationId)
                    .HasConstraintName("FK__PartyServ__Servi__5DCAEF64");
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
                    .HasConstraintName("FK__POI__Type__5812160E");

                entity.HasOne(d => d.Poicategory)
                    .WithMany(p => p.Pois)
                    .HasForeignKey(d => d.PoicategoryId)
                    .HasConstraintName("FK__POI__POICategory__59063A47");
            });

            modelBuilder.Entity<Poicategory>(entity =>
            {
                entity.ToTable("POICategory");

                entity.HasIndex(e => e.Name, "poi_cate_name")
                    .IsUnique();

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
                    .HasConstraintName("FK__Schedule__PartyI__403A8C7D");
            });

            modelBuilder.Entity<ServiceApplication>(entity =>
            {
                entity.ToTable("ServiceApplication");

                entity.HasIndex(e => e.Name, "UQ__ServiceA__737584F6163A3E21")
                    .IsUnique();

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.IsAffiliate).HasColumnName("isAffiliate");

                entity.Property(e => e.Link).IsUnicode(false);

                entity.Property(e => e.Logo).IsUnicode(false);

                entity.Property(e => e.Name).HasMaxLength(255);

                entity.Property(e => e.Status)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.AppCategory)
                    .WithMany(p => p.ServiceApplications)
                    .HasForeignKey(d => d.AppCategoryId)
                    .HasConstraintName("FK__ServiceAp__AppCa__3B75D760");

                entity.HasOne(d => d.Party)
                    .WithMany(p => p.ServiceApplications)
                    .HasForeignKey(d => d.PartyId)
                    .HasConstraintName("FK__ServiceAp__Party__3C69FB99");
            });

            modelBuilder.Entity<ServiceApplicationFeedBack>(entity =>
            {
                entity.ToTable("ServiceApplicationFeedBack");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.HasOne(d => d.Party)
                    .WithMany(p => p.ServiceApplicationFeedBacks)
                    .HasForeignKey(d => d.PartyId)
                    .HasConstraintName("FK__ServiceAp__Party__7E37BEF6");

                entity.HasOne(d => d.ServiceApplication)
                    .WithMany(p => p.ServiceApplicationFeedBacks)
                    .HasForeignKey(d => d.ServiceApplicationId)
                    .HasConstraintName("FK__ServiceAp__Servi__7D439ABD");
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
                    .HasConstraintName("FK__ServiceAp__Creat__70DDC3D8");

                entity.HasOne(d => d.Handler)
                    .WithMany(p => p.ServiceApplicationPublishRequestHandlers)
                    .HasForeignKey(d => d.HandlerId)
                    .HasConstraintName("FK__ServiceAp__Handl__71D1E811");

                entity.HasOne(d => d.ServiceApplication)
                    .WithMany(p => p.ServiceApplicationPublishRequests)
                    .HasForeignKey(d => d.ServiceApplicationId)
                    .HasConstraintName("FK__ServiceAp__Servi__6FE99F9F");
            });

            modelBuilder.Entity<ServiceOrder>(entity =>
            {
                entity.ToTable("ServiceOrder");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Commission).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.OrderDetail).IsUnicode(false);

                entity.Property(e => e.SystemCommission).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Total).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.Kiosk)
                    .WithMany(p => p.ServiceOrders)
                    .HasForeignKey(d => d.KioskId)
                    .HasConstraintName("FK__ServiceOr__Kiosk__25518C17");

                entity.HasOne(d => d.ServiceApplication)
                    .WithMany(p => p.ServiceOrders)
                    .HasForeignKey(d => d.ServiceApplicationId)
                    .HasConstraintName("FK__ServiceOr__Servi__245D67DE");
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
                    .HasConstraintName("FK__Template__PartyI__440B1D61");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
