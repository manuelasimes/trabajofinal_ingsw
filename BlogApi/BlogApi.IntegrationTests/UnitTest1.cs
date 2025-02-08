using Microsoft.Extensions.Configuration; // Asegúrate de importar este namespace
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Json;
using Xunit;
using BlogApi.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;
using BlogApi.Data;
using Microsoft.Extensions.Hosting;
using System.Linq;  // Asegúrate de importar esto para usar SingleOrDefault

namespace BlogApi.IntegrationTests
{
    
    public class ArticlesControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public ArticlesControllerTests(WebApplicationFactory<Program> factory)
        {
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Testing");

            _client = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureAppConfiguration((context, config) =>
                {
                    // Configurar el entorno para pruebas
                    var environment = "Testing"; // Establece el entorno como "Testing"
                    config.AddInMemoryCollection(new[] 
                    { 
                        new KeyValuePair<string, string>("DOTNET_ENVIRONMENT", environment) 
                    });
                })
                .ConfigureServices(services =>
                {
                    // 🔥 Eliminar cualquier DbContext registrado previamente para evitar conflictos
                    var descriptor = services.SingleOrDefault(
                        d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));

                    if (descriptor != null)
                    {
                        services.Remove(descriptor);
                    }

                    // 🔥 Agregar la base de datos en memoria correctamente
                    services.AddDbContext<ApplicationDbContext>(options =>
                        options.UseInMemoryDatabase("TestDatabase"));

                    // 🔥 Reconstruir el proveedor de servicios
                    var sp = services.BuildServiceProvider();

                    using (var scope = sp.CreateScope())
                    {
                        var scopedServices = scope.ServiceProvider;
                        var db = scopedServices.GetRequiredService<ApplicationDbContext>();
                        db.Database.EnsureDeleted();
                        db.Database.EnsureCreated();

                        try
                        {
                            // Insertar datos de prueba
                            db.Articles.Add(new Article { Id = 1, Title = "Test Article", Description = "Content of test article" });
                            db.SaveChanges();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error inicializando la base de datos de prueba: {ex.Message}");
                        }
                    }
                });
            }).CreateClient();
        }

        [Fact]
        public async Task GetArticles_ReturnsOkStatusAndArticles()
        {
            var response = await _client.GetAsync("/api/article");
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            var articles = JsonConvert.DeserializeObject<List<Article>>(responseString);

            Assert.NotEmpty(articles);
            Assert.Equal("application/json", response.Content.Headers.ContentType.MediaType);
        }

        [Fact]
        public async Task GetArticle_ReturnsOkStatusAndArticle_WhenArticleExists()
        {
            var response = await _client.GetAsync("/api/article/1");
            response.EnsureSuccessStatusCode();

            var article = JsonConvert.DeserializeObject<Article>(await response.Content.ReadAsStringAsync());
            Assert.NotNull(article);
            Assert.Equal(1, article.Id);
        }

        [Fact]
        public async Task PostArticle_ReturnsCreatedAtActionResult()
        {
            var newArticle = new Article { Title = "New Article", Description = "Content of new article" };
            var response = await _client.PostAsJsonAsync("/api/article", newArticle);

            response.EnsureSuccessStatusCode();
            Assert.Equal("application/json", response.Content.Headers.ContentType.MediaType);
            Assert.Equal(System.Net.HttpStatusCode.Created, response.StatusCode);
        }
    

    public void Dispose()
    {
        // Limpiar la variable de entorno después de las pruebas
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", null);
    }
    }}
    
