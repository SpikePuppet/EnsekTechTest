using EnsekTechTestModels.Models;

namespace EnsekTechTestServices.Interfaces
{
    public interface IMeterReadingParsingService
    {
        // For now this works, though should be a list of something
        ParsingResult ParseMeterReadings(MeterReadingFile meterReading);
    }
}