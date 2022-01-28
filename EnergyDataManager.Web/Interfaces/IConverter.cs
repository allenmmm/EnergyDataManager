using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnergyDataManager.Web.Interfaces
{
    public interface IConverter<TSource, TDestination>
    {
        abstract TDestination Convert(IEnumerable<TSource> source_object);
        abstract IEnumerable<TSource> Convert(TDestination source_object);
    }
}
