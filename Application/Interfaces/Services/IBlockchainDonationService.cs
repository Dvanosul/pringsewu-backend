using Sindika.AspNet.app015.API.Models.Blockchain;
using Sindika.AspNet.app015.Application.DTOs.Blockchain.Donation;
using Sindika.AspNet.app015.Application.DTOs.Midtrans;

namespace Sindika.AspNet.app015.Application.Interfaces.Services.Blockchain
{
    public interface IBlockchainDonationService
    {
        /// <summary>
        /// Creates a payment for donation. Donation will be created in blockchain after payment is confirmed via webhook.
        /// </summary>
        Task<PaymentResponseDTO> CreateDonationPaymentAsync(CreateBlockchainDonationRequest request);
        
        /// <summary>
        /// Creates donation directly in blockchain (called by webhook after payment confirmed).
        /// </summary>
        Task<BlockchainDonationSingleResponse> CreateDonationInBlockchainAsync(PendingDonationDTO pendingDonation);
        
        Task<BlockchainDonationSingleResponse> GetDonationAsync(string id);
        Task<BlockchainDonationListResponse> GetAllDonationsAsync();
        Task<BlockchainTotalDonationResponse> GetTotalDonationsAsync();
        Task<BlockchainDonationListResponse> GetDonationsByAmountAsync(double minAmount);
        Task<BlockchainDonationListResponse> GetDonationsByEventCodeAsync(string eventCode);
        Task<BlockchainTotalDonationResponse> GetTotalDonationsByEventCodeAsync(string eventCode);
    }
}
