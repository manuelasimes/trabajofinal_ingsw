using Xunit;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using BlogApi.Data;
using BlogApi.Models;
using BlogApi.ArticleServices;
using System.Threading.Tasks;
using System.Linq;

public class ArticleServiceTests
{
    private readonly ApplicationDbContext _context;
    private readonly ArticleService _articleService;

    public ArticleServiceTests()
    {
        // Usamos una base de datos en memoria para las pruebas
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        _context = new ApplicationDbContext(options);
        _articleService = new ArticleService(_context);
    }

    [Fact]
    public async Task GetArticlesAsync_ReturnsEmptyList_WhenNoArticlesExist()
    {
        // Act
        var result = await _articleService.GetArticlesAsync();

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task AddArticleAsync_SavesArticleAndReturnsIt()
    {
        // Arrange
        var newArticle = new Article { Id = 1, Title = "Test", Description = "Test Desc" };

        // Act
        var result = await _articleService.AddArticleAsync(newArticle);

        // Assert
        result.Should().NotBeNull();
        result.Title.Should().Be("Test");

        // Verificamos que se haya guardado correctamente en la base de datos
        var articleFromDb = _context.Articles.FirstOrDefault();
        articleFromDb.Should().NotBeNull();
        articleFromDb.Title.Should().Be("Test");
    }
}
