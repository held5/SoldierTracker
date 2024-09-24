namespace SoldierTracker.Domain.Entities
{
    public class Soldier : BaseEntity
    {
        public string SoldierCode { get; set; }

        public string Name { get; set; }

        public Guid RankID { get; set; }

        public Rank Rank { get; set; }

        public Guid CountryID { get; set; }

        public Country Country { get; set; }

        public string? TrainingInfo { get; set; }

        public ICollection<Sensor> Sensors { get; set; }

        public ICollection<SoldierLocation> SoldierLocations { get; set; }
    }
}
