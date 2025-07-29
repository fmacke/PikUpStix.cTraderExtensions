namespace Domain.Enums
{
    public enum TimeFrame
    {
        Tick,
        M1,
        M5,
        M15,
        M30,
        H1,
        H4,
        D1,
        W1,
        MN1
    }

    public static class TimeFrameParser
    {
        private static readonly Dictionary<string, TimeFrame> mappings = new()
        {
            { "Tick", TimeFrame.Tick },
            { "M1", TimeFrame.M1 },
            { "M5", TimeFrame.M5 },
            { "M15", TimeFrame.M15 },
            { "15", TimeFrame.M15 },
            { "M30", TimeFrame.M30 },
            { "30", TimeFrame.M30 },
            { "H1", TimeFrame.H1 },
            { "H4", TimeFrame.H4 },
            { "D1", TimeFrame.D1 },
            { "Daily", TimeFrame.D1 },
            { "W1", TimeFrame.W1 },
            { "MN1", TimeFrame.MN1 }
        };
        public static bool TryParse(string input, out TimeFrame timeFrame)
        {
            return mappings.TryGetValue(input, out timeFrame);
        }
        public static bool TryParseBarFromTick(TimeFrame input, out TimeFrame timeFrame)
        {
            return barMappings.TryGetValue(input.ToString(), out timeFrame);
        }
        private static readonly Dictionary<string, TimeFrame> barMappings = new()
        {
            { "M1", TimeFrame.M5 },
            { "M5", TimeFrame.M15 },
            { "M15", TimeFrame.M30 },
            { "M30", TimeFrame.H1 },
            { "H1", TimeFrame.D1 },
            { "H4", TimeFrame.D1 },
            { "D1", TimeFrame.W1 },
            { "Daily", TimeFrame.W1 },
            { "W1", TimeFrame.MN1 },
            { "MN1", TimeFrame.MN1 }
        };
    }

}