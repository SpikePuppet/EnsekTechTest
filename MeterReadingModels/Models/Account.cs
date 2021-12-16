using System.ComponentModel.DataAnnotations;

namespace MeterReadingModels.Models
{
    public class Account
    {
        [Key]
        public int AccountId { get; set; } 

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}