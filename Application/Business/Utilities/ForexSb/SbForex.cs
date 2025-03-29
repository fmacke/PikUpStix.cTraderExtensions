using Domain.Entities;
using System.Data.SqlClient;
using System.Globalization;

namespace Application.Business.Utilities.ForexSb
{
    public class HistoricalDataProcessor
    {
        private string _folderPath;
        private string _connectionString;

        public HistoricalDataProcessor(string folderPath, string connectionString)
        {
            _folderPath = folderPath;
            _connectionString = connectionString;
        }

        public void ProcessFiles()
        {
            var csvFiles = Directory.GetFiles(_folderPath, "*.csv");

            foreach (var file in csvFiles)
            {
                var records = ReadCsvFile(file);
                SaveToDatabase(records);
            }
        }

        private List<HistoricalDataRecord> ReadCsvFile(string filePath)
        {
            var records = new List<HistoricalDataRecord>();

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

        private void SaveToDatabase(Instrument instrument)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                foreach (var record in instrument.HistoricalDatas)
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
