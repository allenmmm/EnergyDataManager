using EnergyDataManager.SharedKernel;
using EnergyDataManager.SharedKernel.interfaces;
using EnergyDataManager.SharedKernel.Interfaces;
using EnergyDataReader.Interfaces;
using SharedKernel.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Configuration;
namespace EnergyDataReader
{
    public class MeterReadingFile : File<SourceMeterReading,string>, IExtractMeterReadings
    {

        public MeterReadingFile(
            IConfiguration configuration,
            IConverter<SourceMeterReading, string> converter,
            IFileReadService<SourceMeterReading,string> meterReadingFileService) 
                : base (
                      configuration.GetSection("MeterReadingFileName").Value, 
                      converter, 
                      meterReadingFileService)

        {

        }

        public virtual IEnumerable<IEnumerable<SourceMeterReading>> RetrieveReadingsByAccountId()
        {
            var uniqueAccountIds = Data.Select(x => x.AccountId).Distinct();
            foreach (var id in uniqueAccountIds)
            {
              yield return  Data.Where(x => x.AccountId == id).Select(
                     d => new SourceMeterReading(d)).ToList();
                   }
        }

    }
}
