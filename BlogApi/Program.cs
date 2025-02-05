using BlogApi.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Cadena de conexión para MySQL (en este caso, usando el archivo appsettings.json)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))));

// Configuración de JWT
var key = "clave_secreta_para_firma"; // Debes cambiar esto a algo más seguro
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
    // Configurar CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularDevApp",
        builder => builder.WithOrigins("http://localhost:4200") // Dirección de tu frontend
        .AllowAnyMethod()
        .AllowAnyHeader());
});


builder.Services.AddAuthorization();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IArticleService, ArticleService>();
builder.Services.AddScoped<IAuthService, AuthService>();

var app = builder.Build();

// Middlewares para autenticación y autorización
app.UseAuthentication();
app.UseAuthorization();
app.UseCors("AllowAngularDevApp");

// Configuración de rutas
app.MapControllers();

app.Run();
