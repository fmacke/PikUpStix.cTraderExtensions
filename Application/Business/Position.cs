using Domain.Entities;
using Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Application.Business
{
    public class Positions : List<Position>
    {
        public Positions()
        {
        }

        public Positions(List<Position> positions)
        {
            foreach (var position in positions)
            {
                Add(new Position());
            }
        }

        public Position GetPosition(int id)
        {
            return this.First(x => x.Id == id);
        }

        public void AddPosition(Position position)
        {
            Add(position);
        }

        public void RemovePosition(Position position)
        {
            Remove(position);
        }

        public void RemovePosition(int tradeId)
        {
            Remove(GetPosition(tradeId));
        }

        public void UpdatePosition(Position position)
        {
            var pos = GetPosition(position.Id);
            pos = position;
        }

        public void UpdatePosition(int tradeId, Position position)
        {
            var pos = GetPosition(tradeId);
            pos = position;
        }

        public bool PositionExists(int id)
        {
            return this.Any(x => x.Id == id);
        }

        public bool PositionExists(Position position)
        {
            return this.Any(x => x.Id == position.Id);
        }
    }
    [NotMapped]
    public class Position 
    {
        public int Id { get; set; }
        public string SymbolName {get; set;}
        public decimal StopLoss { get; set; } = 0;
        public decimal TakeProfit { get; set; } = 0;
        public decimal Volume { get; set; } = 0;
        public PositionStatus Status { get; set; } = PositionStatus.POSITION;
        public TradeType TradeType { get; set; } 
        public decimal EntryPrice { get; set; } = 0;
        public decimal ClosePrice { get; set; } = 0;
        public decimal Margin { get; set; } = 0;
        public decimal TrailingStop { get; set; } = 0;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
//public void ClosePosition(decimal exchangeRate, decimal closePrice, DateTime closeDateTime)
        //{
        //    //Test_Trades trade = db.Test_Trades.First(x => x.TradeId == TradeId);
        //    ClosePrice = closePrice;
        //    ClosedAt = closeDateTime;
        //    Status = PositionStatus.HISTORICALTRADE.ToString();
        //    Margin = BackTest.Reports.Margin.Calculate(Instrument.ContractUnit, exchangeRate, this, closePrice);
        //    //trade.ClosePrice = closePrice;
        //    //trade.ClosedAt = closeDateTime;
        //    //trade.Status = Status;
        //    //trade.Margin = Margin;
        //    //db.Test_Trades.AddOrUpdate(trade);
        //    //db.SaveChanges();
        //}

        //public void UpdateMargin(decimal exchangeRate, decimal latestPrice)
        //{
        //    //Test_Trades trade = db.Test_Trades.First(x => x.TradeId == TradeId);
        //    Margin = BackTest.Reports.Margin.Calculate(Instrument.ContractUnit, exchangeRate, this, latestPrice);
        //    //trade.Margin = Margin;
        //    //db.Test_Trades.AddOrUpdate(trade);
        //    //db.SaveChanges();
        //}

        //public void UpdateStopLoss(decimal stopLoss)
        //{
        //Test_Trades trade = db.Test_Trades.First(x => x.TradeId == TradeId);
        //StopLoss = stopLoss;
        //trade.StopLoss = StopLoss;
        //db.Test_Trades.AddOrUpdate(trade);
        //db.SaveChanges();
        //}

        public bool PositionHasStopLoss()
        {
            return StopLoss > 0;
        }

        public bool PositionHasProfitTarget()
        {
            return TakeProfit > 0;
        }

        public bool PositionIsOpen()
        {
            return Status.Equals(PositionStatus.POSITION.ToString());
        }
    }
}