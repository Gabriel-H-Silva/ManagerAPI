using ManagerIO.Model.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace ManagerApi.DM
{
    public class UsersDM
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Office { get; set; }
        public string? Password { get; set; }
        public string? Cpf { get; set; }
        public string? Status { get; set; }

    }
}
