using CinemaAPI.Hubs;
using CinemaAPI.Middleware;
using CinemaAPI.Tasks;
using CinemaBL;
using CinemaBL.Repository;
using CinemaDAL.Models;
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

builder.Services.AddSignalR();

// vari DI:
builder.Services.AddTransient<IUserTypeService, UserTypeService>();
builder.Services.AddTransient<IJobQualificationService, JobQualificationService>();
builder.Services.AddTransient<ICinemaRoomService, CinemaRoomService>();
builder.Services.AddTransient<ITokenMng, TokenMng>();
builder.Services.AddTransient<IUsersMng, UsersMng>();
builder.Services.AddTransient<IMovieService, MovieService>();
builder.Services.AddTransient<IUserEmployeeService, UserEmployeeService>();
builder.Services.AddTransient<IJobEmployeeQualificationService, JobEmployeeQualificationService>();
builder.Services.AddTransient<IMovieScheduleService, MovieScheduleService>();
builder.Services.AddTransient<IReviewService, ReviewService>();

// questa serve per i messaggi HUB
builder.Services.AddTransient<INotify, Notify>();

// UnitOfWork e Repository: query centralizzate nel repository:
//builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
//builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUnitOfWorkGeneric, UnitOfWorkGeneric>();

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
// TODO: RIABILITARE, ADESSO MI DA SOLO FASTIDIO
//builder.Services.AddHostedService<ServiceSetStatusToDONE>();

// BG per inviare i cambiamenti delle poltrone nelle sale:
builder.Services.AddHostedService<ServicePushBySignalRSeatRoom>();



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
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];

                // If the request is for our hub...
                var path = context.HttpContext.Request.Path;
                if (!string.IsNullOrEmpty(accessToken) &&
                    (path.StartsWithSegments("/CinemaHub")))
                {
                    // Read the token out of the query string
                    context.Token = accessToken;
                }
                return Task.CompletedTask;
            }
        };
    });


builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("GET_TICKET", policy =>
        policy.RequireClaim("GET_TICKET", "true"));

    //options.AddPolicy("CheckAge21", policy =>
    //    policy.Requirements.Add(new MinimunAgeRequirement(21)));
});

//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//}).AddJwtBearer(options =>
//{
//    // Configure the Authority to the expected value for
//    // the authentication provider. This ensures the token
//    // is appropriately validated.
//    options.Authority = "Authority URL"; // TODO: Update URL

//    // We have to hook the OnMessageReceived event in order to
//    // allow the JWT authentication handler to read the access
//    // token from the query string when a WebSocket or 
//    // Server-Sent Events request comes in.

//    // Sending the access token in the query string is required due to
//    // a limitation in Browser APIs. We restrict it to only calls to the
//    // SignalR hub in this code.
//    // See https://docs.microsoft.com/aspnet/core/signalr/security#access-token-logging
//    // for more information about security considerations when using
//    // the query string to transmit the access token.
//    options.Events = new JwtBearerEvents
//    {
//        OnMessageReceived = context =>
//        {
//            var accessToken = context.Request.Query["access_token"];

//            // If the request is for our hub...
//            var path = context.HttpContext.Request.Path;
//            if (!string.IsNullOrEmpty(accessToken) &&
//                (path.StartsWithSegments("/hubs/chat")))
//            {
//                // Read the token out of the query string
//                context.Token = accessToken;
//            }
//            return Task.CompletedTask;
//        }
//    };
//});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


// Initialize the database
//var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
//using (var scope = scopeFactory.CreateScope())
//{
//    var db = scope.ServiceProvider.GetRequiredService<CinemaContext>();
//    if (db.Database.EnsureCreated())
//    {   
//        //SeedData.Initialize(db);
//    }
//}

app.MapHub<CinemaHub>("/CinemaHub");
app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().AllowCredentials().WithOrigins("http://localhost:8080", "https://gourav-d.github.io"));
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