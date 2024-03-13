using ManagerApi.Model;

namespace ManagerApi.Business.Interfaces
{
    public interface IToyBusiness
    {
        Toy Create(Toy toy);

        Toy FindById(long id);

        List<Toy> GetAll();

        Toy Update(Toy toy);

    }
}
