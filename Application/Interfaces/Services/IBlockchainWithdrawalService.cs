using Sindika.AspNet.app015.API.Models.Blockchain;
using Sindika.AspNet.app015.Application.DTOs.Blockchain.Withdrawal;

namespace Sindika.AspNet.app015.Application.Interfaces.Services.Blockchain
{
    public interface IBlockchainWithdrawalService
    {
        Task<BlockchainWithdrawalSingleResponse> CreateWithdrawalAsync(CreateBlockchainWithdrawalRequest request);
        Task<BlockchainWithdrawalSingleResponse> GetWithdrawalAsync(string id);
        Task<BlockchainWithdrawalListResponse> GetAllWithdrawalsAsync();
        Task<BlockchainTotalWithdrawalResponse> GetTotalWithdrawalsAsync();
        Task<BlockchainWithdrawalListResponse> GetWithdrawalsByEventCodeAsync(string eventCode);
        Task<BlockchainTotalWithdrawalResponse> GetTotalWithdrawalsByEventCodeAsync(string eventCode);
    }
}
