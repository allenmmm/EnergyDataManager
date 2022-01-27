using System;
using System.Collections.Generic;
using System.Text;

namespace EnergyDataReader.Interfaces
{
    public interface IMeterReader
    {
         List<SourceMeterReading> RetrieveReadingsByAccountId();
         void LoadReadings(string fileName);
    }
}
