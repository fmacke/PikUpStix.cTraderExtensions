﻿using Domain.Entities;
using System.Globalization;
using DataServices;

namespace DataImports.ForexSb
{
    public class HistoricalDataProcessor
    {
        private string _folderPath;
        private DataService  _dataService = new DataService();

        public HistoricalDataProcessor(string folderPath)
        {
            _folderPath = folderPath;
        }

        public void ProcessFiles()
        {
            var csvFiles = Directory.GetFiles(_folderPath, "*.csv");
            var dataSource = Path.GetFileName(_folderPath.TrimEnd(Path.DirectorySeparatorChar));
            foreach (var file in csvFiles)
            {
                var records = ReadCsvFile(file);
                var frequency = GetFrequency(file);
                string instrumentName = Path.GetFileNameWithoutExtension(file).Split('_')[0];

                var x = 1;
                _dataService.InstrumentCaller.AddOrUpdateInstrument(new Instrument
                {
                    InstrumentName = instrumentName,
                    DataSource = dataSource,
                    DataName = instrumentName,
                    Provider = dataSource,
                    Frequency = frequency,
                    Format = "Bar",
                    ContractUnit = 0.0001,
                    ContractUnitType = "CHECK",
                    Currency = "CHECK",
                    HistoricalDatas = records,
                    Sort = "Ascending",
                    PriceQuotation = "CHECK",
                });
            }
        }

        private string GetFrequency(string filePath)
        {
            int lastDotIndex = filePath.LastIndexOf('.');
            string beforeExtension = filePath.Substring(0, lastDotIndex);
            return beforeExtension.Substring(beforeExtension.Length - 2);
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
                    var fields = line.Split('\t');
                    var record = new HistoricalData
                    {
                        Date = DateTime.ParseExact(fields[0], "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
                        OpenPrice = double.Parse(fields[1]),
                        HighPrice = double.Parse(fields[2]),
                        LowPrice = double.Parse(fields[3]),
                        ClosePrice = double.Parse(fields[4]),
                        Volume = double.Parse(fields[5])
                    };

                    records.Add(record);
                }
            }

            return records;
        }
    }
}
