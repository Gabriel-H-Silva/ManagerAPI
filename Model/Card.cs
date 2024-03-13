using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ManagerApi.Model
{
    [Table("tb_card")]
    public class Card
    {
        [Key]
        [Column("id")]
        public long? Id { get; set; }

        [Column("number")]
        public string? number { get; set; }

        [Column("creation_date")]
        public DateTime? creation_date { get; set; }

        [Column("vip")]
        public string? vip { get; set; }
        
        [Column("bonus")]
        public double? bonus { get; set; }

        [Column("status")]
        public string? status { get; set; }

        [Column("date_desactive")]
        public DateTime? date_desactive { get; set; }

        [Column("date_active")]
        public DateTime? date_active { get; set; }

        [Column("clientId")]
        public long? clientId { get; set; }

        public Client? client { get; internal set; }  
    }
}
