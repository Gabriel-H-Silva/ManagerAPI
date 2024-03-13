using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ManagerApi.Model
{
    [Table("tb_readers_settings")]
    public class ReadersSettings
    {
        [Key]
        [Column("id")]
        public long? Id { get; set; }

        [Column("code")]
        public string? Code { get; set; }

        [Column("display")]
        public string? Display { get; set; }

        [Column("light_color")]
        public string? Light_color { get; set; }

        [Column("price")]
        public double? Price { get; set;}

        [Column("price_vip")]
        public double? Price_vip { get; set; }

        [Column("status")]
        public string? Status { get; set; }

        [Column("toyId")]
        public long? ToysId { get; set; }

        public Toy? Toy { get; internal set; }   

    }
}
