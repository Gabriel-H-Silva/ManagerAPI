using ManagerApi.Model;

namespace ManagerApi.Business.Interfaces
{
    public interface ICardBusiness
    {
        Card Create(Card card);

        Card FindById(long id);

        Task<List<Card>> GetAll();

        List<Card> GetAtivos();

        Card Update(Card card);

        void DeleteStatus(long id);

        void ReativarStatus(long id);

        void DeleteVip(long id);

        void ReativarVip(long id);
    }
}
