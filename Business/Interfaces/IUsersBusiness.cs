using ManagerApi.DM;
using ManagerApi.Model;

namespace ManagerIO.Business.Interfaces
{
    public interface IUsersBusiness
    {
        ResultDM CreateNewUser(UsersDM user);
        List<Users> FindAll();
        ResultDM RemoveUser(long id);
        ResultDM UpdateUser(UsersDM user);
    }
}
