using ManagerApi.Business.Interfaces;
using ManagerApi.Model;
using ManagerApi.Repository;


namespace ManagerApi.Business
{
    public class ToyBusiness : IToyBusiness
    {
       ToyRepository _repository;

        public ToyBusiness(ToyRepository repository)
        {
            _repository = repository;
        }

        public List<Toy> GetAll()
        {
            return _repository.Get();
        }

        public Toy FindById(long id)
        {
            return _repository.FindById(id);
        }

        public Toy Create(Toy toy)
        {
            return _repository.Create(toy);
        }

        public Toy Update(Toy toy)
        {
            return _repository.Update(toy);
        }
    }
}
