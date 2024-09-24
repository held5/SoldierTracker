namespace SoldierTracker.Domain.Entities
{
    public class Sensor : BaseEntity
    {
        public string SensorName { get; set; }

        public string SensorType { get; set; }

        public Guid? SoldierID { get; set; }

        public Soldier? Soldier { get; set; }
    }
}
