using CsvHelper.Configuration;
using MeterReadingModels.Models;

namespace MeterReadingModels.ClassMaps
{
    public class AccountClassMap : ClassMap<Account>
    {
        public AccountClassMap()
        {
            Map(m => m.AccountId).Name("AccountId");
            Map(m => m.FirstName).Name("FirstName");
            Map(m => m.LastName).Name("LastName");
        }
    }
}