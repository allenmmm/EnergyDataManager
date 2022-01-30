namespace EnergyDataManager.SharedKernel.interfaces
{
    /* a converter class for non list conversions*/
    public interface IConverter<TSource, TDestination>
    {
        abstract TDestination Convert(TSource source_object);
        abstract TSource Convert(TDestination source_object);
    }
}
