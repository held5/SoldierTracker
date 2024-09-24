namespace SensorHub.API.Models
{
    internal record Soldier
    {
        public Soldier(
            string soldierCode,
            string sensorName,
            decimal latitude,
            decimal longitude,
            DateTimeOffset locationTimestamp)
        {
            SoldierCode = soldierCode;
            SensorName = sensorName;
            Latitude = latitude;
            Longitude = longitude;
            LocationTimestamp = locationTimestamp;
        }

        public string SoldierCode { get; }

        public string SensorName { get; }

        public decimal Latitude { get; }

        public decimal Longitude { get; }

        public DateTimeOffset LocationTimestamp { get; }
    }
}
