namespace SoldierTracker.Domain.Entities
{
    public class SoldierLocation : BaseEntity
    {
        public Guid SoldierId { get; set; }

        public Soldier Soldier { get; set; }

        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }

        public DateTimeOffset LocationTimestamp { get; set; }
    }
}
