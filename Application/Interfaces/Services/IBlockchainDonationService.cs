using Sindika.AspNet.app015.API.Models.Blockchain;
using Sindika.AspNet.app015.Application.DTOs.Blockchain.Donation;

namespace Sindika.AspNet.app015.Application.Interfaces.Services.Blockchain
{
    public interface IBlockchainDonationService
    {
        Task<BlockchainDonationSingleResponse> CreateDonationAsync(CreateBlockchainDonationRequest request);
        Task<BlockchainDonationSingleResponse> GetDonationAsync(string id);
        Task<BlockchainDonationListResponse> GetAllDonationsAsync();
        Task<BlockchainTotalDonationResponse> GetTotalDonationsAsync();
        Task<BlockchainDonationListResponse> GetDonationsByAmountAsync(double minAmount);
        Task<BlockchainDonationListResponse> GetDonationsByEventCodeAsync(string eventCode);
        Task<BlockchainTotalDonationResponse> GetTotalDonationsByEventCodeAsync(string eventCode);
    }
}
