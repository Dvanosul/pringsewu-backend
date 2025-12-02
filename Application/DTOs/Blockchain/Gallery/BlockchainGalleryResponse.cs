namespace Sindika.AspNet.app015.Application.DTOs.Blockchain.Gallery
{
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

    public class BlockchainGalleryImageResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public byte[]? ImageData { get; set; }
        public string? ContentType { get; set; }
    }
}
