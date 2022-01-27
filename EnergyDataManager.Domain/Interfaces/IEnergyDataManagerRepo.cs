using EnergyDataManager.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace EnergyDataManager.Domain.Interfaces
{
    public interface IEnergyDataManagerRepo
    {
        Account ReadAccountWithMeterReading(int id);

        int UpdateAccountWithMeterReadings(
            Reading reading);
    }
}
