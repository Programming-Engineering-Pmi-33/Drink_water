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

        public virtual DbSet<Fluids> Fluids { get; set; }
        public virtual DbSet<Statistics> Statistics { get; set; }
        public virtual DbSet<Users> Users { get; set; }

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
            modelBuilder.Entity<Fluids>(entity =>
            {
                entity.HasKey(e => e.FluidId)
                    .HasName("Fluids_pkey");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("character varying");
            });

            modelBuilder.Entity<Statistics>(entity =>
            {
                entity.HasKey(e => e.StatisticId)
                    .HasName("Statistics_pkey");

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

            modelBuilder.Entity<Users>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("Users_pkey");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnType("character varying");

                entity.Property(e => e.Gender)
                    .IsRequired()
                    .HasColumnType("character varying");

                entity.Property(e => e.GoingToBed).HasColumnType("time without time zone");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnType("character varying");

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasColumnType("character varying");

                entity.Property(e => e.WakeUp).HasColumnType("time without time zone");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
