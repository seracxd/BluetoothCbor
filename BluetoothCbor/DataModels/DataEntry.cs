using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BluetoothCbor.DataModels
{
    public class DataEntry
    {
        public object Value { get; set; }
        public DateTimeOffset? Timestamp { get; set; }

        public DataEntry(object data)
        {
            Value = data;            
        }
      
        public DataEntry(object data, DateTimeOffset timestamp)
        {
            Value = data;
            Timestamp = timestamp;
        }
    }
}
