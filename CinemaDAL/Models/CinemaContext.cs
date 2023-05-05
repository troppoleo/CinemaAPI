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

    public virtual DbSet<JobEmployeeQualification> JobEmployeeQualifications { get; set; }

    public virtual DbSet<Movie> Movies { get; set; }

    public virtual DbSet<MovieSchedule> MovieSchedules { get; set; }

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

            entity.ToTable("CinemaRoom", tb => tb.HasComment("tabelle delle \"sale cinema\", utile per associare un employee alle sale cinema"));

            entity.Property(e => e.MaxStdSeat)
                .HasComment("Massimo numero di posto standard")
                .HasColumnName("maxStdSeat");
            entity.Property(e => e.MaxVipSeat)
                .HasComment("massimo numero di posti VIP")
                .HasColumnName("maxVipSeat");
            entity.Property(e => e.RoomName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasComment("Nome della sala")
                .HasColumnName("roomName");
            entity.Property(e => e.StdSeat)
                .HasDefaultValueSql("((0))")
                .HasComment("numero di posto standard assegnati")
                .HasColumnName("stdSeat");
            entity.Property(e => e.UpgradeVipPrice)
                .HasComment("percentuale di maggiorazione del prezzo VIP rispetto al prezzo standard")
                .HasColumnType("decimal(10, 10)")
                .HasColumnName("upgradeVipPrice");
            entity.Property(e => e.VipSeat)
                .HasDefaultValueSql("((0))")
                .HasComment("numero di posti VIP assegnati")
                .HasColumnName("vipSeat");
        });

        modelBuilder.Entity<JobEmployeeQualification>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__JobQuali__3214EC07DE929701");

            entity.ToTable("JobEmployeeQualification", tb => tb.HasComment("contiene le possibili mansioni che possono essere date ai soli EMPLOYEE:\r\n> Responsabili di sala\r\n> bigliettai"));

            entity.Property(e => e.Description)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.MinimumRequired)
                .HasComment("Numero minimo richiesto di Employee")
                .HasColumnName("minimum_required");
            entity.Property(e => e.ShortDescr)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("short_descr");
        });

        modelBuilder.Entity<Movie>(entity =>
        {
            entity.ToTable("Movie", tb => tb.HasComment("elenco dei film"));

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Actors)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasComment("lista degli attori principali")
                .HasColumnName("actors");
            entity.Property(e => e.Cover)
                .HasMaxLength(250)
                .HasComment("questo andrebbe fatto come \"image\" ma posso mettere anche l'url per ora mi semplifico la vita")
                .HasColumnName("cover");
            entity.Property(e => e.Director)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasComment("regista")
                .HasColumnName("director");
            entity.Property(e => e.Duration).HasColumnName("duration");
            entity.Property(e => e.FilmName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("filmName");
            entity.Property(e => e.Genere)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("genere");
            entity.Property(e => e.MoviePlot)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasComment("è la trama")
                .HasColumnName("moviePlot");
            entity.Property(e => e.ProductionYear).HasColumnName("productionYear");
            entity.Property(e => e.Trama)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("trama");
        });

        modelBuilder.Entity<MovieSchedule>(entity =>
        {
            entity.HasKey(e => new { e.MovieId, e.CinemaRoomId }).HasName("PK_Person");

            entity.ToTable("MovieSchedule", tb => tb.HasComment("mette in relazione i film con le sale cinematografiche, contiene:\r\n> data e ora di inizio \r\n> l'approvazione dell'ADMIN {1, 0}\r\n"));

            entity.Property(e => e.MovieId).HasColumnName("movieId");
            entity.Property(e => e.CinemaRoomId).HasColumnName("cinemaRoomId");
            entity.Property(e => e.IsApproved)
                .HasDefaultValueSql("((0))")
                .HasComment("1 se è stato approvato dall'Admin")
                .HasColumnName("isApproved");
            entity.Property(e => e.StartDate)
                .HasDefaultValueSql("([dbo].[GetMinDate]())")
                .HasColumnType("datetime")
                .HasColumnName("startDate");

            entity.HasOne(d => d.CinemaRoom).WithMany(p => p.MovieSchedules)
                .HasForeignKey(d => d.CinemaRoomId)
                .HasConstraintName("FK_MovieSchedule_CinemaRoom");

            entity.HasOne(d => d.Movie).WithMany(p => p.MovieSchedules)
                .HasForeignKey(d => d.MovieId)
                .HasConstraintName("FK_MovieSchedule_Movie");
        });

        modelBuilder.Entity<Projection>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Projecti__3214EC07FD868ADB");

            entity.ToTable("Projection", tb => tb.HasComment("contiene la programmazione delle proiezioni e l'associazione della sala cinema"));

            entity.Property(e => e.CinemaRoomId).HasColumnName("cinemaRoomId");
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
                .ToTable(tb => tb.HasComment("tabella con i vari ruoli:\r\nADMIN, EMPLOYEE, CUSTOMER\r\n"));

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

            entity.ToTable("UsersAdmin", tb => tb.HasComment("Specifica per gli amministratori"));

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
            entity.HasKey(e => e.Id).HasName("PK__UsersEmp__3214EC071722E0AA");

            entity.ToTable("UsersEmployee", tb => tb.HasComment("specifica per i soli Employee (no admin)"));

            entity.Property(e => e.Birthdate)
                .HasDefaultValueSql("([dbo].[GetMinDate]())")
                .HasColumnType("date");
            entity.Property(e => e.CinemaRoomId).HasColumnName("cinemaRoomId");
            entity.Property(e => e.IsActive)
                .HasComment("serve solo ai bilbiettai, per indicare se sono attivi o meno {null/0, 1 }")
                .HasColumnName("isActive");
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

            entity.HasOne(d => d.CinemaRoom).WithMany(p => p.UsersEmployees)
                .HasForeignKey(d => d.CinemaRoomId)
                .HasConstraintName("FK_UsersEmployee_CinemaRoom");

            entity.HasOne(d => d.JobQualification).WithMany(p => p.UsersEmployees)
                .HasForeignKey(d => d.JobQualificationId)
                .HasConstraintName("FK_JobQual_Employ");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
