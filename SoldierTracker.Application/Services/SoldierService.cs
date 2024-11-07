using SoldierTracker.Application.Abstractions;
using SoldierTracker.Application.Models;
using SoldierTracker.Domain.Entities;
using SoldierTracker.Domain.Interfaces;
using System.Collections.Concurrent;

namespace SoldierTracker.Application.Services
{
    internal sealed class SoldierService : ISoldierService
    {
        private readonly ISignalRService _signalRService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Soldier> _soldierRepository;
        private readonly IRepository<SoldierLocation> _soldierLocationRepository;
        private readonly BlockingCollection<SoldierData> _incomingDataQueue = new();

        public SoldierService(ISignalRService signalRService, IUnitOfWork unitOfWork)
        {
            _signalRService = signalRService;
            _unitOfWork = unitOfWork;
            _soldierRepository = _unitOfWork.GetRepository<Soldier>();
            _soldierLocationRepository = _unitOfWork.GetRepository<SoldierLocation>();
        }

        public event EventHandler<SoldierLocationUpdateEventArgs> SoldierLocationUpdateEvent;

        public async Task<SoldierDTO?> GetSoldierByIdAsync(Guid soldierId)
        {
            var soldier = await _soldierRepository.GetFirstOrDefaultAsync(s => s.Id.Equals(soldierId), "Country,Rank");

            if (soldier == null)
            {
                return default;
            }

            var soldierDto = new SoldierDTO(
                soldier.Id,
                soldier.SoldierCode,
                soldier.Name,
                soldier.Rank.RankName,
                soldier.Country.CountryName);

            return soldierDto;
        }

        public async Task StartReceivingLocationUpdatesAsync(string topicName)
        {
            await _signalRService.StartConnectionAsync();

            // Task to wait and proccess received data
            _ = Task.Run(ProcessIncomingDataAsync);

            // Handler for data received
            _signalRService.On<SoldierData>(topicName, OnLocationReceived);
        }

        private void OnLocationReceived(SoldierData soldierData) => _incomingDataQueue.Add(soldierData);

        private async Task ProcessIncomingDataAsync()
        {
            // Waits for incoming new data to proccess
            foreach (var soldierData in _incomingDataQueue.GetConsumingEnumerable())
            {
                await SaveSoldierDataAsync(soldierData);
            }
        }

        private async Task SaveSoldierDataAsync(SoldierData soldierData)
        {
            try
            {
                var soldier = await _soldierRepository.GetFirstOrDefaultAsync(s => s.SoldierCode == soldierData.SoldierCode);

                if (soldier == null || soldierData.Latitude == default || soldierData.Longitude == default)
                {
                    return;
                }
                // TODO: Add more data validation => Add logging

                var newLocation = new SoldierLocation
                {
                    SoldierId = soldier.Id,
                    Latitude = soldierData.Latitude,
                    Longitude = soldierData.Longitude,
                    LocationTimestamp = soldierData.LocationTimestamp,
                };
                await _soldierLocationRepository.AddAsync(newLocation);
                await _unitOfWork.SaveChangesAsync();

                SoldierLocationUpdateEvent?.Invoke(this, new SoldierLocationUpdateEventArgs(soldier.Id, soldierData.Latitude, soldierData.Longitude));
            }
            catch (Exception ex)
            { 
                // TODO: Handle error in a proper way
            }
        }
    }
}
