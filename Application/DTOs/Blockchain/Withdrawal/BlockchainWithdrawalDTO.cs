namespace Sindika.AspNet.app015.Application.DTOs.Blockchain.Withdrawal
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
}
