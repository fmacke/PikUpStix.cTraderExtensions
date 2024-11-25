using cAlgo.API.Internals;

namespace Robots
{
    public abstract class CarverInjectTrendFollowercTrader : PikUpStixRobotRunner
    {
        /// <summary>
        /// Class to be run directly in FXPro - bridge to PikupStix.Trading strategies which loads the Carver Trend Following Strategy
        /// </summary>
        protected override void OnStart()
        { 
            var result = System.Diagnostics.Debugger.Launch();
            if (!result)
            {
                Print("Debugger launch failed");
            }
           // IsTestRun = false;
            base.OnStart();
        }
        protected override void OnBar()
        {
            var testParams = IsTestRun ? ResultsCapture?.TestParams : null;

            /// Inject the required IFxProStrategyWrapper here
            var x = new CarverTrendFollowerWrapper(Convert.ToDecimal(Account.Equity), Bars, Positions, 
                SymbolName, "FXPRO", Symbol.Ask, Symbol.Bid, testParams);
            ManagePositions(x);
            base.OnBar();
        }
    }
}

















