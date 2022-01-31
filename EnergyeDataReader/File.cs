using EnergyDataManager.SharedKernel.interfaces;
using EnergyDataManager.SharedKernel.Interfaces;
using SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace EnergyDataReader.File
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
