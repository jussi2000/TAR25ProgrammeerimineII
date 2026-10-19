using Microsoft.EntityFrameworkCore;
using ShopTARpe25.Core.Domain;
using StoreTARpe25.Core.Domain;

namespace StoreTAR25.Data
{
    public class ShopTARpe25Context : DbContext
    {
        public ShopTARpe25Context(DbContextOptions<ShopTARpe25Context> options)
            : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<Reader> Readers { get; set; }
        public DbSet<Loan> Loans { get; set; }
    }
}