using System.Collections.ObjectModel;

namespace SoldierTracker.UI.Models
{
    public class SoldierInfo
    {
        public Guid SoldierId { get; set; }

        public ObservableCollection<Location> Locations { get; set; }
    }
}
