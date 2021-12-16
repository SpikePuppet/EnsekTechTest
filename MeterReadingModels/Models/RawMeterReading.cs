using System.ComponentModel.DataAnnotations;

namespace MeterReadingModels.Models
{
    public class RawMeterReading
    {
        public int AccountId { get; set; }

        public DateTime MeterReadingDateTime { get; set; }

        public string RawMeterReadValue { get; set; }
    }
}