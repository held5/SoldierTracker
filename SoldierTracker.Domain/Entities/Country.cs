namespace SoldierTracker.Domain.Entities
{
    public class Country : BaseEntity
    {
        public string CountryName { get; set; }

        public ICollection<Soldier> Soldiers { get; set; }
    }
}
