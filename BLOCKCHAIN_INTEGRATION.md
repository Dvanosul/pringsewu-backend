# VaFund Blockchain Integration - .NET Backend

## Overview
This integration allows the .NET backend (pringsewu-backend) to fetch blockchain data from the Go VaFund API, which acts as a gateway to the Hyperledger Fabric network.

## Architecture

```
Backend .NET (pringsewu-backend)
    ↓ HTTP Requests
Backend Go (vafund-backend) 
    ↓ Fabric Gateway SDK
Hyperledger Fabric Network
    ↓ Chaincode
VaFund Smart Contract
```

## Files Created

### DTOs (Data Transfer Objects)
Location: `Application/DTOs/Blockchain/`

#### 1. BlockchainDonationDTO.cs
```csharp
- BlockchainDonationDTO              // Main donation data
- BlockchainDonationResponse         // API response wrapper
- BlockchainDonationsResponse        // Response for multiple donations
- BlockchainTotalDonationsResponse   // Aggregate total response
- CreateBlockchainDonationRequest    // Request to create a donation
```

#### 2. BlockchainEventDTO.cs
```csharp
- BlockchainEventDTO                  // Main event data
- BlockchainEventResponse             // API response wrapper
- BlockchainEventsResponse            // Response for multiple events
- CreateBlockchainEventRequest        // Request to create an event
- UpdateBlockchainEventRequest        // Request to update an event
- UpdateBlockchainEventStatusRequest  // Request to update event status
```

#### 3. BlockchainWithdrawalDTO.cs
```csharp
- BlockchainWithdrawalDTO             // Main withdrawal data
- BlockchainWithdrawalResponse        // API response wrapper
- BlockchainWithdrawalListResponse    // Response for multiple withdrawals
- BlockchainTotalWithdrawalResponse   // Total response
- BlockchainTotalWithdrawalData       // Total data (Total, EventCode)
- CreateBlockchainWithdrawalRequest   // Request to create a withdrawal
```

#### 4. BlockchainGalleryDTO.cs
```csharp
- BlockchainGalleryDTO                // Main gallery data
- BlockchainGalleryResponse           // API response wrapper
- BlockchainGalleriesResponse         // Response for multiple galleries
- CreateBlockchainGalleryRequest      // Request to create a gallery
- UpdateBlockchainGalleryRequest      // Request to update a gallery
```

### Service Interfaces
Location: `Application/Interfaces/Services/`

#### IBlockchainDonationService.cs (7 methods)
```csharp
Task<BlockchainDonationResponse> CreateDonationAsync(CreateBlockchainDonationRequest request);
Task<BlockchainDonationSingleResponse> GetDonationAsync(string id);
Task<BlockchainDonationsResponse> GetAllDonationsAsync();
Task<BlockchainTotalDonationsResponse> GetTotalDonationsAsync();
Task<BlockchainDonationsResponse> GetDonationsByAmountAsync(double minAmount);
Task<BlockchainDonationsResponse> GetDonationsByEventCodeAsync(string eventCode);
Task<BlockchainTotalDonationsResponse> GetTotalDonationsByEventCodeAsync(string eventCode);
```

#### IBlockchainEventService.cs (6 methods)
```csharp
Task<BlockchainEventResponse> CreateEventAsync(CreateBlockchainEventRequest request);
Task<BlockchainEventSingleResponse> GetEventAsync(string code);
Task<BlockchainEventsResponse> GetAllEventsAsync();
Task<BlockchainEventsResponse> GetActiveEventsAsync();
Task<BlockchainEventResponse> UpdateEventStatusAsync(string code, UpdateBlockchainEventStatusRequest request);
Task<BlockchainEventResponse> UpdateEventAsync(string code, UpdateBlockchainEventRequest request);
```

#### IBlockchainWithdrawalService.cs (6 methods)
```csharp
Task<BlockchainWithdrawalResponse> CreateWithdrawalAsync(CreateBlockchainWithdrawalRequest request);
Task<BlockchainWithdrawalSingleResponse> GetWithdrawalAsync(string id);
Task<BlockchainWithdrawalListResponse> GetAllWithdrawalsAsync();
Task<BlockchainTotalWithdrawalResponse> GetTotalWithdrawalsAsync();
Task<BlockchainWithdrawalListResponse> GetWithdrawalsByEventCodeAsync(string eventCode);
Task<BlockchainTotalWithdrawalResponse> GetTotalWithdrawalsByEventCodeAsync(string eventCode);
```

