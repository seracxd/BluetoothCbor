using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BluetoothCbor.DataIdentifiers
{
    public static class DataIdentifierRegistry
    {
        private static readonly Dictionary<int, DataIdentifier> _identifiersById = [];

        /// <summary>
        /// Registers a DataIdentifier in the central registry.
        /// </summary>
        /// <param name="identifier">The DataIdentifier instance to register</param>
        public static void Register(DataIdentifier identifier)
        {
            if (!_identifiersById.ContainsKey(identifier.Id))
            {
                _identifiersById[identifier.Id] = identifier;
            }
            else
            {
                throw new InvalidOperationException($"Identifier with ID {identifier.Id} is already registered.");
            }
        }

        /// <summary>
        /// Retrieves a DataIdentifier by its ID.
        /// </summary>
        /// <param name="id">The ID of the DataIdentifier</param>
        /// <returns>The DataIdentifier instance</returns>
        public static DataIdentifier GetById(int id)
        {
            if (_identifiersById.TryGetValue(id, out var identifier))
            {
                return identifier;
            }

            throw new KeyNotFoundException($"No identifier found for ID {id}");
        }
    }
}
