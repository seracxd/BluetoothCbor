using BluetoothCbor.DataIdentifiers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BluetoothCbor.Services
{
    internal class GuidGenerator
    {
        private const string AppId = "AB12EF34";

        public static Guid GenerateCustomGuid(BluetoothCharakteristicIdentifiers characteristicType)
        {
            // Použijeme hodnotu výčtu jako čtyřciferný kód charakteristiky
            string characteristicCode = ((int)characteristicType).ToString("D4");

            // Vygenerujeme dva náhodné segmenty pro zajištění unikátnosti
            string randomSegment1 = Guid.NewGuid().ToString("N").Substring(0, 4);
            string randomSegment2 = Guid.NewGuid().ToString("N").Substring(0, 4);
            string randomSegment3 = Guid.NewGuid().ToString("N").Substring(0, 12);

            // Sestavíme GUID jako řetězec
            string guidString = $"{AppId}-{characteristicCode}-{randomSegment1}-{randomSegment2}-{randomSegment3}";
            return Guid.Parse(guidString);
        }
    }
}
