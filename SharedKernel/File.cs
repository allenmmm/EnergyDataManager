using EnergyDataManager.SharedKernel.interfaces;
using SharedKernel.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace SharedKernel
{
    public abstract class File<TDestination,TSource>
    {
        protected IEnumerable<TDestination>  Data { get; }


        public  virtual int Rows { get { return Data.Count(); }  }


        public File(
            string fileName,
            IConverter<TDestination,TSource> converter,
            IFileReadService<TDestination, TSource> meterReadingFileService)
        {
            Data = meterReadingFileService
                .Read(fileName, converter);
        }
    }
}
