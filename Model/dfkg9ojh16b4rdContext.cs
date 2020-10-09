using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DrinkWater
{
    public partial class dfkg9ojh16b4rdContext : DbContext
    {
        public dfkg9ojh16b4rdContext()
        {
        }

        public dfkg9ojh16b4rdContext(DbContextOptions<dfkg9ojh16b4rdContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ActivityTime> ActivityTime { get; set; }
        public virtual DbSet<DailyStatistic> DailyStatistic { get; set; }
        public virtual DbSet<Notifications> Notifications { get; set; }
        public virtual DbSet<Statistic> Statistic { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<UserParameters> UserParameters { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseNpgsql("Host=ec2-34-253-148-186.eu-west-1.compute.amazonaws.com;Port=5432;Port=5432;Username =txhfeaeowkmudw;Password=991081b5cc1b5a824880f029a9c44c0351a6406425e381c8013c501beca8c1a4;Database=dfkg9ojh16b4rd;SSL Mode=Require;TrustServerCertificate=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ActivityTime>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("ActivityTime_pkey");

                entity.Property(e => e.UserId).ValueGeneratedNever();

                entity.Property(e => e.GoingToBed).HasColumnType("time without time zone");

                entity.Property(e => e.WakeUp).HasColumnType("time without time zone");

                entity.HasOne(d => d.User)
                    .WithOne(p => p.ActivityTime)
                    .HasForeignKey<ActivityTime>(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("ActivityTime_UserId_fkey");
            });

            modelBuilder.Entity<DailyStatistic>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("DailyStatistic_pkey");

                entity.Property(e => e.UserId)
                    .HasColumnName("UserID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Date).HasColumnType("date");

                entity.HasOne(d => d.User)
                    .WithOne(p => p.DailyStatistic)
                    .HasForeignKey<DailyStatistic>(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("DailyStatistic_UserID_fkey");
            });

            modelBuilder.Entity<Notifications>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("Notifications_pkey");

                entity.Property(e => e.UserId).ValueGeneratedNever();

                entity.Property(e => e.Period).HasColumnType("time without time zone");

                entity.HasOne(d => d.User)
                    .WithOne(p => p.Notifications)
                    .HasForeignKey<Notifications>(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Notifications_UserId_fkey");
            });

            modelBuilder.Entity<Statistic>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.LiqiudType).IsRequired();

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Statistic)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Statistic_UserId_fkey");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnType("character varying");

                entity.Property(e => e.Role).IsRequired();

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasColumnType("character varying");
            });

            modelBuilder.Entity<UserParameters>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("UserParameters_pkey");

                entity.Property(e => e.UserId).ValueGeneratedNever();

                entity.Property(e => e.Gender).IsRequired();

                entity.HasOne(d => d.User)
                    .WithOne(p => p.UserParameters)
                    .HasForeignKey<UserParameters>(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("UserParameters_UserId_fkey");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
