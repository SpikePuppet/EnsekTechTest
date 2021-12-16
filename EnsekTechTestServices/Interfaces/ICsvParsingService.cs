using CsvHelper.Configuration;

namespace EnsekTechTestServices.Interfaces
{
    public interface ICsvParsingService<T> where T : class, new()
    {
        List<T> Parse(byte[] csv, ClassMap<T> classMap);
    }
}