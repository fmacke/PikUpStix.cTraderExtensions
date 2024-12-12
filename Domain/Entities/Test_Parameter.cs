using Domain.Abstractions;
namespace Domain.Entities
{
    public partial class Test_Parameter : BaseEntity
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public int TestId { get; set; }
        public virtual Test Test { get; set; }
    }
}
