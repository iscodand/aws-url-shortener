using Microsoft.EntityFrameworkCore;
using UrlShortener.Models;

namespace UrlShortener
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
            : base(options)
        { }

        public DbSet<ShortUrl> ShortUrls { get; set; }
    }
}
