using BluetoothCbor.DataIdentifiers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BluetoothCbor.DataModels
{
    internal class DataStorage
    {
        private static readonly Dictionary<DataIdentifier, List<DataEntry>> _storage = [];
        private static readonly Dictionary<SetDataIdentifier, object> _setStorage = [];  
        private static readonly List<DataIdentifier> _notificationIdentifiers = [];

        // Method to add a DataIdentifier to the list
        public static void AddNotificationIdentifier(DataIdentifier identifier)
        {
            if (!_notificationIdentifiers.Contains(identifier))
            {
                _notificationIdentifiers.Add(identifier);
            }
        }

        // Method to remove a specific DataIdentifier from the list
        public static bool RemoveNotificationIdentifier(DataIdentifier identifier)
        {
            return _notificationIdentifiers.Remove(identifier);
        }

        // Method to clear all identifiers from the list
        public static void ClearAllNotificationIdentifiers()
        {
            _notificationIdentifiers.Clear();
        }

        // Optional: Method to retrieve all notification identifiers
        public static List<DataIdentifier> GetAllNotificationIdentifiers()
        {
            return new List<DataIdentifier>(_notificationIdentifiers); // Return a copy to protect the original list
        }


        public static void SetValue(SetDataIdentifier identifier, object value)
        {
            _setStorage[identifier] = value;
        }

        public static object GetValue(SetDataIdentifier identifier)
        {
            return _setStorage.TryGetValue(identifier, out var value) ? value : null;
        }

        public static void AddData(DataIdentifier identifier, object val)
        {
            var dataEntry = new DataEntry(val, DateTime.UtcNow);

            if (!_storage.TryGetValue(identifier, out List<DataEntry> value))
            {
                value = ([]);
                _storage[identifier] = value;
            }

            value.Add(dataEntry);
        }

        public static List<DataEntry> GetData(DataIdentifier identifier)
        {
            return _storage.TryGetValue(identifier, out var entries) ? entries : [];
        }
        public static DataEntry GetLatestData(DataIdentifier identifier)
        {
            if (_storage.TryGetValue(identifier, out var entries) && entries.Count > 0)
            {
                return entries[^1]; // Return the last entry in the list
            }

            return null; // Or you can return a default value if no entries exist
        }

    }
}
