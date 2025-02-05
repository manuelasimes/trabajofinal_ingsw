using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BlogApi.Models; // Namespace del modelo

[ApiController]
[Route("api/article")]
public class ArticlesController : ControllerBase
{
    private readonly IArticleService _articleService;

    public ArticlesController(IArticleService articleService)
    {
        _articleService = articleService;
    }

[HttpGet]
public async Task<ActionResult<IEnumerable<Article>>> GetArticles()
{
    Console.WriteLine("Llamada a GetArticles"); // Agrega este log
    var articles = await _articleService.GetArticlesAsync();
    Console.WriteLine($"Se encontraron {articles.Count()} artículos"); // Log cantidad de artículos
    return Ok(articles);
}


    [HttpGet("{id}")]
    public async Task<ActionResult<Article>> GetArticle(int id)
    {
        var article = await _articleService.GetArticleByIdAsync(id);
        if (article == null) return NotFound();
        return Ok(article);
    }

    [HttpPost]
    public async Task<ActionResult<Article>> PostArticle(Article article)
    {
        var createdArticle = await _articleService.AddArticleAsync(article);
        return CreatedAtAction(nameof(GetArticle), new { id = createdArticle.Id }, createdArticle);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutArticle(int id, Article article)
    {
        var updated = await _articleService.UpdateArticleAsync(id, article);
        if (!updated) return NotFound();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteArticle(int id)
    {
        var deleted = await _articleService.DeleteArticleAsync(id);
        if (!deleted) return NotFound();
        return NoContent();
    }
[HttpPost("upload")]
public async Task<IActionResult> UploadFile(IFormFile file)
{
    if (file == null || file.Length == 0)
    {
        return BadRequest("No se ha proporcionado un archivo válido.");
    }

    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
    
    // Crear el directorio si no existe
    if (!Directory.Exists(uploadsFolder))
    {
        Directory.CreateDirectory(uploadsFolder);
    }

    var filePath = Path.Combine(uploadsFolder, file.FileName);

    using (var stream = new FileStream(filePath, FileMode.Create))
    {
        await file.CopyToAsync(stream);
    }

    var fileUrl = $"/images/{file.FileName}";
    return Ok(new { Url = fileUrl });
}

}
