using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using MeterReadingServices.Interfaces;
using Microsoft.Extensions.Logging;

namespace MeterReadingServices.Services
{
    public class CsvParsingService<T> : ICsvParsingService<T> where T : class, new()
    {
        private readonly ILogger<CsvParsingService<T>> _logger;

        public CsvParsingService(ILogger<CsvParsingService<T>> logger)
        {
            _logger = logger;
        }

        public List<T> Parse(byte[] csv, ClassMap<T> classMap)
        {
            var records = new List<T>();
            var csvReaderConfig = SetupCsvReaderConfig();

            using var memoryStream = new MemoryStream(csv);
            using var streamReader = new StreamReader(memoryStream);
            using var csvReader = new CsvReader(streamReader, csvReaderConfig);
            csvReader.Context.RegisterClassMap(classMap);
            
            while (csvReader.Read())
            {
                try 
                {
                    var record = csvReader.GetRecord<T>();                                        

                    records.Add(record);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Error parsing meter reading record");
                }                
            }

            return records;
        }

        private CsvConfiguration SetupCsvReaderConfig()
        {
            return new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
            };
        }
    }
}