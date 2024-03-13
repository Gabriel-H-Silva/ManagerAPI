using ManagerApi.Model;
using ManagerIO.Model.Context;
using Microsoft.EntityFrameworkCore;

namespace ManagerIO.Repository.Generic
{
    public class ConfigRepository
    {
        private MySQLContext _context;

        private DbSet<Config> _dbSet;

        public ConfigRepository(MySQLContext context)
        {
            _context = context;
            _dbSet = _context.Set<Config>();
        }
        public List<Config> FindAll()
        {
            return _dbSet.ToList();
        }

        public Config Update(Config config)
        {
            var result = _dbSet.SingleOrDefault(p => p.Versao.Equals(config.Versao));
            if (result != null)
            {
                try
                {

                    config.Id = result.Id;
                    if (config.Status == "W")
                    {
                        config.Status = "A";
                    }

                    config.ClientCreate = result.ClientCreate;
                    _context.Entry(result).CurrentValues.SetValues(config);
                    _context.SaveChanges();

                }
                catch (Exception)
                {
                    throw;
                }
            }
            return config;
        }
    }
}
