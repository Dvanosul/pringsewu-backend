namespace Sindika.AspNet.app015.Application.DTOs.Blockchain.Event
{
    public class BlockchainEventResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public object? Data { get; set; }
    }

    public class BlockchainEventListResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public List<BlockchainEventDTO>? Data { get; set; }
    }

    public class BlockchainEventSingleResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public BlockchainEventDTO? Data { get; set; }
    }
}