#### IBlockchainGalleryService.cs (6 methods)
```csharp
Task<BlockchainGalleryResponse> CreateGalleryAsync(CreateBlockchainGalleryRequest request);
Task<BlockchainGallerySingleResponse> GetGalleryAsync(string id);
Task<BlockchainGalleriesResponse> GetAllGalleriesAsync();
Task<BlockchainGalleriesResponse> GetGalleriesByEventCodeAsync(string eventCode);
Task<BlockchainGalleryResponse> UpdateGalleryAsync(string id, UpdateBlockchainGalleryRequest request);
Task<BlockchainGalleryResponse> DeleteGalleryAsync(string id);
```

### Service Implementations
Location: `Application/Services/`

All services use:
- IHttpClientFactory for HTTP requests
- IConfiguration to read the base URL from appsettings
- ILogger<T> for error logging
- JsonSerializer with PropertyNameCaseInsensitive = true
- Try-catch for error handling

### Controllers
Location: `API/Controllers/`

#### 1. BlockchainDonationController.cs
Route: `/api/v1/blockchain/donations`

Endpoints:
- `POST /` - Create a new donation
- `GET /` - Get all donations
- `GET /{id}` - Get donation by ID
- `GET /total` - Get donations total (TotalAmount, CurrentAmount, TotalCount)
- `GET /by-amount?minAmount={amount}` - Get donations >= specified amount
- `GET /event/{eventCode}` - Get donations by event code
- `GET /event/{eventCode}/total` - Get total donations by event code

#### 2. BlockchainEventController.cs
Route: `/api/v1/blockchain/events`

Note: This controller is separate from the existing EventController.

Endpoints:
- `POST /` - Create a new event
- `GET /` - Get all events
- `GET /{code}` - Get event by code
- `GET /active` - Get active events
- `PUT /{code}` - Update event
- `PUT /{code}/status` - Update event status

#### 3. BlockchainWithdrawalController.cs
Route: `/api/v1/blockchain/withdrawals`

Endpoints:
- `POST /` - Create a new withdrawal
- `GET /` - Get all withdrawals
- `GET /{id}` - Get withdrawal by ID
- `GET /total` - Get total withdrawals (Total)
- `GET /event/{eventCode}` - Get withdrawals by event code
- `GET /event/{eventCode}/total` - Get total withdrawals by event code (Total, EventCode)

#### 4. BlockchainGalleryController.cs
Route: `/api/v1/blockchain/gallery`

Endpoints:
- `POST /` - Create a new gallery item
- `GET /` - Get all gallery items
- `GET /{id}` - Get gallery item by ID
- `GET /event/{eventCode}` - Get gallery items by event code
- `PUT /{id}` - Update gallery item
- `DELETE /{id}` - Delete gallery item

## Configuration

### 1. Dependency Injection
File: `Extensions/ServiceExtensions.cs`

```csharp
// Blockchain services
services.AddScoped<IBlockchainDonationService, BlockchainDonationService>();
services.AddScoped<IBlockchainEventService, BlockchainEventService>();
services.AddScoped<IBlockchainWithdrawalService, BlockchainWithdrawalService>();
services.AddScoped<IBlockchainGalleryService, BlockchainGalleryService>();
```

### 2. Configuration Settings
File: `appsettings.json` and `appsettings.Development.json`

```json
{
  "VaFundApi": {
    "BaseUrl": "http://localhost:3000"
  }
}
```

Notes:
- For production, replace with the appropriate server URL
- Ensure the Go VaFund API is running at that URL

## How to Use

### 1. Run Go VaFund API
```bash
cd vafund-backend
go run main.go
```

Ensure the API runs at `http://localhost:3000` and is connected to the Fabric network.

### 2. Run .NET Backend
```bash
cd pringsewu-backend
dotnet run
```

### 3. Access Endpoints
All endpoints require authentication (Keycloak JWT token).

Example Requests:

#### Get All Donations
```http
GET http://localhost:5000/api/v1/blockchain/donations
Authorization: Bearer <your-token>
```

#### Create a New Donation
```http
POST http://localhost:5000/api/v1/blockchain/donations
Authorization: Bearer <your-token>
Content-Type: application/json

{
  "donationCode": "DON20250101001",
  "eventCode": "EVT001",
  "amount": "500000",
  "donorName": "Ahmad Ibrahim",
  "timestamp": 1704067200
}
```

#### Get Total Donations for a Specific Event
```http
GET http://localhost:5000/api/v1/blockchain/donations/event/EVT001/total
Authorization: Bearer <your-token>
```

