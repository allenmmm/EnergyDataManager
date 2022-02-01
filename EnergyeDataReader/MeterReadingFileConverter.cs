using EnergyDataManager.SharedKernel.interfaces;
using System;

namespace EnergyDataReader.File
{
    public class MeterReadingFileConverter : IConverter<SourceMeterReading, string>
    {
        public string Convert(SourceMeterReading source_object)
        {
            return null;
        }

        public SourceMeterReading Convert(string source_object)
        {
            try
            {
                var decodedSource = source_object.Split(',');
                return new SourceMeterReading(
                    decodedSource[0],
                    decodedSource[1],
                    decodedSource[2]);
            }
            catch (Exception ex)
            {
                throw new 
                    FormatException("Meter reading is incorrectly formatted");
            }
        }
    }
}
