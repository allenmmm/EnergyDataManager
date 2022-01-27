using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly IDataReader<SourceMeterReading> _DataReader;
        private readonly IEnergyDataManagerRepo _EDMRepo;




        public MeterReadingUploadsController(
            IConfiguration configuration,
           IMeterReader meterReader,
           IConverter<SourceMeterReading, Reading> converter,
           IEnergyDataManagerRepo energyDataManagerRepo)
        {

        }   




        // POST api/<MeterReadingUploadsController>
        [HttpPost]
        public async Task<IActionResult> MeterReadingAsync()
        {
            return null;
        }
    }
}
