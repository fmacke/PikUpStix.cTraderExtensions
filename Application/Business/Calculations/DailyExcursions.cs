using Domain.Entities;

namespace Application.Business.Calculations
{
    public class DailyExcursions
    {
        public DailyExcursions(IReadOnlyCollection<Position> results)
        {
            Excursions = new List<double>();
            var orderedResults = results.OrderBy(x => x.ClosedAt).ToList();
            for (int i = 0; i < orderedResults.Count; i++)
            {
                var cumulativeMargin = orderedResults.Take(i).Sum(x => x.Margin);
                if (cumulativeMargin == 0)
                    continue;
                Excursions.Add(orderedResults[i].Margin / cumulativeMargin);
            }

            if (Excursions.Count > 0)
            {
                MaxFavourableExcursion = Math.Round(Excursions.Max(), 2);
                MaxAdverseExcursion = Math.Round(Excursions.Min(), 2);
            }
            else
            {
                MaxFavourableExcursion = 0;
                MaxAdverseExcursion = 0;
            }
        }

        public double MaxAdverseExcursion { get; private set; }
        public double MaxFavourableExcursion { get; private set; }
        public List<double> Excursions { get; private set; }
    }

}