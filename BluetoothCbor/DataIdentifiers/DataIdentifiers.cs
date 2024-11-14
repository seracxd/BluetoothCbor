using BluetoothCbor.DataIdentifiers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BluetoothCbor.DataIdentifiers
{
    public abstract class DataIdentifier(int id, Type expectedType)
    {
        public int Id { get; } = id;
        public Type ExpectedType { get; } = expectedType;
        public override bool Equals(object obj)
        {
            if (obj is SetDataIdentifier other)
            {
                return Id == other.Id && ExpectedType == other.ExpectedType;
            }
            return false;
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }
    }

    public class RequestDataIdentifier : DataIdentifier
    {
      

        // Speed and acceleration identifiers
        /// <summary>Speed of the vehicle. Expected type: <c>float</c></summary>
        public static readonly RequestDataIdentifier Speed = new(1, typeof(float));
        /// <summary>Acceleration of the vehicle. Expected type: <c>float</c></summary>
        public static readonly RequestDataIdentifier Acceleration = new(2, typeof(float));
        /// <summary>Average speed of the vehicle. Expected type: <c>float</c></summary>
        public static readonly RequestDataIdentifier AverageSpeed = new(3, typeof(float));
        /// <summary>Maximum speed of the vehicle. Expected type: <c>float</c></summary>
        public static readonly RequestDataIdentifier MaxSpeed = new(4, typeof(float));
        /// <summary>G-Force experienced. Expected type: <c>float</c></summary>
        public static readonly RequestDataIdentifier GForce = new(5, typeof(float));
        /// <summary>Maximum G-Force experienced. Expected type: <c>float</c></summary>
        public static readonly RequestDataIdentifier MaxGForce = new(6, typeof(float));

        // Motor identifiers
        /// <summary>Engine revolutions per minute (RPM). Expected type: <c>ushort</c></summary>
        public static readonly RequestDataIdentifier EngineRPM = new(10, typeof(ushort));
        /// <summary>Engine power. Expected type: <c>ushort</c></summary>
        public static readonly RequestDataIdentifier EnginePower = new(11, typeof(ushort));
        /// <summary>Engine temperature. Expected type: <c>float</c></summary>
        public static readonly RequestDataIdentifier EngineTemperature = new(12, typeof(float));
        /// <summary>Throttle level. Expected type: <c>float</c></summary>
        public static readonly RequestDataIdentifier Throttle = new(13, typeof(float));
        /// <summary>Gear. Expected type: <c>sbyte</c></summary>
        public static readonly RequestDataIdentifier Gear = new(15, typeof(sbyte));

        // Battery identifiers
        /// <summary>Battery level. Expected type: <c>byte</c></summary>
        public static readonly RequestDataIdentifier BatteryLevel = new(20, typeof(byte));
        /// <summary>Estimated range. Expected type: <c>float</c></summary>
        public static readonly RequestDataIdentifier EstimatedRange = new(21, typeof(float));
        /// <summary>Battery temperature. Expected type: <c>float</c></summary>
        public static readonly RequestDataIdentifier BatteryTemperature = new(22, typeof(float));
        /// <summary>Time until fully charged. Expected type: <c>TimeSpan</c></summary>
        public static readonly RequestDataIdentifier ChargeTime = new(23, typeof(TimeSpan));
        /// <summary>Consumption per kilometer. Expected type: <c>float</c></summary>
        public static readonly RequestDataIdentifier ConsumptionPerKm = new(24, typeof(float));
        /// <summary>Ride smoothness. Expected type: <c>float</c></summary>
        public static readonly RequestDataIdentifier RideSmoothness = new(25, typeof(float));
        /// <summary>Consumption per kilowatt-hour (kWh). Expected type: <c>float</c></summary>
        public static readonly RequestDataIdentifier ConsumptionPerKWh = new(26, typeof(float));
        /// <summary>Battery regeneration level. Expected type: <c>float</c></summary>
        public static readonly RequestDataIdentifier BatteryRegen = new(27, typeof(float));

        // GPS identifiers
        /// <summary>GPS North coordinate. Expected type: <c>float</c></summary>
        public static readonly RequestDataIdentifier GPS_North = new(40, typeof(float));
        /// <summary>GPS East coordinate. Expected type: <c>float</c></summary>
        public static readonly RequestDataIdentifier GPS_East = new(41, typeof(float));
        /// <summary>Altitude above sea level. Expected type: <c>ushort</c></summary>
        public static readonly RequestDataIdentifier Altitude = new(42, typeof(ushort));
        /// <summary>Direction. Expected type: <c>float</c></summary>
        public static readonly RequestDataIdentifier Direction = new(43, typeof(float));
        /// <summary>Distance traveled. Expected type: <c>float</c></summary>
        public static readonly RequestDataIdentifier Distance = new(44, typeof(float));
        /// <summary>Incline angle. Expected type: <c>sbyte</c></summary>
        public static readonly RequestDataIdentifier Incline = new(45, typeof(sbyte));
        /// <summary>Gyroscope data (object placeholder). Expected type: <c>object</c></summary>
        public static readonly RequestDataIdentifier Gyroscope = new(46, typeof(object));

        // Environment identifiers
        /// <summary>Ambient temperature. Expected type: <c>float</c></summary>
        public static readonly RequestDataIdentifier AmbientTemperature = new(60, typeof(float));
        /// <summary>Humidity level. Expected type: <c>float</c></summary>
        public static readonly RequestDataIdentifier Humidity = new(61, typeof(float));
        /// <summary>Atmospheric pressure. Expected type: <c>float</c></summary>
        public static readonly RequestDataIdentifier Pressure = new(62, typeof(float));
        /// <summary>Environment time measurement. Expected type: <c>TimeSpan</c></summary>
        public static readonly RequestDataIdentifier EnvironmentTime = new(63, typeof(TimeSpan));
        /// <summary>Light level. Expected type: <c>float</c></summary>
        public static readonly RequestDataIdentifier LightLevel = new(64, typeof(float));



        private RequestDataIdentifier(int id, Type expectedType) : base(id, expectedType)
        {
            DataIdentifierRegistry.Register(this);
        }
    }

    public class SetDataIdentifier : DataIdentifier
    {
        public object Value { get; set; }

        // Control elements
        /// <summary>Power setting of the vehicle. Expected type: <c>float</c></summary>
        public static readonly SetDataIdentifier PowerSetting = new(70, typeof(float), false);
        /// <summary>Battery protection level. Expected type: <c>byte</c></summary>
        public static readonly SetDataIdentifier BatteryProtection = new(71, typeof(byte), false);
        /// <summary>Enables or disables regenerative braking. Expected type: <c>bool</c></summary>
        public static readonly SetDataIdentifier RegenerativeBraking = new(72, typeof(bool), false);
        /// <summary>Controls the lights on the vehicle. Expected type: <c>bool</c></summary>
        public static readonly SetDataIdentifier Lights = new(73, typeof(bool), false);
        /// <summary>Controls hazard lights. Expected type: <c>bool</c></summary>
        public static readonly SetDataIdentifier HazardLights = new(74, typeof(bool), false);
        /// <summary>Activates or deactivates ABS (Anti-lock Braking System). Expected type: <c>bool</c></summary>
        public static readonly SetDataIdentifier ABS = new(75, typeof(bool), true);
        /// <summary>Controls brake power setting. Expected type: <c>bool</c></summary>
        public static readonly SetDataIdentifier BrakePower = new(76, typeof(bool), false);
        /// <summary>Activates or deactivates traction control. Expected type: <c>bool</c></summary>
        public static readonly SetDataIdentifier TractionControl = new(77, typeof(bool), false);
        /// <summary>Sets the driving mode. Expected type: <c>string</c>. Consider using an enum for defined modes.</summary>
        public static readonly SetDataIdentifier DrivingMode = new(78, typeof(string), false);
        /// <summary>Sets the maximum speed limit. Expected type: <c>ushort</c></summary>
        public static readonly SetDataIdentifier MaxSpeedSetting = new(79, typeof(ushort), false);
        /// <summary>Activates or deactivates the handbrake. Expected type: <c>bool</c></summary>
        public static readonly SetDataIdentifier HandBrake = new(80, typeof(bool), false);
        /// <summary>Activates or deactivates demo mode. Expected type: <c>bool</c></summary>
        public static readonly SetDataIdentifier DemoMode = new(81, typeof(bool), false);

        // Safety features
        /// <summary>Activates or deactivates the immobilizer. Expected type: <c>bool</c></summary>
        public static readonly SetDataIdentifier Immobilizer = new(90, typeof(bool), false);
        /// <summary>Activates or deactivates the alarm. Expected type: <c>bool</c></summary>
        public static readonly SetDataIdentifier Alarm = new(91, typeof(bool), false);
        /// <summary>Sets the geofencing range. Expected type: <c>ushort</c></summary>
        public static readonly SetDataIdentifier Geofencing = new(92, typeof(ushort), false);
        /// <summary>Indicates if the device or feature is active. Expected type: <c>bool</c></summary>
        public static readonly SetDataIdentifier IsActive = new(100, typeof(bool), false);


        private SetDataIdentifier(int id, Type expectedType, object value) : base(id, expectedType)
        {
            DataIdentifierRegistry.Register(this);
            Value = value;
        }
    }
}