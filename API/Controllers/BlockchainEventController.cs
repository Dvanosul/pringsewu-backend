using Microsoft.AspNetCore.Mvc;
using Sindika.AspNet.app015.Application.DTOs.Blockchain;
using Sindika.AspNet.app015.Application.Interfaces.Services.Blockchain;
using Sindika.AspNet.Response;
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
        public async Task<IActionResult> CreateEvent([FromBody] CreateBlockchainEventRequest request)
        {
            var response = await _blockchainEventService.CreateEventAsync(request);
            
            if (!response.Success)
            {
                return BadRequest(ResponseHelper.Success<object>(null, response.Message));
            }

            return Ok(ResponseHelper.Success<object>(response.Data, response.Message));
        }

        [Event("view")]
        [HttpGet("get/{code}")]
        public async Task<IActionResult> GetEvent(string code)
        {
            var response = await _blockchainEventService.GetEventAsync(code);
            
            if (!response.Success)
            {
                return NotFound(ResponseHelper.Success<object>(null, response.Message));
            }

            return Ok(ResponseHelper.Success<object>(response.Data, response.Message));
        }

        [Event("view")]
        [HttpGet("list")]
        public async Task<IActionResult> GetAllEvents()
        {
            var response = await _blockchainEventService.GetAllEventsAsync();
            
            if (!response.Success)
            {
                return BadRequest(ResponseHelper.Success<object>(null, response.Message));
            }

            return Ok(ResponseHelper.Success<object>(response.Data, response.Message));
        }

        [Event("view")]
        [HttpGet("active")]
        public async Task<IActionResult> GetActiveEvents()
        {
            var response = await _blockchainEventService.GetActiveEventsAsync();
            
            if (!response.Success)
            {
                return BadRequest(ResponseHelper.Success<object>(null, response.Message));
            }

            return Ok(ResponseHelper.Success<object>(response.Data, response.Message));
        }

        [Event("update")]
        [HttpPut("status/{code}")]
        public async Task<IActionResult> UpdateEventStatus(string code, [FromBody] UpdateBlockchainEventStatusRequest request)
        {
            var response = await _blockchainEventService.UpdateEventStatusAsync(code, request);
            
            if (!response.Success)
            {
                return BadRequest(ResponseHelper.Success<object>(null, response.Message));
            }

            return Ok(ResponseHelper.Success<object>(response.Data, response.Message));
        }

        [Event("update")]
        [HttpPut("update/{code}")]
        public async Task<IActionResult> UpdateEvent(string code, [FromBody] UpdateBlockchainEventRequest request)
        {
            var response = await _blockchainEventService.UpdateEventAsync(code, request);
            
            if (!response.Success)
            {
                return BadRequest(ResponseHelper.Success<object>(null, response.Message));
            }

            return Ok(ResponseHelper.Success<object>(response.Data, response.Message));
        }
    }
}
