using CsvHelper.Configuration;

namespace MeterReadingServices.Interfaces
{
    public interface ICsvParsingService<T> where T : class, new()
    {
        List<T> Parse(byte[] csv, ClassMap<T> classMap);
    }
}