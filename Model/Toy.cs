using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;


namespace ManagerApi.Model
{
    [Table("tb_toys")]
    public class Toy
    {
        [Key]
        [Column("id")]
        public long? Id { get; set; }

        [Column("code")]
        public long? Code { get; set; }

        [Column("name")]
        public string? Name { get; set; }

    }
}
