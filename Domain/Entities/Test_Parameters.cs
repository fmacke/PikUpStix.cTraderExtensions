using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Domain.Entities
{
    public partial class Test_Parameters
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int TestId { get; set; }


        [Column(Order = 1)]
        [StringLength(200)]
        public string Name { get; set; }


        [Column(Order = 2)]
        [StringLength(200)]
        public string Value { get; set; }

        public virtual Test Test { get; set; }
    }
}
