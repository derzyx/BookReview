using BookReview.ClassLibrary;
using Microsoft.EntityFrameworkCore;

namespace BookReview.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<User> User { get; set; }
        public DbSet<Book> Book { get; set; }
    }
}
