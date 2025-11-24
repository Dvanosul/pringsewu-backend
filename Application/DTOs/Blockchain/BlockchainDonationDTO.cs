namespace Sindika.AspNet.app015.Application.DTOs.Blockchain
{
    public class BlockchainDonationDTO
    {
        public string Id { get; set; } = string.Empty;
        public string SenderName { get; set; } = string.Empty;
        public double Amount { get; set; }
        public string Message { get; set; } = string.Empty;
        public string EventCode { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
        public string TxId { get; set; } = string.Empty;
    }

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
    }

    public class BlockchainTotalDonationResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public double TotalAmount { get; set; }
        public double CurrentAmount { get; set; }
        public int TotalCount { get; set; }
    }

    public class CreateBlockchainDonationRequest
    {
        public string DonationId { get; set; } = string.Empty;
        public string SenderName { get; set; } = string.Empty;
        public string Amount { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string EventCode { get; set; } = string.Empty;
    }
}
