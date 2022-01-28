using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using EnergyDataManager.Domain;
using EnergyDataManager.Domain.Interfaces;
using EnergyDataManager.Web.Interfaces;
using EnergyDataReader;
using EnergyDataReader.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EnergyDataManager.Web.Controllers
{
    [Route("meter-reading-uploads")]
    [ApiController]
    public class MeterReadingUploadsController : ControllerBase
    {
        // GET: api/<MeterReadingUploadsController>
        private readonly IConfiguration _Configuration;
        private readonly IMeterReader _MeterReader;
        private readonly IConverter<SourceMeterReading, Account> _Converter;
        private readonly IEnergyDataManagerRepo _EnergyDataManagerRepo;


        public MeterReadingUploadsController(
            IConfiguration configuration,
            IMeterReader meterReader,
            IConverter<SourceMeterReading, Account> converter,
            IEnergyDataManagerRepo energyDataManagerRepo)
        {
            _Configuration = configuration;
            _MeterReader = meterReader;
            _Converter = converter;
            _EnergyDataManagerRepo = energyDataManagerRepo;
        }   

        // POST api/<MeterReadingUploadsController>
        [HttpPost]
        public async Task<IActionResult> MeterReadingAsync()
        {
            IEnumerable<SourceMeterReading> sourceMeterReadings =
                _MeterReader.RetrieveReadingsByAccountId();
            int numberOfUpdatedReadings = 0;

            var totalLoadedNumberOfReadings =  await _MeterReader
                .LoadReadingsAsync(_Configuration
                    .GetValue<string>("MeterReadingFileName"));

            while (sourceMeterReadings != null)
            {
                var account = _Converter.Convert(sourceMeterReadings);
                numberOfUpdatedReadings += await _EnergyDataManagerRepo
                    .UpdateAccountWithMeterReadingsAsync(account);
                sourceMeterReadings = _MeterReader.RetrieveReadingsByAccountId();
            } 
            return StatusCode(
                (int)HttpStatusCode.Created, 
                $"{numberOfUpdatedReadings}/{totalLoadedNumberOfReadings} meter readings were uploaded");
        }
    }
}
