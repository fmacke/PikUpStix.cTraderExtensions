using Domain.Entities;

namespace DataImports
{
    public interface IDataImport
    {
        void ClearExistingAndImportNewData(bool limitImport);
        void ImportLatestData(List<Instrument> instruments, DateTime pullDataFrom);
        void UpdateExchangeRates();
    }
}