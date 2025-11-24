namespace Sindika.AspNet.app015.Application.DTOs.Blockchain
{
    public class BlockchainWithdrawalDTO
    {
        public string Id { get; set; } = string.Empty;
        public string EventCode { get; set; } = string.Empty;
        public double Amount { get; set; }
        public string WithdrawBy { get; set; } = string.Empty;
        public DateTime DateTime { get; set; }
        public DateTime Timestamp { get; set; }
        public string TxId { get; set; } = string.Empty;
    }

    public class BlockchainWithdrawalResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public object? Data { get; set; }
    }

    public class BlockchainWithdrawalListResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public List<BlockchainWithdrawalDTO>? Data { get; set; }
    }

    public class BlockchainWithdrawalSingleResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public BlockchainWithdrawalDTO? Data { get; set; }
    }

    public class BlockchainTotalWithdrawalResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public BlockchainTotalWithdrawalData? Data { get; set; }
    }

    public class BlockchainTotalWithdrawalData
    {
        public double Total { get; set; }
        public string? EventCode { get; set; }
    }

    public class CreateBlockchainWithdrawalRequest
    {
        public string WithdrawalId { get; set; } = string.Empty;
        public string EventCode { get; set; } = string.Empty;
        public string Amount { get; set; } = string.Empty;
        public string WithdrawBy { get; set; } = string.Empty;
        public string DateTime { get; set; } = string.Empty;
    }
}
