using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BlogApi.Data; // Namespace de tu DbContext
using BlogApi.Models; // Namespace de tu modelo

public class ArticleService : IArticleService
{
    private readonly ApplicationDbContext _context;

    public ArticleService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Article>> GetArticlesAsync()
    {
        return await _context.Articles.ToListAsync();
    }

    public async Task<Article?> GetArticleByIdAsync(int id)
    {
        return await _context.Articles.FindAsync(id);
    }

    public async Task<Article> AddArticleAsync(Article article)
    {
        _context.Articles.Add(article);
        await _context.SaveChangesAsync();
        return article;
    }

    public async Task<bool> UpdateArticleAsync(int id, Article article)
    {
        var existingArticle = await _context.Articles.FindAsync(id);
        if (existingArticle == null) return false;

        existingArticle.Title = article.Title;
        existingArticle.Description = article.Description;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteArticleAsync(int id)
    {
        var article = await _context.Articles.FindAsync(id);
        if (article == null) return false;

        _context.Articles.Remove(article);
        await _context.SaveChangesAsync();
        return true;
    }
}
