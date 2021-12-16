using CsvHelper.Configuration;
using EnsekTechTestModels.Models;

namespace EnsekTechTestModels.ClassMaps
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