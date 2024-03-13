using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ManagerApi.Model
{
    [Table("tb_config")]
    public class Config
    {
        [Key]
        [Column("id")]
        public long? Id { get; set; }
        
        [Column("versao")]
        public string? Versao { get; set; }

        [Column("client")]
        public string? Client { get; set; }

        [Column("status")]
        public string? Status { get; set; }

        [Column("contract_expiration")]
        public DateTime? ContractExpiration { get; set; }

        [Column("client_create")]
        public DateTime? ClientCreate { get; set; }

    }
}
