using EnergyDataManager.SharedKernel.interfaces;
using System.Collections.Generic;

namespace SharedKernel.Interfaces
{
    public interface IFileReadService <TDestination,TSource>
    {
        IEnumerable<TDestination> Read(
            string fileName,
            IConverter<TDestination, TSource> converter);
    }
}
