using Sindika.AspNet.app015.Application.DTOs.Blockchain;

namespace Sindika.AspNet.app015.Application.Interfaces.Services.Blockchain
{
    public interface IBlockchainEventService
    {
        Task<BlockchainEventSingleResponse> CreateEventAsync(CreateBlockchainEventRequest request);
        Task<BlockchainEventSingleResponse> GetEventAsync(string code);
        Task<BlockchainEventListResponse> GetAllEventsAsync();
        Task<BlockchainEventListResponse> GetActiveEventsAsync();
        Task<BlockchainEventSingleResponse> UpdateEventStatusAsync(string code, UpdateBlockchainEventStatusRequest request);
        Task<BlockchainEventSingleResponse> UpdateEventAsync(string code, UpdateBlockchainEventRequest request);
    }
}
