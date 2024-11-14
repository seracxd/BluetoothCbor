using BluetoothCbor.DataIdentifiers;
using System;
using System.Collections.Generic;
using System.Formats.Cbor;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace BluetoothCbor.Utilities
{
    internal class CborUtility
    {

        public static (int sequenceNumber, MessageTypeIdentifiers MessageTypeIdentifiers, Dictionary<DataIdentifier, object> data) DecodeRequest(byte[] cborData)
        {
            var reader = new CborReader(cborData);
            reader.ReadStartMap();

            int sequenceNumber = -1;
            MessageTypeIdentifiers MessageTypeIdentifiers = MessageTypeIdentifiers.Undefined;
            var data = new Dictionary<DataIdentifier, object>();

            while (reader.PeekState() != CborReaderState.EndMap)
            {
                int key = reader.ReadInt32();

                if (key == (int)CborIdentifiers.SequenceNumber)
                {
                    sequenceNumber = reader.ReadInt32();
                }
                else if ((key == (int)MessageTypeIdentifiers.Request)|| (key == (int)MessageTypeIdentifiers.SetNotification))
                {
                    MessageTypeIdentifiers = (MessageTypeIdentifiers)key;
                    data = DecodeIdentifiers(reader);
                }
                else if (key == (int)MessageTypeIdentifiers.Set)
                {
                    MessageTypeIdentifiers = MessageTypeIdentifiers.Set;
                    data = DecodeIdentifiersWithValues(reader);
                }
                else
                {
                    throw new InvalidOperationException($"Unknown key in CBOR data: {key}");
                }            
            }

            reader.ReadEndMap();
            return (sequenceNumber, MessageTypeIdentifiers, data);
        }


        public static byte[] EncodeResponse(int sequenceNumber, Dictionary<DataIdentifier, object> responseData)
        {
            var writer = new CborWriter();
            writer.WriteStartMap(responseData.Count + 2);

            writer.WriteInt32((int)CborIdentifiers.SequenceNumber);
            writer.WriteInt32(sequenceNumber);

            writer.WriteInt32((int)CborIdentifiers.Timestamp);
            writer.WriteInt64(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());

            foreach (var entry in responseData)
            {
                writer.WriteInt32(entry.Key.Id);
                WriteValue(writer, entry.Value);
            }

            writer.WriteEndMap();
            return writer.Encode();
        }

        private static Dictionary<DataIdentifier, object> DecodeIdentifiers(CborReader reader)
        {
            var identifiers = new Dictionary<DataIdentifier, object>();

            reader.ReadStartArray();
            while (reader.PeekState() != CborReaderState.EndArray)
            {
                var id = reader.ReadInt32();
                var identifier = DataIdentifierRegistry.GetById(id)
                    ?? throw new InvalidOperationException($"No identifier found for ID {id}");

                // Ukládáme pouze identifikátor bez hodnoty (null) pro typ Request
                identifiers[identifier] = null;
            }
            reader.ReadEndArray();

            return identifiers;
        }

        private static Dictionary<DataIdentifier, object> DecodeIdentifiersWithValues(CborReader reader)
        {
            var identifiersWithValues = new Dictionary<DataIdentifier, object>();

            reader.ReadStartArray();
            while (reader.PeekState() != CborReaderState.EndArray)
            {
                var id = reader.ReadInt32();
                var identifier = DataIdentifierRegistry.GetById(id)
                    ?? throw new InvalidOperationException($"No identifier found for ID {id}");

                // Čteme hodnotu na základě očekávaného typu pro typ Set
                var value = ReadValue(reader, identifier.ExpectedType);
                identifiersWithValues[identifier] = value;
            }
            reader.ReadEndArray();

            return identifiersWithValues;
        }

        private static object ReadValue(CborReader reader, Type expectedType)
        {
            return expectedType switch
            {
                Type t when t == typeof(int) => reader.ReadInt32(),
                Type t when t == typeof(float) => reader.ReadSingle(),
                Type t when t == typeof(bool) => reader.ReadBoolean(),
                Type t when t == typeof(string) => reader.ReadTextString(),
                Type t when t == typeof(long) => reader.ReadInt64(),
                Type t when t == typeof(ushort) => (ushort)reader.ReadUInt32(),
                Type t when t == typeof(byte) => (byte)reader.ReadUInt32(),
                Type t when t == typeof(sbyte) => (sbyte)reader.ReadInt32(),
                Type t when t == typeof(TimeSpan) => TimeSpan.FromTicks(reader.ReadInt64()),
                _ => throw new InvalidOperationException($"Unsupported type {expectedType.Name}"),
            };
        }
        private static void WriteValue(CborWriter writer, object value)
        {
            switch (value)
            {
                case float f: writer.WriteSingle(f); break;
                case bool b: writer.WriteBoolean(b); break;
                case int i: writer.WriteInt32(i); break;
                case uint u: writer.WriteUInt32(u); break;
                case ushort us: writer.WriteUInt32(us); break;
                case sbyte sb: writer.WriteInt32(sb); break;
                case byte bt: writer.WriteUInt32(bt); break;
                case TimeSpan ts: writer.WriteInt64(ts.Ticks); break;
                case string str: writer.WriteTextString(str); break;
                case long l: writer.WriteInt64(l); break;
                default: throw new InvalidOperationException("Unsupported data type");
            }
        }
    }
}
