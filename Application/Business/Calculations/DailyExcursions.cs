using Domain.Entities;

namespace Application.Business.Calculations
{
    public class DailyExcursions
    {
        public DailyExcursions(IReadOnlyCollection<Position> results)
        {
            Excursions = new List<double>();
            var orderedResults = results.OrderBy(x => x.ClosedAt).ToList();

            double cumulativeMargin = 0; // Ensure it includes current position

            for (int i = 0; i < orderedResults.Count; i++)
            {
                cumulativeMargin += orderedResults[i].Margin; // Accumulate margin properly

                if (cumulativeMargin == 0)
                    continue;

                Excursions.Add(cumulativeMargin);
            }

            MaxFavourableExcursion = Excursions.Count > 0 ? Math.Round(Excursions.Max(), 2) : 0;
            MaxAdverseExcursion = Excursions.Count > 0 ? Math.Round(Excursions.Min(), 2) : 0;
        }

        public double MaxAdverseExcursion { get; private set; }
        public double MaxFavourableExcursion { get; private set; }
        public List<double> Excursions { get; private set; }
    }
}