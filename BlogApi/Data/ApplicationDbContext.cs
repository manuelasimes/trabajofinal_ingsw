using Microsoft.EntityFrameworkCore;
using BlogApi.Models; // Asegúrate de incluir el namespace de los modelos

namespace BlogApi.Data
{
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Article> Articles { get; set; }
}
}
