using Domain.Enums;
using Domain.Entities;

namespace Robots.Common
{
    public class PositionUpdate
    {
        public Position Position { get; set; }
        public InstructionType InstructionType { get; set; }
        //public double Forecast { get; set; }

        public PositionUpdate(Position pos, InstructionType instruction)//, double forecast)
        {
            Position = pos;
            InstructionType = instruction;
            //Forecast = forecast;
        }
    }
}
