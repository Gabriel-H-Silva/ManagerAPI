using ManagerApi.Model;

namespace ManagerApi.Business.Interfaces
{
    public interface IClientBusiness
    {
        Client Create(Client client);

        Client FindById(long id);

        List<Client> GetAll();

        Client Update(Client client);
    }
}
