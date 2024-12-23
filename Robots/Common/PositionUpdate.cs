using Application.Business;
using Domain.Enums;
using Robots.Interfaces;

namespace Robots.Common
{
    public class PositionUpdate
    {
        public Position Position { get; set; }
        public double? NewStopLoss { get; set; }
        public double? TakeProfit { get; set; }
        public InstructionType InstructionType { get; set; }
        public TradeType TradeType { get; set; }
        public double Volume { get; set; }
        public double Forecast { get; set; }

        public PositionUpdate(Position pos, double? stopLoss, double? takeProfit, InstructionType instruction, double volume, double forecast)
        {
            Position = pos;
            NewStopLoss = stopLoss;
            TakeProfit = takeProfit;
            InstructionType = instruction;
            Volume = volume;
            Forecast = forecast;
        }
        public PositionUpdate(TradeType tradeType, double? stopLoss, double? takeProfit, InstructionType instruction, double volume, double forecast)
        {
            TradeType = tradeType;
            NewStopLoss = stopLoss;
            TakeProfit = takeProfit;
            InstructionType = instruction;
            Volume = volume;
            Forecast = forecast;
        }


    }
}
