using MeterReadingModels.Models;

namespace MeterReadingServices.Interfaces
{
    public interface IMeterReadingParsingService
    {
        // For now this works, though should be a list of something
        ParsingResult ParseMeterReadings(MeterReadingFile meterReading);
    }
}