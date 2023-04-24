using CinemaAPI.Middleware;
using CinemaAPI.Tasks;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddDbContext<CinemaDAL.Models.CinemaContext>(x => x.UseSqlServer(builder.Configuration.GetConnectionString("connSql")));
builder.Services.AddTransient<CinemaBL.IUserTypeService, CinemaBL.UserTypeService>();
builder.Services.AddTransient<CinemaBL.IJobQualificationService, CinemaBL.JobQualificationService>();
builder.Services.AddTransient<CinemaBL.ICinemaRoomService, CinemaBL.CinemaRoomService>();
builder.Services.AddTransient<CinemaBL.ITokenMng, CinemaBL.TokenMng>();
builder.Services.AddTransient<CinemaBL.IUsersMng, CinemaBL.UsersMng>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Registro il mio Periodic Background Task
builder.Services.AddHostedService<ServiceDoCheck>();




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// salvataggi a DB:
app.MySaveChangeOnDB();


app.Run();
/*
 * --------------------------------------
 * JWT:
 * --------------------------------------
 * attributo decoratore
 * [Authorize(Roles= "Admin,User")]
 * 
 * da "PM"
 * crea un ruolo per l'utenza di test
 * dotnet user-jwts create --role Admin
 
 */