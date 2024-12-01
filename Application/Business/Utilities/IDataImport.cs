using Domain.Entities;

namespace PikUpStix.Trading.Data
{
    public interface IDataImport
    {
        void ClearExistingAndImportNewData(bool limitImport);
        void ImportLatestData(List<Instrument> instruments, DateTime pullDataFrom);
        void UpdateExchangeRates();
    }
}