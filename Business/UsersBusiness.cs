using ManagerApi.DM;
using ManagerApi.Model;
using ManagerIO.Business.Interfaces;
using ManagerIO.Model.Context;
using ManagerIO.Repository;
using ManagerIO.Repository.Generic;

namespace ManagerIO.Business
{
    public class UsersBusiness : IUsersBusiness
    {
        UsersRepository _repository;

        public UsersBusiness(UsersRepository repository)
        {
            _repository = repository;
        }


        public ResultDM CreateNewUser(UsersDM user)
        {
            ResultDM result = new ResultDM();

                Users sUser = new Users();
                sUser.Name = user.Name;
                sUser.Password = user.Password;
                sUser.Creation_date = DateTime.Now;
                sUser.Office = user.Office;
                sUser.Cpf = user.Cpf;
                sUser.Status = "A";

                result = _repository.Save(sUser);

                return result;
            
        }

        public List<Users> FindAll()
        {
            return _repository.FindAll();
        }

        public ResultDM RemoveUser(long id)
        {
            ResultDM result = _repository.RemoveUserById(id);

            return result;
        }

        public ResultDM UpdateUser(UsersDM user)
        {
                ResultDM result = new ResultDM();

                result = _repository.Update(user);

                return result;
        }
    }
}
