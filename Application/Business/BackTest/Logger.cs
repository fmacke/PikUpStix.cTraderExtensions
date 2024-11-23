//using Application.Portfolio;
//using Domain.Enums;
//using Infrastructure.Contexts;

//namespace Application.BackTest
//{
//    public class Logger
//    {
//        public bool LoggingEnabled { get; private set; }

//        public Logger(bool loggingEnabled)
//        {
//            LoggingEnabled = loggingEnabled;
//        }

//        private void LogEvent(string eventDescription)
//        {
//            if (LoggingEnabled)
//                Console.WriteLine(eventDescription);
//        }

//        public void LogNewDay(DateTime cursorDate)
//        {
//            LogEvent("Date is " + cursorDate.ToShortDateString());
//        }

//        public void MethodError(string classMethodLocation, string errorMessage)
//        {
//            LogEvent("MethodError at: " + classMethodLocation + ",  Message: " + errorMessage);
//        }

//        public void CrossCheckMargins(int testId, decimal currentLiveMargin, DateTime cursorDate, decimal startingCapital)
//        {
//            if (LoggingEnabled)
//            {

//                ///todo: Refactor to move data call to infrastucture project - observe seperation of concerns
//                var db = new TraderDBContextDerived();
//                var trades = db.Test_Trades.Where(x => x.TestId == testId);
//                if (trades.Any(x => x.TestId == testId))
//                {
//                    decimal sumOfMarginsInDb =
//                        Math.Round(db.Test_Trades.Where(x => x.TestId == testId).Sum(x => x.Margin), 0);
//                    decimal sumOfMarginsLive = Math.Round(currentLiveMargin - startingCapital, 0);
//                    if (sumOfMarginsLive > sumOfMarginsInDb || sumOfMarginsLive < sumOfMarginsInDb)
//                        LogEvent("Margin disparity between system and db at " + cursorDate.ToShortDateString() + ", system margin = " + sumOfMarginsLive + "; db margin = " + sumOfMarginsInDb);
//                }
//            }
//        }

//        public void CrossCheckPositions(int testId, WeightedProposedPositions weightedPositions, DateTime cursorDate)
//        {
//            if (LoggingEnabled)
//            {
//                var positions =
//                    new TraderDBContextDerived().Test_Trades.Where(
//                        x => x.TestId == testId && x.Status == PositionStatus.POSITION.ToString());
//                foreach (var proposed in weightedPositions)
//                {
//                    if (proposed.ProposedWeightedPosition != 0)
//                    {
//                        if (positions.Any(x => x.InstrumentId == proposed.Instrument.InstrumentId))
//                        {
//                            var actualVolume =
//                                positions.Where(x => x.InstrumentId == proposed.Instrument.InstrumentId)
//                                    .Sum(x => x.Volume);
//                            if (proposed.ProposedWeightedPosition != actualVolume)
//                                LogEvent("Aaaargh!!  It's fucked!");
//                        }
//                    }
//                }
//            }
//        }
//    }
//}
