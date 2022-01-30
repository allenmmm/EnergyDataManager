using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnergyDataManager.SharedKernel.Interfaces
{
    public interface IConverterList<TDestination, TSource>
    {
        abstract TDestination Convert(IEnumerable<TSource> source_object);
        abstract IEnumerable<TSource> Convert(TDestination source_object);
    }
}
