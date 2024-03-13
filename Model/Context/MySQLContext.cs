using ManagerApi.Model;
using Microsoft.EntityFrameworkCore;

namespace ManagerIO.Model.Context
{
    public class MySQLContext : DbContext
    {
        public MySQLContext() { }
        public MySQLContext(DbContextOptions<MySQLContext> options) : base(options) { }
        public DbSet<Users> Users { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Config> Config { get; set; }
        public DbSet<ReadersSettings> ReadersSettings { get; set; }
        public DbSet<Toy> Toys { get; set; }

    }
}
