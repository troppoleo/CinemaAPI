using CinemaAPI.Middleware;
using CinemaAPI.Tasks;
using CinemaBL;
using CinemaBL.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// stringa di connessione:
builder.Services.AddDbContext<CinemaDAL.Models.CinemaContext>(x => x.UseSqlServer(builder.Configuration.GetConnectionString("connSql")));

// vari DI:
builder.Services.AddTransient<CinemaBL.IUserTypeService, CinemaBL.UserTypeService>();
builder.Services.AddTransient<CinemaBL.IJobQualificationService, CinemaBL.JobQualificationService>();
builder.Services.AddTransient<CinemaBL.ICinemaRoomService, CinemaBL.CinemaRoomService>();
builder.Services.AddTransient<CinemaBL.ITokenMng, CinemaBL.TokenMng>();
builder.Services.AddTransient<CinemaBL.IUsersMng, CinemaBL.UsersMng>();
builder.Services.AddTransient<IMovieService, MovieService>();

// UnitOfWork e Repository: query centralizzate nel repository:
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// configurazione di Swagger per fare l'autenticazione
builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo { Title = "MyAPI", Version = "v1" });
    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });
    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});


// Registro il mio Periodic Background Task
builder.Services.AddHostedService<ServiceDoCheck>();

// added
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,     // invertite rispetto al demo
            ValidateAudience = true,   // invertite rispetto al demo
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });


builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("GET_TICKET", policy =>
        policy.RequireClaim("GET_TICKET", "true"));

    //options.AddPolicy("CheckAge21", policy =>
    //    policy.Requirements.Add(new MinimunAgeRequirement(21)));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


// added: credo applichi quando scritto nel builder.Services.AddAuthentication
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

// salvataggi a DB:
app.MySaveChangeOnDB();
// gestione delle eccezioni
app.MyCatchException();


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