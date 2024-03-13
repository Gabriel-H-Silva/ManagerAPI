
using ManagerApi.Model;

namespace ManagerIO.Business.Interfaces
{
    public interface IConfigBusiness
    {
        List<Config> FindAll();

        Config Update(Config config);

    }
}
