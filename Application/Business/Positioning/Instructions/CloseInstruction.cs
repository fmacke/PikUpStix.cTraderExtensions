using Domain.Enums;
using Domain.Entities;
using Application.Business.Positioning.Validation;

namespace Application.Business.Positioning.Instructions
{
    public class CloseInstruction : PositionInstruction
    {
        private readonly IValidationService _validationService;

        [ValidateCloseInstruction]
        public double ClosePrice { get; set; }

        [ValidateCloseInstruction]
        public DateTime ClosedAt { get; set; }

        public CloseInstruction(Position pos, double closePrice, DateTime closedAt, IValidationService validationService) : base(pos, InstructionType.Close)
        {
            
            ClosePrice = closePrice;
            ClosedAt = closedAt;
            _validationService = validationService;
            _validationService.Validate(this);
        }
    }
}
