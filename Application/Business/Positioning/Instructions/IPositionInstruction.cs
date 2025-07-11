﻿using Domain.Entities;
using Domain.Enums;

namespace Application.Business.Positioning.Instructions
{
    public interface IPositionInstruction
    {
        InstructionType InstructionType { get; set; }
        Position Position { get; set; }
    }
}