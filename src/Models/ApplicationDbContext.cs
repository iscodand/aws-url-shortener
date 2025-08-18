using Microsoft.EntityFrameworkCore;

namespace UrlShortener.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
            : base(options)
        { }

        public DbSet<ShortUrl> ShortUrls { get; set; }
    }
}
