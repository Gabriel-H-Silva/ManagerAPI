using ManagerApi.DM;

namespace ManagerIO.Business.Interfaces
{
    public interface ILoginBusiness
    {
        TokenDM ValidateCredentials(UsersDM user);

        TokenDM ValidateCredentials(TokenDM token);
        public bool RevokeToken(string userName);
    }
}
