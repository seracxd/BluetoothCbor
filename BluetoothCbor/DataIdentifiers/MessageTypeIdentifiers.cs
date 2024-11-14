using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BluetoothCbor.DataIdentifiers
{
    public enum MessageTypeIdentifiers
    {
        Request = 1100,
        Set = 1101,
        Notification = 1102,
        SetNotification = 1103,
        Undefined =1104     
    }
}
