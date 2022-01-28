using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EnergyDataReader.Interfaces
{
    public interface IMeterReader
    {
         IEnumerable<SourceMeterReading> RetrieveReadingsByAccountId();
         Task<int> LoadReadingsAsync(string fileName);
    }
}
