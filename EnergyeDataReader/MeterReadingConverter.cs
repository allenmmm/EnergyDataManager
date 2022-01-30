using EnergyDataManager.SharedKernel.interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace EnergyDataReader
{
    public class MeterReadingConverter : IConverter<SourceMeterReading, string>
    {
        public string Convert(SourceMeterReading source_object)
        {
            throw new NotImplementedException();
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
