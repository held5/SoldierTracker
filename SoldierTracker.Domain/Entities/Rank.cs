namespace SoldierTracker.Domain.Entities
{
    public class Rank : BaseEntity
    {
        public string RankName { get; set; }

        public ICollection<Soldier> Soldiers { get; set; }
    }
}
