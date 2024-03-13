using ManagerApi.Model;
using ManagerIO.Model.Context;
using Microsoft.EntityFrameworkCore;

namespace ManagerApi.Repository
{
    public class ReadersSettingsRepository
    {
        private MySQLContext _context;

        private DbSet<ReadersSettings> _dbSet;

        public ReadersSettingsRepository(MySQLContext context)
        {
            _context = context;
            _dbSet = _context.Set<ReadersSettings>();
        }
        public List<ReadersSettings> GetAll()
        //Metodo para consultar todos as leitoras
        {
            var results = _dbSet
                .Join(
                    _context.Toys,
                    readersSettings => readersSettings.Toy!.Id,
                    toy => toy.Id,
                    (readersSettings, toy) => new { ReadersSettings = readersSettings, Toy = toy }
                )
                .Select(joined => new ReadersSettings
                {
                    Id = joined.ReadersSettings.Id,
                    Code = joined.ReadersSettings.Code,
                    Display = joined.ReadersSettings.Display,
                    Light_color = joined.ReadersSettings.Light_color,
                    Price = joined.ReadersSettings.Price,
                    Price_vip = joined.ReadersSettings.Price_vip,
                    Status = joined.ReadersSettings.Status,
                    ToysId = joined.ReadersSettings.ToysId,
                    Toy = new Toy
                    {
                        Id = joined.Toy.Id,
                        Code = joined.Toy.Code,
                        Name = joined.Toy.Name,
                    }
                })
                .ToList();

            return results;

        }

        public List<ReadersSettings> GetAtivos()
        //Metodo para consultar apenas leitoras ativos, ativos = A
        {
            var readersSettings = _dbSet.Where(p => p.Status == "A")
                .Join(
                    _context.Toys,
                    readersSettings => readersSettings.Toy!.Id,
                    toy => toy.Id,
                    (readersSettings, toy) => new { ReadersSettings = readersSettings, Toy = toy }
                )
                .Select(joined => new ReadersSettings
                {
                    Id = joined.ReadersSettings.Id,
                    Code = joined.ReadersSettings.Code,
                    Display = joined.ReadersSettings.Display,
                    Light_color = joined.ReadersSettings.Light_color,
                    Price = joined.ReadersSettings.Price,
                    Price_vip = joined.ReadersSettings.Price_vip,
                    Status = joined.ReadersSettings.Status,
                    ToysId = joined.ReadersSettings.ToysId,
                    Toy = new Toy
                    {
                        Id = joined.Toy.Id,
                        Code = joined.Toy.Code,
                        Name = joined.Toy.Name,
                      
                    }
                })
                .ToList();

            return readersSettings;
        }

        public ReadersSettings FindById(long Id)
        //Metodo para consulta pelo Id, tb_readers_settings.id
        {
            var idSelected = _dbSet.SingleOrDefault(p => p.Id.Equals(Id));
            return idSelected;
        }

        public ReadersSettings Create(ReadersSettings readersSettings)
        //Metodo para criar uma leitora
        {
            try
            {
                _dbSet.Add(readersSettings);
                _context.SaveChanges();
                return readersSettings;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public ReadersSettings Update(ReadersSettings readersSettings)
        //Metodo para inserir novos valores nos campos da tabela tb_readers_settings
        {
            var result = _dbSet.SingleOrDefault(p => p.Id.Equals(readersSettings.Id));
            if (result != null)
            {
                try
                {
                    _context.Entry(result).CurrentValues.SetValues(readersSettings);
                    _context.SaveChanges();

                }
                catch (Exception)
                {
                    throw;
                }
            }
            return readersSettings;
             
           
        }

        public void ReativarStatus(long Id)
        //Metodo para reativar um cartao passando o id, muda de inativo para ativo, tb_card.status = 'A'
        {
            var readersSettings = _dbSet.SingleOrDefault(p => p.Id == Id);

            if (readersSettings != null)
            {
                if (readersSettings.Status == "I")
                {
                    readersSettings.Status = "A";
                    try
                    {
                        _context.SaveChanges();
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }
        }

        public void DeleteStatus(long Id)
        //Metodo para desativar uma leitora, não exclui leitora,
        //passando o id, muda de ativo para inativo, tb_readers_settings.status = 'I'
        {
            ReadersSettings readersSettings = _dbSet.SingleOrDefault(p => p.Id == Id);

            if (readersSettings != null)
            {
                if (readersSettings.Status == "A")
                {
                    readersSettings.Status = "I";
                    try
                    {
                        _context.SaveChanges();
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }
        }

    }
}
