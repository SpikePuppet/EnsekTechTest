using System.ComponentModel.DataAnnotations;

namespace EnsekTechTestModels.Models
{
    public class RawMeterReading
    {
        public int AccountId { get; set; }

        public DateTime MeterReadingDateTime { get; set; }

        public string RawMeterReadValue { get; set; }
    }
}