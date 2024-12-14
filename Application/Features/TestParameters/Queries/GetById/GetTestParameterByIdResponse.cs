using Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Application.Features.TestParameters.Queries.GetById
{
    public class GetTestParameterByIdResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public int TestId { get; set; }
        public virtual Test Test { get; set; }
    }
    
}
