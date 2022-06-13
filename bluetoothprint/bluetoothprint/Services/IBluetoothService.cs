using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using bluetoothprint.Structure;

namespace bluetoothprint.Services
{
    public interface IBluetoothService
    {
        IList<string> GetDeviceList();
        Task Print(string deviceName, byte[] buffer);
        Task Print(string deviceName, Dte dte);
    }
}
