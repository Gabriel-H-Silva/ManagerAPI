using ManagerApi.Model;

namespace ManagerApi.Business.Interfaces
{
    public interface IReadersSettingsBusiness
    {
        ReadersSettings Create(ReadersSettings readersSettings);

        ReadersSettings FindById(long id);

        List<ReadersSettings> GetAll();

        List<ReadersSettings> GetAtivos();

        ReadersSettings Update(ReadersSettings readersSettings);

        void DeleteStatus(long Id);

        void ReativarStatus(long Id);
    }
}
