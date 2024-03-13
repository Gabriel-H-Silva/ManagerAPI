using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ManagerApi.Model
{
    [Table("tb_users")]
    public class Users
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("name")]
        public string? Name { get; set; }

        [Column("creation_date")]
        public DateTime Creation_date { get; set; }

        [Column("office")]
        public string? Office { get; set; }

        [Column("password")]
        public string? Password { get; set; }

        [Column("refresh_token")]
        public string? RefreshToken { get; set; }

        [Column("refresh_token_expiry_time")]
        public DateTime? RefreshTokenExpiryTime { get; set; }

        [Column("status")]
        public string? Status { get; set; }

        [Column("cpf")]
        public string? Cpf { get; set; }

    }
}
