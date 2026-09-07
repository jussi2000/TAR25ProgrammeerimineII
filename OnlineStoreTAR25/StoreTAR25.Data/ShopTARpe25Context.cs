
using Microsoft.EntityFrameworkCore;
using ShopTARpe25.Core.Domain;

namespace ShopTARpe25.Data
{
    //Teha sellest classist DbContext, et saaks andmebaasi kasutada
    public class ShopTARpe25Context : DbContext
    {
        public ShopTARpe25Context(DbContextOptions<ShopTARpe25Context> options)
            : base(options)
        {
        }
        //teha DbSet, et saaks andmebaasi kasutada
        //nimega Spaceship
        public DbSet<SpaceShip> Spaceships { get; set; }

    }
}
