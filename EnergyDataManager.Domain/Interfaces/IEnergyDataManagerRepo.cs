using System.Threading.Tasks;

namespace EnergyDataManager.Domain.Interfaces
{
    public interface IEDMRepo
    {
        Task<int> UpdateAccountWithMeterReadingsAsync(
            Account account);
    }
}
