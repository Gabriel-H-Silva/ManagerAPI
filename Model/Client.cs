using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ManagerApi.Model
{
    [Table("tb_clients")]
    public class Client
    {
        [Key]
        [Column("id")]
        public long? Id { get; set; }

        [Column("cpf")]
        public long? cpf {  get; set; }

        [Column("name")]
        public string? name { get; set; }

        [Column("plays")]
        public long? plays { get; set; }

        [Column("balance")]
        public double? balance { get; set; }

        [Column("tickets")]
        public double? tickets { get; set; }

        [Column("email")]
        public string? email { get; set; }

        [Column("creation_date")]
        public DateTime creation_date { get; set; }

        [Column("telephone")]
        public string? telephone { get; set; }

        [Column ("cardId")]
        public long? cardId { get; set; }
        //public Card card { get; internal set; }

        public List<Card>? Cards { get; set; } 
    }
}
