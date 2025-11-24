namespace Sindika.AspNet.app015.Application.DTOs.Blockchain
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

    public class CreateBlockchainEventRequest
    {
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string StartDate { get; set; } = string.Empty;
        public string EndDate { get; set; } = string.Empty;
        public string IsActive { get; set; } = "true";
    }

    public class UpdateBlockchainEventStatusRequest
    {
        public string IsActive { get; set; } = string.Empty;
    }

    public class UpdateBlockchainEventRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string StartDate { get; set; } = string.Empty;
        public string EndDate { get; set; } = string.Empty;
        public string IsActive { get; set; } = string.Empty;
    }
}
