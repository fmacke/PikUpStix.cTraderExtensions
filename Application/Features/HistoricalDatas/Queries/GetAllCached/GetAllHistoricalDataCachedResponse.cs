﻿using Application.Common.Results;
using MediatR;

namespace Application.Features.HistoricalDatas.Queries.GetAllCached
{
    public class GetAllHistoricalDataCachedResponse : IRequest<Result<int>>
    {
        public int Id { get; set; }
        public DateTime? Date { get; set; }
        public double OpenPrice { get; set; }
        public double ClosePrice { get; set; }
        public double LowPrice { get; set; }
        public double HighPrice { get; set; }
        public double Volume { get; set; }
        public double Settle { get; set; }
        public double OpenInterest { get; set; }
        public int InstrumentId { get; set; }
    }
}