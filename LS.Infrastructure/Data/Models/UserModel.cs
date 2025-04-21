using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LS.Infrastructure.Data.Models
{
    [Table("users")]
    public class UserModel
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("name")]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [Column("total_points")]
        public int TotalPoints { get; set; }
    }
}
