using System.Linq.Expressions;
using System.Text;
using System;
using System.Collections.Generic;
using CsvHelper.Configuration;
using MeterReadingModels.Models;
using MeterReadingServices.Interfaces;
using MeterReadingServices.Services;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace MeterReadingTests;

[TestClass]
public class MeterReadingParsingServiceTests
{
    private readonly Mock<IEntityService<Account>> _accountService = new Mock<IEntityService<Account>>();
    private readonly Mock<IEntityService<MeterReading>> _meterReadingService = new Mock<IEntityService<MeterReading>>();
    private readonly Mock<ICsvParsingService<RawMeterReading>> _csvParsingService = new Mock<ICsvParsingService<RawMeterReading>>();
    private readonly Mock<ILogger<MeterReadingParsingService>> _logger = new Mock<ILogger<MeterReadingParsingService>>();

    private MeterReadingParsingService _meterReadingParsingService;

    [TestInitialize]
    public void SetupTestEnvironment()
    {        
        _accountService.Setup(x => x.GetAll()).Returns(new List<Account>());

        _meterReadingService.Setup(x => x.Search(It.IsAny<Expression<Func<MeterReading, bool>>>())).Returns(new List<MeterReading>());

        _meterReadingParsingService = new MeterReadingParsingService(_accountService.Object, _meterReadingService.Object, _csvParsingService.Object, _logger.Object);
    }

    [TestMethod]
    public void ParseMeterReadings_NoFileContent_ReturnsParsingResultWithRecordsSuccessfullyParsedAndRecordsFailedToParseSetToZero()
    {
        // Arrange
        _csvParsingService.Setup(x => x.Parse(It.IsAny<byte[]>(), It.IsAny<ClassMap<RawMeterReading>>())).Returns(new List<RawMeterReading>());

        // Act
        var parsingResult = _meterReadingParsingService.ParseMeterReadings(new MeterReadingFile{Content = new byte[0]});

        // Assert
        Assert.AreEqual(0, parsingResult.RecordsSuccessfullyParsed);
        Assert.AreEqual(0, parsingResult.RecordsFailedToParse);
    }

    [TestMethod]
    public void ParseMeterReadings_CsvFileWithHeaderAndOneRecordWithMeterReadingNotConformingToNNNNNFormat_ReturnsParsingResultWithRecordsSuccessfullyParsedSetToZeroAndRecordsFailedToParseSetToOne()
    {
        // Arrange
        _csvParsingService.Setup(x => x.Parse(It.IsAny<byte[]>(), It.IsAny<ClassMap<RawMeterReading>>())).Returns(new List<RawMeterReading>
        {
            new RawMeterReading
            {
                AccountId = 123456789,
                RawMeterReadValue = "123454",
                MeterReadingDateTime = DateTime.Parse("01/01/2018 00:00")
            }
        });

        var content = "AccountId,MeterReadingDateTime,RawMeterReadValue\r\n123456789,01/01/2018 00:00,123454";

        // Act
        var parsingResult = _meterReadingParsingService.ParseMeterReadings(new MeterReadingFile{Content = Encoding.ASCII.GetBytes(content)});

        // Assert
        Assert.AreEqual(0, parsingResult.RecordsSuccessfullyParsed);
        Assert.AreEqual(1, parsingResult.RecordsFailedToParse);
    }
    
    [TestMethod]
    public void ParseMeterReadings_CsvFileWithHeaderAndOneRecordWithMeterReadingConformingToNNNNNFormatButAccountDoesNotExists_ReturnsParsingResultWithRecordsSuccessfullyParsedSetToZeroAndRecordsFailedToParseSetToOne()
    {
        // Arrange
        _csvParsingService.Setup(x => x.Parse(It.IsAny<byte[]>(), It.IsAny<ClassMap<RawMeterReading>>())).Returns(new List<RawMeterReading>
        {
            new RawMeterReading{
                AccountId = 123456789,
                RawMeterReadValue = "12345",
                MeterReadingDateTime = DateTime.Parse("01/01/2018 00:00")
            }        
        });

        var content = "AccountId,MeterReadingDateTime,RawMeterReadValue\r\n123456789,01/01/2018 00:00,12345";

        // Act
        var parsingResult = _meterReadingParsingService.ParseMeterReadings(new MeterReadingFile{Content = Encoding.ASCII.GetBytes(content)});

        // Assert
        Assert.AreEqual(0, parsingResult.RecordsSuccessfullyParsed);
        Assert.AreEqual(1, parsingResult.RecordsFailedToParse);
    }

