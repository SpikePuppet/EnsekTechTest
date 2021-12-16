using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using MeterReadingModels.ClassMaps;
using MeterReadingModels.Models;
using Microsoft.EntityFrameworkCore;

namespace MeterReadingDatabase
{
    public class MeterReadingDbContext : DbContext
    {
        public MeterReadingDbContext(DbContextOptions<MeterReadingDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Define in model
            var accounts = GenerateAccountSeedData();

            modelBuilder.Entity<Account>().HasData(accounts);
        }

        private List<Account> GenerateAccountSeedData()
        {
            using var streamReader = new StreamReader("Test_Accounts.csv");
            using var csvReader = new CsvReader(streamReader, CultureInfo.InvariantCulture);
            csvReader.Context.RegisterClassMap<AccountClassMap>();
            return csvReader.GetRecords<Account>().ToList();
        }

        public DbSet<MeterReading> MeterReadings { get; set; }

        public DbSet<Account> Accounts { get; set; }
    }
}