Response:
```json
{
  "success": true,
  "message": "Total donations for event EVT001 retrieved successfully",
  "data": {
    "totalAmount": 10000000,
    "currentAmount": 8500000,
    "totalCount": 25
  }
}
```

#### Get Withdrawals for a Specific Event
```http
GET http://localhost:5000/api/v1/blockchain/withdrawals/event/EVT001
Authorization: Bearer <your-token>
```

#### Get Active Events
```http
GET http://localhost:5000/api/v1/blockchain/events/active
Authorization: Bearer <your-token>
```

## Technical Details

### Patterns Used
1. HttpClient-based communication - all services use `IHttpClientFactory`
2. Async/await - all methods are asynchronous
3. Error handling - try-catch with `ILogger` for exception logging
4. Case-insensitive JSON - deserializer configured with `PropertyNameCaseInsensitive = true`
5. Response wrapping - using `ResponseHelper.Success<object>` for consistency
6. Attribute-based routing - `[Page]`, `[Event]`, `[PrivateScope]` according to project standards

### Naming Convention
- All blockchain components use the "Blockchain" prefix to avoid conflicts with existing domain entities
- Controllers follow the existing patterns with attributes `[Page]`, `[Event]`, `[PrivateScope]`

### Differences in Response Structure

#### Donation Totals (more detailed):
```json
{
  "totalAmount": 10000000,
  "currentAmount": 8500000,
  "totalCount": 25
}
```
- totalAmount: Sum of all donations
- currentAmount: Amount after withdrawals
- totalCount: Number of donation records

#### Withdrawal Totals (simpler):
```json
{
  "total": 1500000,
  "eventCode": "EVT001"  // only present in the by-event endpoint
}
```
- total: Total withdrawals
- eventCode: Event code (optional, only in by-event endpoint)

## Security

1. Authentication: All endpoints use `[PrivateScope]` — require Keycloak JWT token
2. Authorization: Controllers use `[Page]` and `[Event]` attributes for permission checks
3. HTTPS: Use HTTPS in production for communication with the Go API

## Error Handling

Service layer uses this pattern:
```csharp
try 
{
    // HTTP call to Go API
}
catch (Exception ex)
{
    _logger.LogError(ex, "Error message");
    return new Response 
    {
        Success = false,
        Message = "User-friendly error message"
    };
}
```

Controllers return:
- 200 OK - Successful requests
- 400 BadRequest - Validation error or API error
- 404 NotFound - Resource not found
- 500 InternalServerError - Server error (handled by middleware)

## Monitoring & Logging

All blockchain services use `ILogger<T>` for logging:
- HTTP request errors
- Deserialization errors
- Connection failures

Logs can be viewed in the console or file as configured by Serilog in `appsettings.json`.

## Testing

### Manual Testing with cURL

#### Test Connection
```bash
curl -X GET http://localhost:3000/api/donations
```

#### Test from .NET
```bash
curl -X GET http://localhost:5000/api/v1/blockchain/donations \
  -H "Authorization: Bearer <token>"
```

### Troubleshooting

Error: "Connection refused"
- Ensure the Go VaFund API is running on port 3000
- Check `VaFundApi:BaseUrl` in appsettings.json

Error: "Unauthorized"
- Ensure the JWT token is valid
- Check Keycloak configuration

Error: "Donation not found"
- Ensure chaincode is deployed
- Check ledger data with peer CLI

## Development Notes

1. No Domain Entities — per recommendation ("Only need request models, no entity"), this integration uses only DTOs
2. Separate Controller — `BlockchainEventController` is separate from existing `EventController`
3. HttpClientFactory — registered in `Program.cs`, services can inject `IHttpClientFactory`
4. Configuration — base URL can be changed per environment (Development, Staging, Production)

## Roadmap / Future Improvements

1. ✅ Basic integration with Go API
2. ⏳ Retry policy for transient HTTP failures
3. ⏳ Circuit breaker pattern
4. ⏳ Integration tests
5. ⏳ Swagger/OpenAPI documentation
6. ⏳ Response caching for read operations
7. ⏳ Bulk operations support
8. ⏳ Webhook notifications from blockchain events

## References

- Go VaFund API: `vafund-backend/` - gateway to Hyperledger Fabric
- Smart Contract: `vafund-smartcontract/chaincode/` - chaincode running on Fabric
- .NET Standards: follows patterns from existing controllers in `pringsewu-backend`

---
