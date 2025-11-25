namespace Sindika.AspNet.app015.Application.DTOs.Blockchain.Event
{
    public class BlockchainEventDTO
    {
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
        public DateTime Timestamp { get; set; }
        public string TxId { get; set; } = string.Empty;
    }
}
