using System.Text.RegularExpressions;
using MeterReadingModels.Models;
using MeterReadingServices.Interfaces;
using MeterReadingModels.ClassMaps;
using Microsoft.Extensions.Logging;
using System.Text;

namespace MeterReadingServices.Services
{
    public class MeterReadingParsingService : IMeterReadingParsingService
    {
        private readonly IEntityService<Account> _accountService;
        private readonly IEntityService<MeterReading> _meterReadingService;
        private readonly ICsvParsingService<RawMeterReading> _csvParsingService;
        private readonly ILogger<MeterReadingParsingService> _logger;

        private const string NewLinePattern = "\r\n|\r|\n";

        public MeterReadingParsingService(IEntityService<Account> accountService, IEntityService<MeterReading> meterReadingService, ICsvParsingService<RawMeterReading> csvParsingService, ILogger<MeterReadingParsingService> logger)
        {
            _accountService = accountService;
            _meterReadingService = meterReadingService;
            _csvParsingService = csvParsingService;
            _logger = logger;
        }

        public ParsingResult ParseMeterReadings(MeterReadingFile meterReading)
        {
            var totalLineCount = GetTotalRecordCount(meterReading.Content);
            
            var rawMeterReadings = _csvParsingService.Parse(meterReading.Content, new RawMeterReadingClassMap());

            RemoveRawMeterReadingsWhereRawMeterReadValueDoesNotMatchFormat(rawMeterReadings);

            RemoveRawMeterReadingsWhichDoNotHaveAMatchingAccount(rawMeterReadings);

            RemoveDuplicateRawMeterReadings(rawMeterReadings);

            _meterReadingService.Add(rawMeterReadings.Select(x => new MeterReading{
                AccountId = x.AccountId,
                MeterReadValue = int.Parse(x.RawMeterReadValue),
                MeterReadingDateTime = x.MeterReadingDateTime
            }).ToList());

            return new ParsingResult
            {
                RecordsSuccessfullyParsed = rawMeterReadings.Count,
                RecordsFailedToParse = totalLineCount - rawMeterReadings.Count
            };
        }

        private int GetTotalRecordCount(byte[] content)
        {
            var fileContent = Encoding.UTF8.GetString(content);

            var lineByLineContent = Regex.Split(fileContent, NewLinePattern);

            return lineByLineContent.Skip(1).Count();
        }

        private void RemoveRawMeterReadingsWhereRawMeterReadValueDoesNotMatchFormat(List<RawMeterReading> meterReadings)
        {
            var regex = new Regex("^[0-9]{5}$");

            meterReadings.RemoveAll(meterReading => !regex.IsMatch(meterReading.RawMeterReadValue));
        }

        private void RemoveRawMeterReadingsWhichDoNotHaveAMatchingAccount(List<RawMeterReading> meterReadings)
        {
            var accountIds = _accountService.GetAll().Select(x => x.AccountId).Distinct().ToList();

            meterReadings.RemoveAll(meterReading => !accountIds.Contains(meterReading.AccountId));
        }

        private void RemoveDuplicateRawMeterReadings(List<RawMeterReading> meterReadings)
        {
            var meterReadingsToRemove = new List<RawMeterReading>();

            var meterReadingsGroupedByAccountId = meterReadings.GroupBy(meterReading => meterReading.AccountId);

            foreach (var accountMeterReadings in meterReadingsGroupedByAccountId)
            {
                var existingMeterReadings = _meterReadingService.Search(meterReading => meterReading.AccountId == accountMeterReadings.Key);

                meterReadingsToRemove.AddRange(accountMeterReadings.Where(x => DoesMeterReadingExist(x, existingMeterReadings)).ToList());
            }

            meterReadings.RemoveAll(meterReading => meterReadingsToRemove.Contains(meterReading));
        }

        private bool DoesMeterReadingExist(RawMeterReading meterReading, List<MeterReading> meterReadings)
        {
            return meterReadings.Any(x => x.AccountId == meterReading.AccountId 
                && x.MeterReadingDateTime == meterReading.MeterReadingDateTime 
                && x.MeterReadValue == int.Parse(meterReading.RawMeterReadValue));
        }
    }
}