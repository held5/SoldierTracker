namespace SoldierTracker.Application.Models
{
    public record SoldierDTO
    {
        public SoldierDTO(
            Guid soldierId,
            string soldierCode,
            string soldierName,
            string rankName,
            string countryName)
        {
            SoldierId = soldierId;
            SoldierCode = soldierCode;
            SoldierName = soldierName;
            RankName = rankName;
            CountryName = countryName;
        }

        public Guid SoldierId { get; }

        public string SoldierCode { get; }

        public string SoldierName { get; }

        public string RankName { get; }

        public string CountryName { get; }
    }
}
