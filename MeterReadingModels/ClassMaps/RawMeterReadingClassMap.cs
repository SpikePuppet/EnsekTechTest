using CsvHelper.Configuration;
using MeterReadingModels.Models;

namespace MeterReadingModels.ClassMaps
{
    public class RawMeterReadingClassMap : ClassMap<RawMeterReading>
    {
        public RawMeterReadingClassMap()
        {
            Map(x => x.AccountId).Name("AccountId");
            Map(x => x.MeterReadingDateTime).Name("MeterReadingDateTime").TypeConverterOption.Format("dd/MM/yyyy HH:mm");
            Map(x => x.RawMeterReadValue).Name("MeterReadValue");
        }
    }
}