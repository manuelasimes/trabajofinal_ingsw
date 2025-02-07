using BlogApi.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using BlogApi.AuthServices;
using BlogApi.ArticleServices;
using Microsoft.Extensions.DependencyInjection.Extensions;

var builder = WebApplication.CreateBuilder(args);

// ✅ Configurar la base de datos correctamente según el entorno
var environment = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? builder.Environment.EnvironmentName;

if (environment == "Testing")
{
    builder.Services.RemoveAll(typeof(DbContextOptions<ApplicationDbContext>));
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseInMemoryDatabase("TestDatabase"));
}
else
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
}

// 🔑 Configuración de autenticación con JWT
var key = "clave_secreta_para_firma"; // Cambiar en producción
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "tu_issuer",
            ValidAudience = "tu_audience",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
        };
    });

// 🌐 Configurar CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularDevApp",
        policy => policy.WithOrigins("http://localhost:4200")
            .AllowAnyMethod()
            .AllowAnyHeader());
});

// ✅ Servicios de la API
builder.Services.AddAuthorization();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IArticleService, ArticleService>();
builder.Services.AddScoped<IAuthService, AuthService>();

var app = builder.Build();

// ✅ Middlewares
app.UseAuthentication();
app.UseAuthorization();
app.UseCors("AllowAngularDevApp");

if (app.Environment.IsDevelopment() || environment == "Testing")
{
    app.UseDeveloperExceptionPage();
}

app.MapControllers();
app.Run();

// ✅ Agrega esta clase parcial para que WebApplicationFactory lo reconozca
public partial class Program { }
