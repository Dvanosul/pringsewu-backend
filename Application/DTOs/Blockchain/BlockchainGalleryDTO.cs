namespace Sindika.AspNet.app015.Application.DTOs.Blockchain
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

    public class BlockchainGalleryResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public object? Data { get; set; }
    }

    public class BlockchainGalleryListResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public List<BlockchainGalleryDTO>? Data { get; set; }
    }

    public class BlockchainGallerySingleResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public BlockchainGalleryDTO? Data { get; set; }
    }

    public class CreateBlockchainGalleryRequest
    {
        public string Id { get; set; } = string.Empty;
        public string EventCode { get; set; } = string.Empty;
        public string ImageURL { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }

    public class UpdateBlockchainGalleryRequest
    {
        public string EventCode { get; set; } = string.Empty;
        public string ImageURL { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
