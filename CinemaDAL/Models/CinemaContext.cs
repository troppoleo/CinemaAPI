using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace CinemaDAL.Models;

public partial class CinemaContext : DbContext
{
    public CinemaContext()
    {
    }

    public CinemaContext(DbContextOptions<CinemaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<CinemaRoom> CinemaRooms { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<JobQualification> JobQualifications { get; set; }

    public virtual DbSet<Projection> Projections { get; set; }

    public virtual DbSet<UserType> UserTypes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;database=Cinema;Trusted_Connection=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CinemaRoom>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__CinemaRo__3214EC07D65C7C8B");

            entity.ToTable("CinemaRoom");

            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Employee__3214EC07599A0153");

            entity.ToTable("Employee");

            entity.Property(e => e.JobQaulificationId).HasColumnName("jobQaulificationID");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Surname)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("surname");

            entity.HasOne(d => d.JobQaulification).WithMany(p => p.Employees)
                .HasForeignKey(d => d.JobQaulificationId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_JobQual_Employ");
        });

        modelBuilder.Entity<JobQualification>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__JobQuali__3214EC07DE929701");

            entity.ToTable("JobQualification");

            entity.Property(e => e.Description)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.ShortDescr)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("short_descr");
        });

        modelBuilder.Entity<Projection>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Projecti__3214EC07FD868ADB");

            entity.ToTable("Projection");

            entity.Property(e => e.CinemaRoomId).HasColumnName("cinemaRoomID");
            entity.Property(e => e.EndShow)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("end_show");
            entity.Property(e => e.StartShow)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("start_show");
            entity.Property(e => e.Title)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("title");

            entity.HasOne(d => d.CinemaRoom).WithMany(p => p.Projections)
                .HasForeignKey(d => d.CinemaRoomId)
                .HasConstraintName("FK_Projection_ToTable");
        });

        modelBuilder.Entity<UserType>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.Description)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.UserTypeShort)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("userTypeShort");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
