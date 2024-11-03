using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public partial class ErrorMessage
    {
        [Key]
        public int ErrorId { get; set; }

        [Required]
        [StringLength(2000)]
        public string Message { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime Logged { get; set; }

        [Required]
        [StringLength(500)]
        public string ClassName { get; set; }

        [Required]
        [StringLength(500)]
        public string MethodName { get; set; }
    }
}
