namespace NestLogger.Core
{
    public class DeviceReadings
    {
        public double CurrentHumidity { get; set; }
        public double CurrentTemp { get; set; }
        public double TargetTemp { get; set; }
        public int AutoAway { get; set; }

        public bool HeatState { get; set; }
    }
}