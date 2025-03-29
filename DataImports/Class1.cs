using Application.Features.Instruments.Queries.GetAllCached;
using DataServices;
using Domain.Entities;
using System.Globalization;

namespace DataImports
{
    public class HistoricalDataProcessor
    {
        private string _folderPath;
        private string _connectionString;
        private DataService _dataServices;
        private List<GetAllInstrumentsCachedResponse> _instruments;

        public HistoricalDataProcessor(string folderPath, string connectionString)
        {
            _folderPath = folderPath;
            _connectionString = connectionString;
            _dataServices = new DataService();
            var ins = _dataServices.InstrumentCaller.GetAllInstrumentsCachedAsync();
            _instruments = ins.Result;
        }

        public void ProcessFiles()
        {
            var csvFiles = Directory.GetFiles(_folderPath, "*.csv");

            foreach (var file in csvFiles)
            {
                var instrument = GetInstrumentFromFileName(file);   
                var records = ReadCsvFile(file);
                SaveToDatabase(records);
            }
        }

        private List<HistoricalData> ReadCsvFile(string filePath)
        {
            var records = new List<HistoricalData>();
  
            using (var reader = new StreamReader(filePath))
            {
                // Skip header line
                var header = reader.ReadLine();

                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var fields = line.Split(',');

                    var record = new HistoricalDataRecord
                    {
                        Date = DateTime.ParseExact(fields[0], "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
                        OpenPrice = double.Parse(fields[1]),
                        HighPrice = double.Parse(fields[2]),
                        LowPrice = double.Parse(fields[3]),
                        ClosePrice = double.Parse(fields[4])
                        // Additional fields can be populated as needed.
                    };

                    records.Add(record);
                }
            }

            return records;
        }

        private void SaveToDatabase(List<HistoricalDataRecord> records)
        {
           

            foreach (var record in records)
                {
                    var query = @"INSERT INTO [dbo].[HistoricalData] 
                            (Date, OpenPrice, HighPrice, LowPrice, ClosePrice) 
                            VALUES (@Date, @OpenPrice, @HighPrice, @LowPrice, @ClosePrice)";

                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Date", record.Date);
                        command.Parameters.AddWithValue("@OpenPrice", record.OpenPrice);
                        command.Parameters.AddWithValue("@HighPrice", record.HighPrice);
                        command.Parameters.AddWithValue("@LowPrice", record.LowPrice);
                        command.Parameters.AddWithValue("@ClosePrice", record.ClosePrice);

                        command.ExecuteNonQuery();
                    }
                }
            }
        }

        
    }
}
