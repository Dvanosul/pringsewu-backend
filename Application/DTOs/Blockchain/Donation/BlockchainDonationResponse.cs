using Sindika.AspNet.app015.Application.DTOs.Midtrans;

namespace Sindika.AspNet.app015.Application.DTOs.Blockchain.Donation
{
    public class BlockchainDonationResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public object? Data { get; set; }
    }

    public class BlockchainDonationListResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public List<BlockchainDonationDTO>? Data { get; set; }
    }

    public class BlockchainDonationSingleResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public BlockchainDonationDTO? Data { get; set; }
        public PaymentResponseDTO? Payment { get; set; }
    }

    public class BlockchainTotalDonationResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public double TotalAmount { get; set; }
        public double CurrentAmount { get; set; }
        public int TotalCount { get; set; }
    }
}
