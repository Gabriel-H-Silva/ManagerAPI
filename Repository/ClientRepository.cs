using ManagerApi.Model;
using ManagerIO.Model.Context;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace ManagerApi.Repository
{
    public class ClientRepository
    {
        private MySQLContext _context;

        private DbSet<Client> _dbSet;

        private string dateFormated = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        public ClientRepository(MySQLContext context)
        {
            _context = context;
            _dbSet = _context.Set<Client>();
        }
        public List<Client> GetAll()
        //Metodo para consultar todos os usuarios
        {
            var clients = _dbSet
                .Include(client => client.Cards)
                .Select(client => new Client
                    {
                        Id = client.Id,
                        cpf = client.cpf,
                        name = client.name,
                        plays = client.plays,
                        balance = client.balance,
                        tickets = client.tickets,
                        email = client.email,
                        creation_date = client.creation_date,
                        telephone = client.telephone,
                        cardId = client.cardId,
                        Cards = client.Cards.Select(card => new Card
                    {
                        number = card.number,
                        vip = card.vip,
                        bonus = card.bonus,
                        status = card.status,
                    }).ToList()
                    })
                .ToList();

            return clients;

        }

        public Client FindById(long Id)
        {
            var idSelected = _dbSet.SingleOrDefault(p => p.Id.Equals(Id));
            return idSelected;
        }

        public Client Create(Client client)
        //Metodo para criar um usuario
        {
            try
            {
    
                var creationDate = DateTime.ParseExact(dateFormated, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                client.creation_date = creationDate;

                _dbSet.Add(client);
                _context.SaveChanges();
                return client;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Client Update(Client client)
        //Metodo para inserir novos valores nos campos da tabela tb_card
        {
            var result = _dbSet.SingleOrDefault(p => p.Id.Equals(client.Id));
            if(result != null)
            {
                try
                {
                    _context.Entry(result).CurrentValues.SetValues(client);
                    _context.SaveChanges();
                } 
                catch (Exception)
                {
                    throw;
                }
            }
             
            return result;
        }

    }
}
