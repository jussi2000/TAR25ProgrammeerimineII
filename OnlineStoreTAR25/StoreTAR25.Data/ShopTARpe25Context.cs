
using Microsoft.EntityFrameworkCore;
using ShopTARpe25.Core.Domain;

namespace ShopTARpe25.Data;

public class LibraryContext : DbContext
{
    public LibraryContext(DbContextOptions<LibraryContext> options)
        : base(options)
    {
    }

    public DbSet<Book> Books { get; set; }
    public DbSet<Reader> Readers { get; set; }
    public DbSet<Loan> Loans { get; set; }

    // Andmebaasi ühendus seadistatakse Program.cs failis.
    // Ühenduse andmed asuvad appsettings.json failis.
    // UseSqlServer määrab, et kasutame SQL Serveri andmebaasi.
}
