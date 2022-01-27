using System;
using System.Collections.Generic;
using System.Text;

namespace EnergyDataReader.Interfaces
{
    public interface IDataReader <T> where T : class
    {
        public T GetData();


    }
}
