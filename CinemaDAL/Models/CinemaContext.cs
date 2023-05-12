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

    public virtual DbSet<CinemaRoomCrossUserEmployee> CinemaRoomCrossUserEmployees { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<JobEmployeeQualification> JobEmployeeQualifications { get; set; }

    public virtual DbSet<Movie> Movies { get; set; }

    public virtual DbSet<MovieSchedule> MovieSchedules { get; set; }

    public virtual DbSet<Projection> Projections { get; set; }

    public virtual DbSet<Ticket> Tickets { get; set; }

    public virtual DbSet<UserEmployee> UserEmployees { get; set; }

    public virtual DbSet<UserType> UserTypes { get; set; }

    public virtual DbSet<UsersAdmin> UsersAdmins { get; set; }

    public virtual DbSet<WeekCalendar> WeekCalendars { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;database=Cinema;Trusted_Connection=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CinemaRoom>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__CinemaRo__3214EC07D65C7C8B");

            entity.ToTable("CinemaRoom", tb => tb.HasComment("tabelle delle \"sale cinema\", utile per associare un employee alle sale cinema"));

            entity.HasIndex(e => e.RoomName, "AK_roomName").IsUnique();

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
        });

        modelBuilder.Entity<CinemaRoomCrossUserEmployee>(entity =>
        {
            entity.ToTable("CinemaRoomCrossUserEmployee", tb => tb.HasComment("tabella che associa gli impiegati alle sale cinema"));

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CinemaRoomId).HasColumnName("cinemaRoomId");
            entity.Property(e => e.UserEmployeeId).HasColumnName("userEmployeeId");

            entity.HasOne(d => d.CinemaRoom).WithMany(p => p.CinemaRoomCrossUserEmployees)
                .HasForeignKey(d => d.CinemaRoomId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_CinemaRoomCrossUserEmployee_CinemaRoom");

            entity.HasOne(d => d.UserEmployee).WithMany(p => p.CinemaRoomCrossUserEmployees)
                .HasForeignKey(d => d.UserEmployeeId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_CinemaRoomCrossUserEmployee_UserEmployee");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.ToTable("Customer");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Birthdate)
                .HasColumnType("date")
                .HasColumnName("birthdate");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasMaxLength(100)
                .HasColumnName("password");
            entity.Property(e => e.Surname)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("surname");
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
            entity.Property(e => e.LimitAge)
                .HasComment("indica se è vietato ai minori di anni X")
                .HasColumnName("limitAge");
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
            entity.HasKey(e => e.Id).HasName("PK_Person");

            entity.ToTable("MovieSchedule", tb => tb.HasComment("mette in relazione i film con le sale cinematografiche, contiene:\r\n> data e ora di inizio \r\n> l'approvazione dell'ADMIN {1, 0}\r\n"));

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CinemaRoomId).HasColumnName("cinemaRoomId");
            entity.Property(e => e.IsApproved)
                .HasDefaultValueSql("((0))")
                .HasComment("1 se è stato approvato dall'Admin")
                .HasColumnName("isApproved");
            entity.Property(e => e.MovieId).HasColumnName("movieId");
            entity.Property(e => e.StartDate)
                .HasDefaultValueSql("([dbo].[GetMinDate]())")
                .HasColumnType("datetime")
                .HasColumnName("startDate");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValueSql("('WAITING')")
                .HasComment("dominio:\r\nWAITING --> deve ancora iniziare\r\nIN_PROGRESS --> è in corso di visione\r\nCLEAN_TIME --> è finito e stanno facendo le pulizie\r\nDONE --> finito e sala liberata, include i 10 min extra film\r\n\r\nutile per semplificare i filtri, aggiornata dal BGW")
                .HasColumnName("status");
            entity.Property(e => e.StdSeat).HasColumnName("stdSeat");
            entity.Property(e => e.VipSeat).HasColumnName("vipSeat");

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

            entity.ToTable("Projection", tb => tb.HasComment("contiene la schedulazione/programmazione delle proiezioni e l'associazione della sala cinema"));

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

        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_CustomerCrossMovieSchedule");

            entity.ToTable("Ticket", tb => tb.HasComment("tiene traccia dei biglietti emessi\r\ncon l'informazione dei film che un customer ha comprato\r\n"));

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CommentNote)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasComment("Commento sul film")
                .HasColumnName("commentNote");
            entity.Property(e => e.CustomerId).HasColumnName("customerId");
            entity.Property(e => e.MovieScheduleId).HasColumnName("movieScheduleId");
            entity.Property(e => e.Price)
                .HasComment("è il prezzo del biglietto che eventualmente potrebbe essere maggiorato per vip")
                .HasColumnType("money")
                .HasColumnName("price");
            entity.Property(e => e.Rate)
                .HasComment("Valurazione del film")
                .HasColumnName("rate");

            entity.HasOne(d => d.Customer).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_CustomerCrossMovieSchedule_Customer");

            entity.HasOne(d => d.MovieSchedule).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.MovieScheduleId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_CustomerCrossMovieSchedule_MovieSchedule");
        });

        modelBuilder.Entity<UserEmployee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UsersEmp__3214EC071722E0AA");

            entity.ToTable("UserEmployee", tb => tb.HasComment("specifica per i soli Employee (no admin)"));

            entity.Property(e => e.IsActive)
                .HasComment("serve solo ai bigliettai, per indicare se sono attivi o meno {null/0, 1 }")
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

            entity.HasOne(d => d.JobQualification).WithMany(p => p.UserEmployees)
                .HasForeignKey(d => d.JobQualificationId)
                .HasConstraintName("FK_JobQual_Employ");
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

        modelBuilder.Entity<WeekCalendar>(entity =>
        {
            entity.ToTable("WeekCalendar", tb => tb.HasComment("contiene i giorni della settimana con le fasce di apertura\r\n"));

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.DayName)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("dayName");
            entity.Property(e => e.EndTime)
                .HasPrecision(0)
                .HasColumnName("endTIme");
            entity.Property(e => e.StartTime)
                .HasPrecision(0)
                .HasColumnName("startTime");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
