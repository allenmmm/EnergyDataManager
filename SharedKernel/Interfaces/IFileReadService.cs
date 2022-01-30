using EnergyDataManager.SharedKernel.interfaces;
using EnergyDataManager.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharedKernel.Interfaces
{
    public interface IFileReadService <TDestination,TSource>
    {
        IEnumerable<TDestination> Read(
            string fileName,
            IConverter<TDestination, TSource> converter);
    }
}
