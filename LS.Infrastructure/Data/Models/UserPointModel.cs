using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace LS.Infrastructure.Data.Models
{
    [Table("user_points")]
    [Index(nameof(UserId), Name = "ix_user_points_user_id")]
    public class UserPointModel
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("user_id")]
        public int UserId { get; set; }

        [Required]
        [Column("points")]
        public int Points { get; set; }

        [Required]
        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        public UserModel User { get; set; }
    }
}
