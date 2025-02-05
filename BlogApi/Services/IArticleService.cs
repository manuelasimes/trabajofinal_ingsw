using System.Collections.Generic;
using System.Threading.Tasks;
using BlogApi.Models; // Asegúrate de que el namespace coincida con el de tu modelo

public interface IArticleService
{
    Task<IEnumerable<Article>> GetArticlesAsync();
    Task<Article?> GetArticleByIdAsync(int id);
    Task<Article> AddArticleAsync(Article article);
    Task<bool> UpdateArticleAsync(int id, Article article);
    Task<bool> DeleteArticleAsync(int id);
}
