using Domain.Entities;

namespace Application.Business.Calculations
{
    public class DailyExcursions
    {
        public DailyExcursions(IReadOnlyCollection<Position> results)
        {
            Excursions = new List<decimal>();
            foreach (Position result in results.OrderBy(x => x.ClosedAt))
            {
                var cumulativeMargin = results.Where(x => x.ClosedAt < result.ClosedAt).Sum(x => x.Margin);
                Excursions.Add(Convert.ToDecimal(result.Margin / cumulativeMargin));
            }

            if (results.Count > 0)
            {
                MaxFavourableExcursion = Math.Round(Excursions.Max(), 2);
                MaxAdverseExcursion = Math.Round(Excursions.Min(), 2);
            }
        }
        public decimal MaxAdverseExcursion { get; private set; }
        public decimal MaxFavourableExcursion { get; private set; }
        public List<decimal> Excursions { get; private set; }
    }
}