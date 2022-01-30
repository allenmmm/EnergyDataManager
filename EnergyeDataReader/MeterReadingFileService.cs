using EnergyDataManager.SharedKernel.interfaces;
using EnergyDataManager.SharedKernel.Interfaces;
using SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace EnergyDataReader
{
    public class MeterReadingFileService : IFileReadService<SourceMeterReading,string>
    {
        public IEnumerable<SourceMeterReading> Read(
            string fileName,
            IConverter<SourceMeterReading,string> converter)
        {
            try
            {
                return (IEnumerable<SourceMeterReading>)
                    File.ReadAllLines(fileName)
                        .Skip(1)
                        .Select(v => converter.Convert(v))
                        .ToList();
            }
            catch(IOException ex)
            {
                throw new IOException("Unable to find or load meter reading file");
            }
        }
    }
}
