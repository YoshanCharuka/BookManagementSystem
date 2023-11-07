using Microsoft.EntityFrameworkCore;

namespace BookMgtSystem.Models
{
    public class BookAPIDBContext : DbContext
    {
        public BookAPIDBContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Book> Books { get; set; }

    }
}
