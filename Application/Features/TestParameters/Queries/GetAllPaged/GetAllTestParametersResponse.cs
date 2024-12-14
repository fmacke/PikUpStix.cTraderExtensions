using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Domain.Entities;

namespace Application.Features.TestParameters.Queries.GetAllPaged
{
    public class GetAllTestParametersResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public int TestId { get; set; }
        public virtual Test Test { get; set; }
    }
}