using ManagerApi.Business.Interfaces;
using ManagerApi.Model;
using ManagerApi.Repository;
using ManagerIO.Configurations;
using ManagerIO.Model.Context;
using ManagerIO.Repository;
using ManagerIO.Repository.Generic;
using ManagerIO.Services;
using Microsoft.Extensions.Configuration;


namespace ManagerApi.Business
{
    public class CardBusiness : ICardBusiness
    {
        CardRepository _repository;

        public CardBusiness(CardRepository repository)
        {
            _repository = repository;
        }

        public Task<List<Card>> GetAll()
        {
            return _repository.GetAll();
        }
        public List<Card> GetAtivos()
        {
            return _repository.GetAtivos();
        }

        public Card FindById(long id)
        {
            return _repository.FindById(id);
        }

        public Card Create(Card card)
        {
            return _repository.Create(card);
        }

        public Card Update(Card card)
        {
            return _repository.Update(card);
        }
        public void DeleteStatus(long id)
        {
            _repository.DeleteStatus(id);
        }

        public void ReativarStatus(long id)
        {
            _repository.ReativarStatus(id);
        }

        public void DeleteVip(long id)
        {
            _repository.DeleteVip(id);
        }

        public void ReativarVip(long id)
        {
            _repository.ReativarVip(id);
        }
    }
}
