using System.Diagnostics;

namespace Application.Common.Utilities
{
    public static class MethodTimer
    {
        public static TimeSpan MeasureExecutionTime(Action action)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            action.Invoke();
            stopwatch.Stop();
            return stopwatch.Elapsed;
        }
    }
}