    [TestMethod]
    public void ParseMeterReadings_CsvFileWithHeaderAndOneRecordWithMeterReadingConformingToNNNNNFormatWithAMatchingAccountButEntryAlreadyExists_ReturnsParsingResultWithRecordsSuccessfullyParsedSetToZeroAndRecordsFailedToParseSetToOne()
    {
        // Arrange
        _csvParsingService.Setup(x => x.Parse(It.IsAny<byte[]>(), It.IsAny<ClassMap<RawMeterReading>>())).Returns(new List<RawMeterReading>
        {
            new RawMeterReading
            {
                AccountId = 123456789,
                RawMeterReadValue = "12345",
                MeterReadingDateTime = DateTime.Parse("01/01/2018 00:00")
            }        
        });

        _accountService.Setup(x => x.GetAll()).Returns(new List<Account>
        {
            new Account
            {
                AccountId = 123456789,
                FirstName = "Test",
                LastName = "Account"
            }
        });

        _meterReadingService.Setup(x => x.Search(It.IsAny<Expression<Func<MeterReading, bool>>>())).Returns(new List<MeterReading>
        {
            new MeterReading
            {
                AccountId = 123456789,
                MeterReadingDateTime = DateTime.Parse("01/01/2018 00:00"),
                MeterReadValue = 12345
            }
        });

        var content = "AccountId,MeterReadingDateTime,RawMeterReadValue\r\n123456789,01/01/2018 00:00,12345";

        // Act
        var parsingResult = _meterReadingParsingService.ParseMeterReadings(new MeterReadingFile{Content = Encoding.ASCII.GetBytes(content)});

        // Assert
        Assert.AreEqual(0, parsingResult.RecordsSuccessfullyParsed);
        Assert.AreEqual(1, parsingResult.RecordsFailedToParse);
    }

    

    [TestMethod]
    public void ParseMeterReadings_CsvFileWithHeaderAndOneRecordWithMeterReadingConformingToNNNNNFormatWithAMatchingAccountAndEntryDoesNotExist_AddsMeterReadingToDataBaseAndReturnsParsingResultWithRecordsSuccessfullyParsedSetToOneAndRecordsFailedToParseSetToZero()
    {
        // Arrange
        _csvParsingService.Setup(x => x.Parse(It.IsAny<byte[]>(), It.IsAny<ClassMap<RawMeterReading>>())).Returns(new List<RawMeterReading>
        {
            new RawMeterReading
            {
                AccountId = 123456789,
                RawMeterReadValue = "12345",
                MeterReadingDateTime = DateTime.Parse("01/01/2018 00:00")
            }        
        });

        _accountService.Setup(x => x.GetAll()).Returns(new List<Account>
        {
            new Account
            {
                AccountId = 123456789,
                FirstName = "Test",
                LastName = "Account"
            }
        });
        
        var content = "AccountId,MeterReadingDateTime,RawMeterReadValue\r\n123456789,01/01/2018 00:00,12345";

        // Act
        var parsingResult = _meterReadingParsingService.ParseMeterReadings(new MeterReadingFile{Content = Encoding.ASCII.GetBytes(content)});

        // Assert
        Assert.AreEqual(1, parsingResult.RecordsSuccessfullyParsed);
        Assert.AreEqual(0, parsingResult.RecordsFailedToParse);
        _meterReadingService.Verify(x => x.Add(It.Is<List<MeterReading>>(y => y.Count == 1)), Times.Once);
    }
}