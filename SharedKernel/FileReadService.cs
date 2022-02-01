using EnergyDataManager.SharedKernel.interfaces;
using SharedKernel.Interfaces;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SharedKernel
{
    public class FileReadService<T> : IFileReadService<T, string>
    {
        public IEnumerable<T> Read(string fileName, IConverter<T, string> converter)
        {
            try
            {
                return (IEnumerable<T>)
                    System.IO.File.ReadAllLines(fileName)
                        .Skip(1)
                        .Select(v => converter.Convert(v))
                        .ToList();
            }
            catch (IOException ex)
            {
                throw new IOException("Unable to find or load file");
            }
        }
    }
}
