using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnergyDataManager.Web.Interfaces
{
    public interface IConverter<TSource, TDestination>
    {
        abstract List<TDestination> Convert(List<TSource> source_object);
        abstract List<TSource> Convert(List<TDestination> source_object);
    }
}
