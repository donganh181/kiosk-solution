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
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Schedule> Schedules { get; set; }
        public virtual DbSet<ScheduleTemplate> ScheduleTemplates { get; set; }
        public virtual DbSet<ServiceApplication> ServiceApplications { get; set; }
        public virtual DbSet<ServiceApplicationPublishRequest> ServiceApplicationPublishRequests { get; set; }
        public virtual DbSet<ServiceOrder> ServiceOrders { get; set; }
        public virtual DbSet<Template> Templates { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=103.125.170.20, 10052;Database=Kiosk_Platform;User Id =sa;Password=Goboi123;Trusted_Connection=False;Persist Security Info=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<AppCategory>(entity =>
            {
                entity.ToTable("AppCategory");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Name).HasMaxLength(255);
            });

            modelBuilder.Entity<AppCategoryPosition>(entity =>
            {
                entity.ToTable("AppCategoryPosition");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Status)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.AppCategory)
                    .WithMany(p => p.AppCategoryPositions)
                    .HasForeignKey(d => d.AppCategoryId)
                    .HasConstraintName("FK__AppCatego__AppCa__656C112C");

                entity.HasOne(d => d.Template)
                    .WithMany(p => p.AppCategoryPositions)
                    .HasForeignKey(d => d.TemplateId)
                    .HasConstraintName("FK__AppCatego__Templ__66603565");
            });

            modelBuilder.Entity<ApplicationMarket>(entity =>
            {
                entity.ToTable("ApplicationMarket");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Logo).IsUnicode(false);

                entity.Property(e => e.Name).HasMaxLength(255);

                entity.Property(e => e.Status)
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Event>(entity =>
            {
                entity.ToTable("Event");

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
                    .HasForeignKey(d => d.CreatorId)
                    .HasConstraintName("FK__Event__CreatorId__619B8048");

                entity.HasOne(d => d.Template)
                    .WithMany(p => p.Events)
                    .HasForeignKey(d => d.TemplateId)
                    .HasConstraintName("FK__Event__Status__60A75C0F");
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
                    .HasConstraintName("FK__EventPosi__Event__6A30C649");

                entity.HasOne(d => d.Template)
                    .WithMany(p => p.EventPositions)
                    .HasForeignKey(d => d.TemplateId)
                    .HasConstraintName("FK__EventPosi__Templ__6B24EA82");
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

                entity.Property(e => e.Latitude)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Longtitude)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Name).HasMaxLength(255);

                entity.Property(e => e.Status)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.KioskLocation)
                    .WithMany(p => p.Kiosks)
                    .HasForeignKey(d => d.KioskLocationId)
                    .HasConstraintName("FK__Kiosk__KioskLoca__440B1D61");

                entity.HasOne(d => d.Party)
                    .WithMany(p => p.Kiosks)
                    .HasForeignKey(d => d.PartyId)
                    .HasConstraintName("FK__Kiosk__PartyId__44FF419A");
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
                entity.ToTable("Kiosk_Schedule");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.HasOne(d => d.Kiosk)
                    .WithMany(p => p.KioskSchedules)
                    .HasForeignKey(d => d.KioskId)
                    .HasConstraintName("FK__Kiosk_Sch__Kiosk__5812160E");

                entity.HasOne(d => d.Schedule)
                    .WithMany(p => p.KioskSchedules)
                    .HasForeignKey(d => d.ScheduleId)
                    .HasConstraintName("FK__Kiosk_Sch__Sched__59063A47");
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

                entity.HasIndex(e => e.PhoneNumber, "UQ__Party__85FB4E3832AEBB2C")
                    .IsUnique();

                entity.HasIndex(e => e.Email, "UQ__Party__A9D10534315F0F38")
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
                    .HasConstraintName("FK__Party__CreatorId__3C69FB99");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Parties)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK__Party__RoleId__3D5E1FD2");
            });

            modelBuilder.Entity<PartyNotification>(entity =>
            {
                entity.ToTable("Party_Notification");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Status)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.Notification)
                    .WithMany(p => p.PartyNotifications)
                    .HasForeignKey(d => d.NotificationId)
                    .HasConstraintName("FK__Party_Not__Notif__0E6E26BF");

                entity.HasOne(d => d.Party)
                    .WithMany(p => p.PartyNotifications)
                    .HasForeignKey(d => d.PartyId)
                    .HasConstraintName("FK__Party_Not__Party__0D7A0286");
            });

            modelBuilder.Entity<PartyServiceApplication>(entity =>
            {
                entity.ToTable("Party_ServiceApplication");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.HasOne(d => d.Party)
                    .WithMany(p => p.PartyServiceApplications)
                    .HasForeignKey(d => d.PartyId)
                    .HasConstraintName("FK__Party_Ser__Party__72C60C4A");

                entity.HasOne(d => d.ServiceApplication)
                    .WithMany(p => p.PartyServiceApplications)
                    .HasForeignKey(d => d.ServiceApplicationId)
                    .HasConstraintName("FK__Party_Ser__Servi__73BA3083");
            });

            modelBuilder.Entity<Poi>(entity =>
            {
                entity.ToTable("POI");

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
                    .HasForeignKey(d => d.CreatorId)
                    .HasConstraintName("FK__POI__CreatorId__6EF57B66");
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
                    .HasConstraintName("FK__Schedule__PartyI__5441852A");
            });

            modelBuilder.Entity<ScheduleTemplate>(entity =>
            {
                entity.ToTable("Schedule_Template");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.HasOne(d => d.Schedule)
                    .WithMany(p => p.ScheduleTemplates)
                    .HasForeignKey(d => d.ScheduleId)
                    .HasConstraintName("FK__Schedule___Sched__778AC167");

                entity.HasOne(d => d.Template)
                    .WithMany(p => p.ScheduleTemplates)
                    .HasForeignKey(d => d.TemplateId)
                    .HasConstraintName("FK__Schedule___Templ__787EE5A0");
            });

            modelBuilder.Entity<ServiceApplication>(entity =>
            {
                entity.ToTable("ServiceApplication");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Link).IsUnicode(false);

                entity.Property(e => e.Logo).IsUnicode(false);

                entity.Property(e => e.Name).HasMaxLength(255);

                entity.Property(e => e.Status)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.AppCategory)
                    .WithMany(p => p.ServiceApplications)
                    .HasForeignKey(d => d.AppCategoryId)
                    .HasConstraintName("FK__ServiceAp__AppCa__4E88ABD4");

                entity.HasOne(d => d.ApplicationMarket)
                    .WithMany(p => p.ServiceApplications)
                    .HasForeignKey(d => d.ApplicationMarketId)
                    .HasConstraintName("FK__ServiceAp__Appli__5070F446");

                entity.HasOne(d => d.Party)
                    .WithMany(p => p.ServiceApplications)
                    .HasForeignKey(d => d.PartyId)
                    .HasConstraintName("FK__ServiceAp__Party__4F7CD00D");
            });

            modelBuilder.Entity<ServiceApplicationPublishRequest>(entity =>
            {
                entity.ToTable("ServiceApplicationPublishRequest");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Status)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.Creator)
                    .WithMany(p => p.ServiceApplicationPublishRequestCreators)
                    .HasForeignKey(d => d.CreatorId)
                    .HasConstraintName("FK__ServiceAp__Creat__05D8E0BE");

                entity.HasOne(d => d.Handler)
                    .WithMany(p => p.ServiceApplicationPublishRequestHandlers)
                    .HasForeignKey(d => d.HandlerId)
                    .HasConstraintName("FK__ServiceAp__Handl__06CD04F7");

                entity.HasOne(d => d.ServiceApplication)
                    .WithMany(p => p.ServiceApplicationPublishRequests)
                    .HasForeignKey(d => d.ServiceApplicationId)
                    .HasConstraintName("FK__ServiceAp__Servi__04E4BC85");
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
                    .HasConstraintName("FK__ServiceOr__Kiosk__01142BA1");

                entity.HasOne(d => d.Party)
                    .WithMany(p => p.ServiceOrders)
                    .HasForeignKey(d => d.PartyId)
                    .HasConstraintName("FK__ServiceOr__Party__00200768");

                entity.HasOne(d => d.ServiceApplication)
                    .WithMany(p => p.ServiceOrders)
                    .HasForeignKey(d => d.ServiceApplicationId)
                    .HasConstraintName("FK__ServiceOr__Servi__7F2BE32F");
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
                    .HasConstraintName("FK__Template__PartyI__5CD6CB2B");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
