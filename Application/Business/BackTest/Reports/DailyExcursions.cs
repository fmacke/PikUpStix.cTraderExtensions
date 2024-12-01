using Domain.Entities;

namespace Application.Business.BackTest.Reports
{
    public class DailyExcursions
    {
        public DailyExcursions(IReadOnlyCollection<Test_Trades> results)
        {
            Excursions = new List<decimal>();
            foreach (Test_Trades result in results)
            {
                //todo: this needs updated since Test_Results table was made redundant
                //Excursions.Add(Convert.ToDecimal(result.Margin / (result.CumulativeMargin - result.Margin)));
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