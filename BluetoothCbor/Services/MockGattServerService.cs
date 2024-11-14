using BluetoothCbor.DataIdentifiers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BluetoothCbor.Services
{
    public class MockGattServerService : IGattServerService
    {
        public Task StartGattServerAsync() => Task.CompletedTask;
        public Task StopGattServerAsync() => Task.CompletedTask;
        public Task SendNotificationAsync(List<DataIdentifier> identifiersToSend, int sequenceNumber) => Task.CompletedTask;
    }
}
