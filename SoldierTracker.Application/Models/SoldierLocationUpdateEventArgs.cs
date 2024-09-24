namespace SoldierTracker.Application.Models
{
    public record SoldierLocationUpdateEventArgs
    {
        public SoldierLocationUpdateEventArgs(Guid soldierId, decimal latitude, decimal longitude)
        {
            SoldierId = soldierId;
            Latitude = latitude;
            Longitude = longitude;
        }

        public Guid SoldierId { get; }

        public decimal Latitude { get; }

        public decimal Longitude { get; }
    }
}
