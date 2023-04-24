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

    public virtual DbSet<JobQualification> JobQualifications { get; set; }

    public virtual DbSet<Projection> Projections { get; set; }

    public virtual DbSet<UserType> UserTypes { get; set; }

    public virtual DbSet<UsersAdmin> UsersAdmins { get; set; }

    public virtual DbSet<UsersEmployee> UsersEmployees { get; set; }

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

        modelBuilder.Entity<JobQualification>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__JobQuali__3214EC07DE929701");

            entity.ToTable("JobQualification", tb => tb.HasComment("contiene le possibili mansioni che possono essere date ai soli EMPLOYEE:\r\n> Responsabili di sala\r\n> bigliettai"));

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
            entity
                .HasNoKey()
                .ToTable(tb => tb.HasComment("tabella con i vari ruoli \r\nADMIN\r\nEMPLOYEE\r\nCUSTOMER\r\n"));

            entity.Property(e => e.Description)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasComment("Tabella delle qualifiche che possono essere CRUD dal'admin");
            entity.Property(e => e.UserTypeShort)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasComment("nome breve")
                .HasColumnName("userTypeShort");
        });

        modelBuilder.Entity<UsersAdmin>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UsersAdm__3214EC07027A5AF9");

            entity.ToTable("UsersAdmin");

            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Password).HasMaxLength(200);
            entity.Property(e => e.Surname)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("surname");
            entity.Property(e => e.UserName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<UsersEmployee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UsersEmp__3214EC075B506F21");

            entity.ToTable("UsersEmployee");

            entity.Property(e => e.Birthdate)
                .HasDefaultValueSql("([dbo].[GetMinDate]())")
                .HasColumnType("date");
            entity.Property(e => e.JobQualificationId).HasColumnName("jobQualificationID");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Password).HasMaxLength(200);
            entity.Property(e => e.Surname)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("surname");
            entity.Property(e => e.UserName)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.JobQualification).WithMany(p => p.UsersEmployees)
                .HasForeignKey(d => d.JobQualificationId)
                .HasConstraintName("FK_JobQual_Employ");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
