using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EnergyDataReader.File.Interfaces
{
    public interface IExtractMeterReadings
    {
        //  int TotalNumberOfProperties { get; }

        IEnumerable< IEnumerable<SourceMeterReading>> RetrieveReadingsByAccountId();
    //     Task<int> LoadReadingsAsync(string fileName);
         
    }
}
