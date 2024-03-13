using ManagerApi.Business.Interfaces;
using ManagerApi.Model;
using ManagerApi.Repository;


namespace ManagerApi.Business
{
    public class ReadersSettingsBusiness : IReadersSettingsBusiness
    {
       ReadersSettingsRepository _repository;

        public ReadersSettingsBusiness(ReadersSettingsRepository repository)
        {
            _repository = repository;
        }

        public List<ReadersSettings> GetAll()
        {
            return _repository.GetAll();
        }

        public List<ReadersSettings> GetAtivos()
        {
            return _repository.GetAtivos();
        }

        public ReadersSettings FindById(long id)
        {
            return _repository.FindById(id);
        }

        public ReadersSettings Create(ReadersSettings readersSettings)
        {
            return _repository.Create(readersSettings);
        }

        public ReadersSettings Update(ReadersSettings readersSettings)
        {
            return _repository.Update(readersSettings);
        }
        public void DeleteStatus(long Id)
        {
            _repository.DeleteStatus(Id);
        }

        public void ReativarStatus(long Id)
        {
            _repository.ReativarStatus(Id);
        }
    }
}
