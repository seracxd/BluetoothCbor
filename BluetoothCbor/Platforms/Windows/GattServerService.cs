using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.Versioning;

using Windows.Devices.Bluetooth.Advertisement;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using Windows.Devices.Bluetooth;
using Windows.Storage.Streams;

using BluetoothCbor.DataIdentifiers;
using BluetoothCbor.Utilities;
using BluetoothCbor.DataModels;
using BluetoothCbor.Services;




namespace BluetoothCbor.Platforms.Windows
{
    internal class GattServerService : IGattServerService
    {
        private GattServiceProvider _gattServiceProvider;
        private static GattLocalCharacteristic _notificationCharacteristic;        

        // TODO: vyřešit problémy se staršími verzemi
        // Starts the GATT server and initializes characteristics
        [SupportedOSPlatform("windows10.0.19041")]
        public async Task StartGattServerAsync()
        {
            // Define UUID for the GATT service
            Guid serviceUuid = new("AB12EF34-1000-4e5d-6c7b-8a9babcdef04");
            var serviceProviderResult = await GattServiceProvider.CreateAsync(serviceUuid);

            if (serviceProviderResult.Error == BluetoothError.Success)
            {
                _gattServiceProvider = serviceProviderResult.ServiceProvider;

                // Initialize characteristics (Read, Write, Notifications, etc.)
                InitializeCharacteristics();
             
            }
        }

        private async void InitializeCharacteristics()
        {
            // Vytvoření charakteristiky pro čtení
            var readCharacteristicParameters = new GattLocalCharacteristicParameters
            {
                CharacteristicProperties = GattCharacteristicProperties.Read,
                ReadProtectionLevel = GattProtectionLevel.EncryptionRequired,
                UserDescription = "Read"
            };
            var readCharacteristicUuid = new Guid("AB12EF34-0000-1a2b-3c4d-5678abcdef01");
            await _gattServiceProvider.Service.CreateCharacteristicAsync(readCharacteristicUuid, readCharacteristicParameters);          
            

            // Vytvoření charakteristiky pro zápis
            var writeCharacteristicParameters = new GattLocalCharacteristicParameters
            {
                CharacteristicProperties = GattCharacteristicProperties.Write,
                WriteProtectionLevel = GattProtectionLevel.EncryptionRequired,
                UserDescription = "Write"
            };
            var writeCharacteristicUuid = new Guid("AB12EF34-0001-5f6e-7d8c-90ababcdef02");
            await _gattServiceProvider.Service.CreateCharacteristicAsync(writeCharacteristicUuid, writeCharacteristicParameters);

            // Vytvoření charakteristiky pro data notifikace
            var dataNotificationParameters = new GattLocalCharacteristicParameters
            {
                CharacteristicProperties = GattCharacteristicProperties.Notify,
                ReadProtectionLevel = GattProtectionLevel.EncryptionRequired,
                UserDescription = "Data Notification"
            };
            var dataNotificationCharacteristicUuid = new Guid("AB12EF34-0002-1b2a-3d4c-5e6fabcdef03");
            var dataNotificationResult = await _gattServiceProvider.Service.CreateCharacteristicAsync(dataNotificationCharacteristicUuid, dataNotificationParameters);
            _notificationCharacteristic = dataNotificationResult.Characteristic;

            // Vytvoření charakteristiky pro upozornění notifikace
            var warningNotificationParameters = new GattLocalCharacteristicParameters
            {
                CharacteristicProperties = GattCharacteristicProperties.Notify,
                ReadProtectionLevel = GattProtectionLevel.EncryptionRequired,
                UserDescription = "Warning Notification"
            };
            var warningNotificationCharacteristicUuid = new Guid("AB12EF34-0003-9a8b-7c6d-5e4fabcdef05");
            await _gattServiceProvider.Service.CreateCharacteristicAsync(warningNotificationCharacteristicUuid, warningNotificationParameters);


            var appIdentifier = "AB12EF34"; // Identifikátor aplikace
            byte[] appIdentifierBytes = Encoding.UTF8.GetBytes(appIdentifier);
            IBuffer serviceData = appIdentifierBytes.AsBuffer();

            _gattServiceProvider.StartAdvertising(new GattServiceProviderAdvertisingParameters
            {
                IsDiscoverable = true,
                IsConnectable = true,
                ServiceData = serviceData,
            });
        }
   

        // Event handler for when client sets a parametr
        private async void OnWriteRequested(GattLocalCharacteristic sender, GattWriteRequestedEventArgs args)
        {
            var deferral = args.GetDeferral();
            try
            {
                var responseData = new Dictionary<DataIdentifier, object>();

                var request = await args.GetRequestAsync();
                var reader = DataReader.FromBuffer(request.Value);
                byte[] receivedData = new byte[request.Value.Length];
                reader.ReadBytes(receivedData);

                // Decode the received CBOR request using CborUtility
                var (sequenceNumber, messageType, data) = CborUtility.DecodeRequest(receivedData);

                switch (messageType)
                {
                    case MessageTypeIdentifiers.Set:
                        foreach (var entry in data)
                        {
                            int id = entry.Key.Id;
                            object value = entry.Value;

                            var identifier = DataIdentifierRegistry.GetById(id);

                            if (identifier is SetDataIdentifier setDataIdentifier)
                            {
                                setDataIdentifier.Value = value;
                            }
                        }
                        request.Respond();
                        break;
                    case MessageTypeIdentifiers.Request:

                        await SendNotificationAsync([.. data.Keys], sequenceNumber);
                        //foreach (var identifier in data.Keys)
                        //{
                        //    var latestEntry = DataStorage.GetLatestData(identifier);
                        //    if (latestEntry != null)
                        //    {
                        //        responseData[identifier] = latestEntry.Value;
                        //    }
                        //}
                        //byte[] cborResponse = CborUtility.EncodeResponse(sequenceNumber, responseData);

                        //if (_notificationCharacteristic != null)
                        //{
                        //    var writer = new DataWriter();
                        //    writer.WriteBytes(cborResponse);
                        //    await _notificationCharacteristic.NotifyValueAsync(writer.DetachBuffer());
                        //}
                        request.Respond();
                        break;
                    case MessageTypeIdentifiers.SetNotification:
                        foreach (var identifier in data.Keys)
                        {
                            DataStorage.AddNotificationIdentifier(identifier);                         
                        }
                        request.Respond();
                        break;
                    default:
                        throw new InvalidOperationException($"Unsupported message type: {messageType}");
                }
            }
            finally
            {
                deferral.Complete();
            }
        }


        // Stops the GATT server and advertising
        public async Task StopGattServerAsync()
        {
            _gattServiceProvider?.StopAdvertising();          
            _gattServiceProvider = null;          
            _notificationCharacteristic = null;
        }

        // Sends notification data to the client
        public async Task SendNotificationAsync(List<DataIdentifier> identifiersToSend, int sequenceNumber)
        {
            var responseData = new Dictionary<DataIdentifier, object>();

            // Iterate over the specified identifiers and get their latest data
            foreach (var identifier in identifiersToSend)
            {
                var latestEntry = DataStorage.GetLatestData(identifier);
                if (latestEntry != null)
                {
                    responseData[identifier] = latestEntry.Value;
                }
            }

            // Encode the filtered data into CBOR format
            byte[] cborResponse = CborUtility.EncodeResponse(sequenceNumber, responseData);

            // Send the data as a notification
            if (_notificationCharacteristic != null)
            {
                var writer = new DataWriter();
                writer.WriteBytes(cborResponse);
                await _notificationCharacteristic.NotifyValueAsync(writer.DetachBuffer());
            }
        }
    }
}
