namespace Sindika.AspNet.app015.Application.DTOs.Blockchain.Withdrawal
{
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
}
