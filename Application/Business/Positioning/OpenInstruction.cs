using Domain.Enums;
using Domain.Entities;
using Application.Business.Positioning.Validation;

namespace Application.Business.Positioning
{
    public class OpenInstruction : PositionInstruction
    {
        private readonly IValidationService _validationService;

        [ValidateOpenInstruction]
        public Position Position { get; set; }

        public OpenInstruction(Position pos, IValidationService validationService) : base(pos, InstructionType.Open)
        {
            Position = pos;
            _validationService = validationService;
            _validationService.Validate(this);            
        }
    }
}
