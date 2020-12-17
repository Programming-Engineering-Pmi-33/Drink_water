#nullable disable

namespace DrinkWater
{
    using System;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata;

#pragma warning disable SA1300 // Element should begin with upper-case letter
    public partial class dfkg9ojh16b4rdContext : DbContext
#pragma warning restore SA1300 // Element should begin with upper-case letter
    {
        public dfkg9ojh16b4rdContext()
        {
        }

        public dfkg9ojh16b4rdContext(DbContextOptions<dfkg9ojh16b4rdContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Dailystatistic> Dailystatistics { get; set; }

        public virtual DbSet<Fluid> Fluids { get; set; }

        public virtual DbSet<Statistic> Statistics { get; set; }

        public virtual DbSet<Totalmonthstatistic> Totalmonthstatistics { get; set; }

        public virtual DbSet<Totalweekstatistic> Totalweekstatistics { get; set; }

        public virtual DbSet<Totalyearstatistic> Totalyearstatistics { get; set; }

        public virtual DbSet<User> Users { get; set; }

        public virtual DbSet<Watermonthstatistic> Watermonthstatistics { get; set; }

        public virtual DbSet<Waterweekstatistic> Waterweekstatistics { get; set; }

        public virtual DbSet<Wateryearstatistic> Wateryearstatistics { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql("Host=ec2-34-253-148-186.eu-west-1.compute.amazonaws.com;Port=5432;Port=5432;Username =txhfeaeowkmudw;Password=991081b5cc1b5a824880f029a9c44c0351a6406425e381c8013c501beca8c1a4;Database=dfkg9ojh16b4rd;SSL Mode=Require;TrustServerCertificate=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Dailystatistic>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("dailystatistic");

                entity.Property(e => e.Sum).HasColumnName("sum");
            });

            modelBuilder.Entity<Fluid>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("character varying");
            });

            modelBuilder.Entity<Statistic>(entity =>
            {
                entity.Property(e => e.Date).HasColumnType("date");

                entity.HasOne(d => d.FluidIdRefNavigation)
                    .WithMany(p => p.Statistics)
                    .HasForeignKey(d => d.FluidIdRef)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Statistics_FluidIdRef_fkey");

                entity.HasOne(d => d.UserIdRefNavigation)
                    .WithMany(p => p.Statistics)
                    .HasForeignKey(d => d.UserIdRef)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Statistics_UserIdRef_fkey");
            });

            modelBuilder.Entity<Totalmonthstatistic>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("totalmonthstatistic");

                entity.Property(e => e.Sum).HasColumnName("sum");
            });

            modelBuilder.Entity<Totalweekstatistic>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("totalweekstatistic");

                entity.Property(e => e.Sum).HasColumnName("sum");
            });

            modelBuilder.Entity<Totalyearstatistic>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("totalyearstatistic");

                entity.Property(e => e.Sum).HasColumnName("sum");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnType("character varying");

                entity.Property(e => e.GoingToBed).HasColumnType("time without time zone");

                entity.Property(e => e.NotitficationsPeriod).HasColumnType("time without time zone");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnType("character varying");

                entity.Property(e => e.Sex).HasColumnType("character varying");

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasColumnType("character varying");

                entity.Property(e => e.WakeUp).HasColumnType("time without time zone");
            });

            modelBuilder.Entity<Watermonthstatistic>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("watermonthstatistic");

                entity.Property(e => e.Amount).HasColumnName("amount");

                entity.Property(e => e.Date).HasColumnType("date");
            });

            modelBuilder.Entity<Waterweekstatistic>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("waterweekstatistic");

                entity.Property(e => e.Amount).HasColumnName("amount");

                entity.Property(e => e.Date).HasColumnType("date");
            });

            modelBuilder.Entity<Wateryearstatistic>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("wateryearstatistic");

                entity.Property(e => e.Amount).HasColumnName("amount");

                entity.Property(e => e.Date).HasColumnType("date");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
