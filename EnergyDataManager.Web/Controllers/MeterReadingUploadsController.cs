using System.Net;
using System.Threading.Tasks;
using EnergyDataManager.Domain;
using EnergyDataManager.Domain.Interfaces;
using EnergyDataManager.SharedKernel.Interfaces;
using EnergyDataReader.File;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EnergyDataManager.Web.Controllers
{
    [Produces("application/json")]
    [Route("meter-reading-uploads")]
    [ApiController]
    public class MeterReadingUploadsController : ControllerBase
    {
        private readonly IConverterList<Account, SourceMeterReading> _Converter;
        private readonly IEDMRepo _EnergyDataManagerRepo;
        private readonly MeterReadingFile _MeterReadingFile;

        public MeterReadingUploadsController(
            IConverterList<Account, SourceMeterReading> converter,
            IEDMRepo energyDataManagerRepo,
            MeterReadingFile meterReadingFile)
        {
            _Converter = converter;
            _EnergyDataManagerRepo = energyDataManagerRepo;
            _MeterReadingFile = meterReadingFile;

        }   

        // POST api/<MeterReadingUploadsController>
        [HttpPost]
        public async Task<IActionResult> MeterReadingAsync()
        {
            int numberOfUpdatedReadings = 0;

            foreach( var sourceMeterReadings in _MeterReadingFile.RetrieveReadingsByAccountId())
            {
                var account = _Converter.Convert(sourceMeterReadings);
                numberOfUpdatedReadings += await _EnergyDataManagerRepo
                    .UpdateAccountWithMeterReadingsAsync(account);
            }
            return StatusCode(
                (int)HttpStatusCode.Created, 
                $"{numberOfUpdatedReadings}/{_MeterReadingFile.Rows} meter readings were uploaded");
        }
    }
}
