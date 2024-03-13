
using ManagerApi.Model;
using ManagerIO.Business.Interfaces;
using ManagerIO.Repository;
using ManagerIO.Repository.Generic;


namespace ManagerIO.Business
{
    public class ConfigBusiness : IConfigBusiness
    {
        ConfigRepository _repository;

        public ConfigBusiness(ConfigRepository repository)
        {
            _repository = repository;
        }

        public List<Config> FindAll()
        {
            return _repository.FindAll();
        }

        public Config Update(Config config)
        {
            return _repository.Update(config);
        }
    }
}
