namespace SoldierTracker.UI.Models
{
    public record Location
    {
        public Location(decimal latitude, decimal longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        public decimal Latitude { get; }

        public decimal Longitude { get; }
    }
}
