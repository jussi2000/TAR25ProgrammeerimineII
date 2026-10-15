using Microsoft.EntityFrameworkCore;
using ShopTARpe25.Core.Domain;
using System.Xml;

namespace ShopTARpe25.Data
{
    public class ShopTARpe25Context : DbContext
    {
        public ShopTARpe25Context(DbContextOptions<ShopTARpe25Context> options)
            : base(options)
        {
        }

        public DbSet<Kindergarden> Kindergardens { get; set; }
        public DbSet<ExistingFilePath> ExistingFilePath { get; set; }
        public DbSet<FileToApi> FilesToApi { get; set; }
    }
}