using ManagerApi.Model;
using ManagerIO.Model.Context;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace ManagerApi.Repository
{
    public class ToyRepository
    {
        private MySQLContext _context;

        private DbSet<Toy> _dbSet;

        public ToyRepository(MySQLContext context)
        {
            _context = context;
            _dbSet = _context.Set<Toy>();
        }
        public List<Toy> Get()
        //Metodo para consultar todos os brinquedos
        {
            var results = _dbSet.ToList();

            return results;

        }

        public Toy FindById(long Id)
        //Metodo para consulta pelo Id, tb_toys.id
        {
            var idSelected = _dbSet.SingleOrDefault(p => p.Id.Equals(Id));
            return idSelected;
        }

        public Toy Create(Toy toy)
        //Metodo para criar um brinquedo
        {
            try
            {
                _dbSet.Add(toy);
                _context.SaveChanges();
                return toy;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Toy Update(Toy toy)
        //Metodo para inserir novos valores nos campos da tabela tb_toys
        {
            var result = _dbSet.SingleOrDefault(p => p.Id.Equals(toy.Id));
            if (result != null)
            {
                try
                {
                    _context.Entry(result).CurrentValues.SetValues(toy);
                    _context.SaveChanges();

                }
                catch (Exception)
                {
                    throw;
                }
            }
            return toy;
             
           
        }

    }
}
