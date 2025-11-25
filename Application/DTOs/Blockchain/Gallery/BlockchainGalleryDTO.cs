namespace Sindika.AspNet.app015.Application.DTOs.Blockchain.Gallery
{
    public class BlockchainGalleryDTO
    {
        public string Id { get; set; } = string.Empty;
        public string EventCode { get; set; } = string.Empty;
        public string ImageURL { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
        public string TxId { get; set; } = string.Empty;
    }
}
