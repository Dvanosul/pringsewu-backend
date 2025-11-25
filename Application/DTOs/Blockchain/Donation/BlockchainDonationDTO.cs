namespace Sindika.AspNet.app015.Application.DTOs.Blockchain.Donation
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
}
