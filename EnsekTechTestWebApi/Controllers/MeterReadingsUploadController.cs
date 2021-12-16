using Microsoft.AspNetCore.Mvc;
using EnsekTechTestServices.Interfaces;
using EnsekTechTestModels.Models;

namespace EnsekTechTestWebApi.Controllers;

[ApiController]
[Route("meter-reading-upload")]
public class MeterReadingsUploadController : ControllerBase
{
    private readonly IMeterReadingParsingService _meterReadingParsingService;

    public MeterReadingsUploadController(IMeterReadingParsingService meterReadingParsingService)
    {
        _meterReadingParsingService = meterReadingParsingService;
    }

    [HttpPost(Name = "UploadMeterReadings")]
    public IActionResult Post([FromForm(Name = "MeterReadings")]IFormFile meterReadings)
    {
        var meterReadingFiles = GetMeterReadingFileFromFormCollection(meterReadings);

        var results = _meterReadingParsingService.ParseMeterReadings(meterReadingFiles);

        return Ok(results);
    }

    private MeterReadingFile GetMeterReadingFileFromFormCollection(IFormFile meterReadingFile)
    {
        byte[] content;

        using var memoryStream = new MemoryStream();
        meterReadingFile.CopyTo(memoryStream);
        content = memoryStream.ToArray();

        return new MeterReadingFile
        {
            Content = content
        };
    }
}
