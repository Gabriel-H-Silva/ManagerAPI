using Microsoft.EntityFrameworkCore;
using ManagerIO.Model.Context;
using System.Security.Cryptography;
using System.Text;
using ManagerApi.Model;
using ManagerApi.DM;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace ManagerIO.Repository.Generic
{
    public class UsersRepository
    {
        private MySQLContext _context;

        

        private DbSet<Users> _dbSet;

        public UsersRepository(MySQLContext context)
        {
            _context = context;
            _dbSet = _context.Set<Users>();
        }

        public Users ValidadeCredentials(Users user)
        {
  
            try
            {
                var pass = ComputeHash(user.Password, SHA256.Create());
                return _context.Users.FirstOrDefault(u =>
                (u.Name == user.Name) && (u.Password == pass) && (u.Status == "A"));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro Exceção: {ex}");
                return null;
            }
            
        }

        public Users ValidadeCredentials(string userName)
        {
            return _context.Users.SingleOrDefault(u => (u.Name == userName));
        }

        public bool RevokeToken(string userName)
        {
            var user = _context.Users.SingleOrDefault(u => (u.Name == userName));
            if (user is null) return false;
            user.RefreshToken = null;
            _context.SaveChanges();
            return true;
        }
        private object ComputeHash(string? password, HashAlgorithm algorithm)
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(password);
            byte[] hashedBytes = algorithm.ComputeHash(inputBytes);

            var builder = new StringBuilder();

            foreach (var item in hashedBytes)
            {
                builder.Append(item.ToString("x2"));
            }
            return builder.ToString();
        }

        public Users RefreshUserInfo(Users user)
        {
            if (!_context.Users.Any(u => u.Id.Equals(user.Id))) return null;

            var result = _context.Users.SingleOrDefault(p => p.Id.Equals(user.Id));
            if (result != null)
            {
                try
                {
                    _context.Entry(result).CurrentValues.SetValues(user);
                    _context.SaveChanges();
                    return result;
                }
                catch (Exception)
                {
                    throw;
                }
            }

            return result;
        }

        public List<Users> FindAll()
        {
            return _dbSet.Where(u => u.Status == "A").ToList();
        }

        public ResultDM Save(Users user)
        {
            ResultDM result = new ResultDM();
            try
            {
                var pass = ComputeHash(user.Password, SHA256.Create());

                user.Password = (string?)pass;
                _dbSet.Add(user);
                _context.SaveChanges();


                result.Message = "Usuario salvo com sucesso!";
                result.Status = "Sucess";

                return result;
            }
            catch (Exception)
            {
                result.Message = "Error";
                result.Status = "Error";

                return result;
            }

        }

        public ResultDM RemoveUserById(long Id)
        {
            ResultDM result = new ResultDM();

            try
            {
                var user = _context.Users.SingleOrDefault(u => (u.Id == Id));

                if (user != null)
                {

                    user.Status = "I";
                    _context.Entry(user).CurrentValues.SetValues(user.Status);
                    _context.SaveChanges();

                    result.Message = "Usuario desativado com sucesso";
                    result.Status = "Sucess";


                }
                else
                {
                    result.Message = "Error Para Salvar";
                    result.Status = "Sucess";
                }
            }
            catch(Exception e) {
                result.Message = "Error " + e;
                result.Status = "Error";
            }
            
            return result;
        }

        public ResultDM Update(UsersDM user)
        {
            ResultDM result = new ResultDM();
            if (!_context.Users.Any(u => u.Id.Equals(user.Id))) return null;

            var data = _context.Users.SingleOrDefault(p => p.Id.Equals(user.Id));
            
            if (data != null)
            {
                try
                {
                    string oldPassword = data.Password;

                    _context.Entry(data).CurrentValues.SetValues(user);
                    if (!string.IsNullOrEmpty(user.Password))
                    {
                        var pass = ComputeHash(user.Password, SHA256.Create());
                        data.Password = (string?)pass;

                    }
                    else
                    {
                        data.Password = oldPassword;
                    }
                    data.Status = "A";
                    _context.SaveChanges();

                    result.Message = "Usuario atualizado com sucesso";
                    result.Status = "Sucess";
                }
                catch (Exception e)
                {
                    result.Message = "Error " + e;
                    result.Status = "Error";
                }
            }

            return result;
        }
    }
}
