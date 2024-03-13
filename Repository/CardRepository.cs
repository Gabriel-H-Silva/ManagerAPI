using ManagerApi.Model;
using ManagerIO.Model.Context;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using System;
using System.Globalization;

namespace ManagerApi.Repository
{
    public class CardRepository
    {
        private MySQLContext _context;

        private DbSet<Card> _dbSet;

        private string dateFormated = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        public CardRepository(MySQLContext context)
        {
            _context = context;
            _dbSet = _context.Set<Card>();
        }
        public async Task<List<Card>> GetAll()
        //Metodo para consultar todos os cartoes
        {

            var results = _dbSet
                .Join(
                    _context.Clients,
                    card => card.client!.Id,
                    client => client.Id,
                    (card, client) => new { Card = card, Client = client }
                )
                .Select(joined => new Card
                {
                    Id = joined.Card.Id,
                    number = joined.Card.number,
                    creation_date = joined.Card.creation_date,
                    vip = joined.Card.vip,
                    bonus = joined.Card.bonus,
                    status = joined.Card.status,
                    date_active = joined.Card.date_active,
                    date_desactive = joined.Card.date_desactive,
                    clientId = joined.Card.clientId,
                    client = new Client
                    {
                        Id = joined.Client.Id,
                        name = joined.Client.name,
                        plays = joined.Client.plays,
                        balance = joined.Client.balance,
                        tickets = joined.Client.tickets,
                    }
                })
                .ToList();

            return results;

        }

        public List<Card> GetAtivos()
        //Metodo para consultados apenas cartoes ativos, ativos = A
        {
            var cards = _dbSet.Where(c => c.status == "A")
                .Join(
                    _context.Clients,
                    card => card.client!.Id,
                    client => client.Id,
                    (card, client) => new { Card = card, Client = client }
                )
                .Select(joined => new Card
                {
                    Id = joined.Card.Id,
                    number = joined.Card.number,
                    creation_date = joined.Card.creation_date,
                    vip = joined.Card.vip,
                    bonus = joined.Card.bonus,
                    status = joined.Card.status,
                    date_active = joined.Card.date_active,
                    date_desactive = joined.Card.date_desactive,
                    clientId = joined.Card.clientId,
                    client = new Client
                    {
                        Id = joined.Client.Id,
                        name = joined.Client.name,
                        plays = joined.Client.plays,
                        balance = joined.Client.balance,
                        tickets = joined.Client.tickets,
                    }
                })
                .ToList();

            return cards;
        }

        public Card FindById(long Id)
        //Metodo para consulta pelo Id, tb_card.id
        {
            var idSelected = _dbSet.SingleOrDefault(p => p.Id.Equals(Id));
            return idSelected;
        }

        public Card Create(Card card)
        //Metodo para criar um cartao
        {
            try
            {
    
                var creationDate = DateTime.ParseExact(dateFormated, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                card.creation_date = creationDate;

                _dbSet.Add(card);
                _context.SaveChanges();
                return card;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Card Update(Card card)
        //Metodo para inserir novos valores nos campos da tabela tb_card
        {
            var result = _dbSet.SingleOrDefault(p => p.Id.Equals(card.Id));
            if (result != null)
            {
                try
                {
                    _context.Entry(result).CurrentValues.SetValues(card);
                    _context.SaveChanges();

                }
                catch (Exception)
                {
                    throw;
                }
            }
            return card;
             
           
        }

        public void ReativarStatus(long id)
        //Metodo para reativar um cartao passando o id, muda de inativo para ativo, tb_card.status = 'I'
        {
            var card = _dbSet.SingleOrDefault(p => p.Id == id);

            var dateActive = DateTime.ParseExact(dateFormated, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

            if (card != null)
            {
                if (card.status == "I")
                {
                    card.status = "A";
                    card.date_active = dateActive;
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

        public void DeleteStatus(long id)
        //Metodo para desativar um cartao, não exclui cartao,
        //passando o id, muda de ativo para inativo, tb_card.status = 'I'
        {
            var dateDesactive = DateTime.ParseExact(dateFormated, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

            Card card = _dbSet.SingleOrDefault(p => p.Id == id);

            if (card != null)
            {
                if (card.status == "A")
                {
                    card.status = "I";
                    card.date_desactive = dateDesactive;
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


        public void ReativarVip(long id)
        //Metodo para reativar um cartao passando o id, muda de inativo para ativo, tb_card.vip = 'A'
        {
            var card = _dbSet.SingleOrDefault(p => p.Id == id);

            var dateActive = DateTime.ParseExact(dateFormated, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

            if (card != null)
            {
                if (card.vip == "I")
                {
                    card.vip = "A";
                    card.date_active = dateActive;
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

        public void DeleteVip(long id)
        //Metodo para desativar o vip do cartao, não exclui o vip,
        //passando o id, muda de ativo para inativo, tb_card.vip = 'I'
        {
            var dateDesactive = DateTime.ParseExact(dateFormated, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

            Card card = _dbSet.SingleOrDefault(p => p.Id == id);

            if (card != null)
            {
                if (card.vip == "A")
                {
                    card.vip = "I";
                    card.date_desactive = dateDesactive;
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
