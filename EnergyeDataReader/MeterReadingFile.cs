using EnergyDataManager.SharedKernel.interfaces;
using SharedKernel.Interfaces;
using System.Collections.Generic;
using System.Linq;
using EnergyDataReader.File.Interfaces;
using SharedKernel;
using Microsoft.Extensions.Configuration;

namespace EnergyDataReader.File
{
    public class MeterReadingFile : File<SourceMeterReading,string>, IExtractMeterReadings
    {

        public MeterReadingFile(
            IConfiguration configuration,
            IConverter<SourceMeterReading, string> converter,
            IFileReadService<SourceMeterReading,string> meterReadingFileService) 
                : base (
                      configuration.GetSection("MeterReadingsFilePath").Value, 
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
