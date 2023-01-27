using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ClPlatform.DataModels
{
    public partial class ClPlatformContext : DbContext
    {
        public ClPlatformContext()
        {
        }

        public ClPlatformContext(DbContextOptions<ClPlatformContext> options)
            : base(options)
        {
        }

        public virtual DbSet<City> Cities { get; set; } = null!;
        public virtual DbSet<Comment> Comments { get; set; } = null!;
        public virtual DbSet<Country> Countries { get; set; } = null!;
        public virtual DbSet<ImgDesc> ImgDescs { get; set; } = null!;
        public virtual DbSet<Mission> Missions { get; set; } = null!;
        public virtual DbSet<MissionImg> MissionImgs { get; set; } = null!;
        public virtual DbSet<MissionSkill> MissionSkills { get; set; } = null!;
        public virtual DbSet<MissionUser> MissionUsers { get; set; } = null!;
        public virtual DbSet<Policy> Policies { get; set; } = null!;
        public virtual DbSet<Skill> Skills { get; set; } = null!;
        public virtual DbSet<SkillUser> SkillUsers { get; set; } = null!;
        public virtual DbSet<Story> Stories { get; set; } = null!;
        public virtual DbSet<Theme> Themes { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<Volunteer> Volunteers { get; set; } = null!;
        public virtual DbSet<WorkDay> WorkDays { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=DESKTOP-BVTLLAS\\SQLEXPRESS; Database=ClPlatform; Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<City>(entity =>
            {
                entity.ToTable("City");

                entity.HasIndex(e => e.CityName, "IX_City")
                    .IsUnique();

                entity.Property(e => e.CityName).HasMaxLength(50);

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.Cities)
                    .HasForeignKey(d => d.CountryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_City_Country");
            });

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.ToTable("Comment");

                entity.Property(e => e.Comment1)
                    .HasMaxLength(500)
                    .HasColumnName("Comment");

                entity.Property(e => e.DateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Comment_User");
            });

            modelBuilder.Entity<Country>(entity =>
            {
                entity.ToTable("Country");

                entity.HasIndex(e => e.CountryName, "IX_Country")
                    .IsUnique();

                entity.Property(e => e.CountryName).HasMaxLength(50);
            });

            modelBuilder.Entity<ImgDesc>(entity =>
            {
                entity.ToTable("ImgDesc");

                entity.Property(e => e.Description).HasMaxLength(1000);

                entity.Property(e => e.ImgUrl).HasMaxLength(500);

                entity.Property(e => e.Title).HasMaxLength(200);
            });

            modelBuilder.Entity<Mission>(entity =>
            {
                entity.ToTable("Mission");

                entity.Property(e => e.DetailDesc).HasMaxLength(4000);

                entity.Property(e => e.EndDate).HasColumnType("date");

                entity.Property(e => e.MissionName).HasMaxLength(100);

                entity.Property(e => e.OrganizationDesc).HasMaxLength(2000);

                entity.Property(e => e.OrganizationName).HasMaxLength(50);

                entity.Property(e => e.ShortDesc).HasMaxLength(300);

                entity.Property(e => e.SponcerDetail).HasMaxLength(2000);

                entity.Property(e => e.StartDate).HasColumnType("date");

                entity.HasOne(d => d.City)
                    .WithMany(p => p.Missions)
                    .HasForeignKey(d => d.CityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Mission_City");

                entity.HasOne(d => d.Theme)
                    .WithMany(p => p.Missions)
                    .HasForeignKey(d => d.ThemeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Mission_Theme");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Missions)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Mission_User");

                entity.HasOne(d => d.WorkDay)
                    .WithMany(p => p.Missions)
                    .HasForeignKey(d => d.WorkDayId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Mission_WorkDay");
            });

            modelBuilder.Entity<MissionImg>(entity =>
            {
                entity.ToTable("MissionImg");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.ImgUrl)
                    .HasMaxLength(500)
                    .HasColumnName("ImgURL");

                entity.HasOne(d => d.Mission)
                    .WithMany(p => p.MissionImgs)
                    .HasForeignKey(d => d.MissionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MissionImg_Mission");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.MissionImgs)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MissionImg_User");
            });

            modelBuilder.Entity<MissionSkill>(entity =>
            {
                entity.ToTable("MissionSkill");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.Mission)
                    .WithMany(p => p.MissionSkills)
                    .HasForeignKey(d => d.MissionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MissionSkill_Mission");

                entity.HasOne(d => d.Skill)
                    .WithMany(p => p.MissionSkills)
                    .HasForeignKey(d => d.SkillId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MissionSkill_Skill");
            });

            modelBuilder.Entity<MissionUser>(entity =>
            {
                entity.ToTable("MissionUser");

                entity.HasOne(d => d.Mission)
                    .WithMany(p => p.MissionUsers)
                    .HasForeignKey(d => d.MissionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MissionUser_Mission");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.MissionUsers)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MissionUser_User");
            });

            modelBuilder.Entity<Policy>(entity =>
            {
                entity.ToTable("Policy");

                entity.Property(e => e.PolicyDesc).HasMaxLength(4000);

                entity.Property(e => e.Title).HasMaxLength(100);
            });

            modelBuilder.Entity<Skill>(entity =>
            {
                entity.ToTable("Skill");

                entity.HasIndex(e => e.SkillName, "IX_Skill")
                    .IsUnique();

                entity.Property(e => e.SkillName).HasMaxLength(50);
            });

            modelBuilder.Entity<SkillUser>(entity =>
            {
                entity.ToTable("SkillUser");

                entity.HasOne(d => d.Skill)
                    .WithMany(p => p.SkillUsers)
                    .HasForeignKey(d => d.SkillId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SkillUser_Skill");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.SkillUsers)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SkillUser_User");
            });

            modelBuilder.Entity<Story>(entity =>
            {
                entity.ToTable("Story");

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.Photo).HasMaxLength(500);

                entity.Property(e => e.StoryDesc).HasMaxLength(4000);

                entity.Property(e => e.StoryTitle).HasMaxLength(100);

                entity.Property(e => e.VideoUrl)
                    .HasMaxLength(200)
                    .HasColumnName("VideoURL");

                entity.HasOne(d => d.IdNavigation)
                    .WithOne(p => p.Story)
                    .HasForeignKey<Story>(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Story_Mission");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Stories)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Story_User");
            });

            modelBuilder.Entity<Theme>(entity =>
            {
                entity.ToTable("Theme");

                entity.HasIndex(e => e.ThemeName, "IX_Theme")
                    .IsUnique();

                entity.Property(e => e.ThemeName).HasMaxLength(50);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.HasIndex(e => e.Email, "IX_User")
                    .IsUnique();

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.FirstName).HasMaxLength(50);

                entity.Property(e => e.LastName).HasMaxLength(50);

                entity.Property(e => e.Password).HasMaxLength(20);

                entity.Property(e => e.PhoneNo).HasMaxLength(10);
            });

            modelBuilder.Entity<Volunteer>(entity =>
            {
                entity.ToTable("Volunteer");

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.AboutVolunteer).HasMaxLength(4000);

                entity.Property(e => e.Company).HasMaxLength(50);

                entity.Property(e => e.Department).HasMaxLength(50);

                entity.Property(e => e.LinkedIn).HasMaxLength(200);

                entity.Property(e => e.Photo).HasMaxLength(500);

                entity.Property(e => e.WhyVolunteer).HasMaxLength(4000);

                entity.HasOne(d => d.City)
                    .WithMany(p => p.Volunteers)
                    .HasForeignKey(d => d.CityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Volunteer_City");

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.Volunteers)
                    .HasForeignKey(d => d.CountryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Volunteer_Country");

                entity.HasOne(d => d.IdNavigation)
                    .WithOne(p => p.Volunteer)
                    .HasForeignKey<Volunteer>(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Volunteer_WorkDay");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Volunteers)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Volunteer_User");
            });

            modelBuilder.Entity<WorkDay>(entity =>
            {
                entity.ToTable("WorkDay");

                entity.HasIndex(e => e.Title, "IX_WorkDay")
                    .IsUnique();

                entity.Property(e => e.Title).HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
