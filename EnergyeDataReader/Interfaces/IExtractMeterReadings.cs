using System.Collections.Generic;

namespace EnergyDataReader.File.Interfaces
{
    public interface IExtractMeterReadings
    {
        IEnumerable< IEnumerable<SourceMeterReading>> RetrieveReadingsByAccountId();     
    }
}
