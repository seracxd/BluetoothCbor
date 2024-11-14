using BluetoothCbor.DataIdentifiers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#if WINDOWS
using BluetoothCbor.Platforms.Windows;
#endif

namespace BluetoothCbor.Services
{
    public interface IGattServerService
    {
        Task StartGattServerAsync();
        Task StopGattServerAsync();
        Task SendNotificationAsync(List<DataIdentifier> identifiersToSend, int sequenceNumber);
    }
}