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
    public class ClientBusiness : IClientBusiness
    {
        ClientRepository _repository;

        public ClientBusiness(ClientRepository repository)
        {
            _repository = repository;
        }

        public List<Client> GetAll()
        {
            return _repository.GetAll();
        }

        public Client FindById(long id)
        {
            return _repository.FindById(id);
        }

        public Client Create(Client client)
        {
            return _repository.Create(client);
        }

        public Client Update(Client client)
        {
            return _repository.Update(client);
        }
    }
}
