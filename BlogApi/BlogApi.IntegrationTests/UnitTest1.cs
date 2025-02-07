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

namespace BlogApi.IntegrationTests
{
    public class ArticlesControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public ArticlesControllerTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    // 🔥 Eliminar cualquier DbContext registrado previamente
                    var descriptor = services.SingleOrDefault(
                        d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));

                    if (descriptor != null)
                    {
                        services.Remove(descriptor);
                    }

                    // 🔥 Usar solo la base de datos en memoria para los tests
                    services.AddDbContext<ApplicationDbContext>(options =>
                        options.UseInMemoryDatabase("TestDatabase"));

                    // 🔥 Inicializar la base de datos con datos de prueba
                    using (var scope = services.BuildServiceProvider().CreateScope())
                    {
                        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                        db.Database.EnsureDeleted();
                        db.Database.EnsureCreated();

                        db.Articles.Add(new Article { Id = 1, Title = "Test Article", Description = "Content of test article" });
                        db.SaveChanges();
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
    }
}
