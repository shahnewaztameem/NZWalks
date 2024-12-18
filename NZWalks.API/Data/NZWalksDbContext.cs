using Microsoft.EntityFrameworkCore;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Data
{
    public class NZWalksDbContext : DbContext
    {
        // db options pass to contructor
        public NZWalksDbContext(DbContextOptions dbContextOptions): base(dbContextOptions)
        {
            
        }

        // create DB set (property of db context class) - collection of entities
        // these 3 props will create tables inside new db
        public DbSet<Difficulty> Difficulties{ get; set; }

        public DbSet<Region> Regions{ get; set; }

        public DbSet<Walk> Walks { get; set; }

    }
}
