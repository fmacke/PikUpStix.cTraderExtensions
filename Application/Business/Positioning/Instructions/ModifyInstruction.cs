using Domain.Enums;
using Domain.Entities;
using Application.Business.Positioning.Validation;

namespace Application.Business.Positioning.Instructions
{
    public class ModifyInstruction : PositionInstruction
    {
        private readonly IValidationService _validationService;

        [ValidateModifyInstruction]
        public double AdjustStopLossTo { get; }

        [ValidateModifyInstruction]
        public double AdjustTakeProfitTo { get; }

        public Position Position { get; set; }

        public ModifyInstruction(Position pos, double adjustStopLossTo, double adjustTakeProfitTo, IValidationService validationService) : base(pos, InstructionType.Modify)
        {           
            AdjustStopLossTo = adjustStopLossTo;
            AdjustTakeProfitTo = adjustTakeProfitTo;
            Position = pos;
            _validationService = validationService;
            _validationService.Validate(this);
        }
    }

}
