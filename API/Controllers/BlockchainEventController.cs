using Microsoft.AspNetCore.Mvc;
using Sindika.AspNet.app015.API.Models.Blockchain;
using Sindika.AspNet.app015.Application.Interfaces.Services.Blockchain;
using Sindika.AspNet.app015.Common.Helpers;
using Sindika.AspNet.Response;
using Sindika.AspNet.Request;
using Sindika.AspNet.Authentication.Attributes;

namespace Sindika.AspNet.app015.API.Controllers
{
    [Page("blockchain-event", "Blockchain event data from VaFund API")]
    [PublicScope]
    [ApiController]
    [Route("api/v1/blockchain/events")]
    public class BlockchainEventController : ControllerBase
    {
        private readonly IBlockchainEventService _blockchainEventService;

        public BlockchainEventController(IBlockchainEventService blockchainEventService)
        {
            _blockchainEventService = blockchainEventService;
        }

        [Event("insert")]
        [HttpPost("create")]
        public async Task<IActionResult> CreateEvent([FromBody] BaseRequest<CreateBlockchainEventRequest> request)
        {
            var response = await _blockchainEventService.CreateEventAsync(request.Data);
            return Ok(ResponseHelper.Success<object>(response.Data, response.Message));
        }

        [Event("view")]
        [HttpGet("get/{code}")]
        public async Task<IActionResult> GetEvent(string code)
        {
            var response = await _blockchainEventService.GetEventAsync(code);
            return Ok(ResponseHelper.Success<object>(response.Data, response.Message));
        }

        [Event("view")]
        [HttpGet("list")]
        public async Task<IActionResult> GetAllEvents()
        {
            var response = await _blockchainEventService.GetAllEventsAsync();
            return Ok(ResponseHelper.Success<object>(response.Data, response.Message));
        }

        [Event("view")]
        [HttpGet("active")]
        public async Task<IActionResult> GetActiveEvents()
        {
            var response = await _blockchainEventService.GetActiveEventsAsync();
            return Ok(ResponseHelper.Success<object>(response.Data, response.Message));
        }

        [Event("update")]
        [HttpPut("status/{code}")]
        public async Task<IActionResult> UpdateEventStatus(string code, [FromBody] BaseRequest<UpdateBlockchainEventStatusRequest> request)
        {
            var response = await _blockchainEventService.UpdateEventStatusAsync(code, request.Data);
            return Ok(ResponseHelper.Success<object>(response.Data, response.Message));
        }

        [Event("update")]
        [HttpPut("update/{code}")]
        public async Task<IActionResult> UpdateEvent(string code, [FromBody] BaseRequest<UpdateBlockchainEventRequest> request)
        {
            var response = await _blockchainEventService.UpdateEventAsync(code, request.Data);
            return Ok(ResponseHelper.Success<object>(response.Data, response.Message));
        }
    }
}
