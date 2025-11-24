using Microsoft.AspNetCore.Mvc;
using Sindika.AspNet.app015.Application.DTOs.Blockchain;
using Sindika.AspNet.app015.Application.Interfaces.Services.Blockchain;
using Sindika.AspNet.Response;
using Sindika.AspNet.Authentication.Attributes;

namespace Sindika.AspNet.app015.API.Controllers
{
    [Page("blockchain-donation", "Blockchain donation data from VaFund API")]
    [PublicScope]
    [ApiController]
    [Route("api/v1/blockchain/donations")]
    public class BlockchainDonationController : ControllerBase
    {
        private readonly IBlockchainDonationService _blockchainDonationService;

        public BlockchainDonationController(IBlockchainDonationService blockchainDonationService)
        {
            _blockchainDonationService = blockchainDonationService;
        }

        [Event("insert")]
        [HttpPost("create")]
        public async Task<IActionResult> CreateDonation([FromBody] CreateBlockchainDonationRequest request)
        {
            var response = await _blockchainDonationService.CreateDonationAsync(request);
            
            if (!response.Success)
            {
                return BadRequest(ResponseHelper.Success<object>(null, response.Message));
            }

            return Ok(ResponseHelper.Success<object>(response.Data, response.Message));
        }

        [Event("view")]
        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetDonation(string id)
        {
            var response = await _blockchainDonationService.GetDonationAsync(id);
            
            if (!response.Success)
            {
                return NotFound(ResponseHelper.Success<object>(null, response.Message));
            }

            return Ok(ResponseHelper.Success<object>(response.Data, response.Message));
        }

        [Event("view")]
        [HttpGet("list")]
        public async Task<IActionResult> GetAllDonations()
        {
            var response = await _blockchainDonationService.GetAllDonationsAsync();
            
            if (!response.Success)
            {
                return BadRequest(ResponseHelper.Success<object>(null, response.Message));
            }

            return Ok(ResponseHelper.Success<object>(response.Data, response.Message));
        }

        [Event("view")]
        [HttpGet("total")]
        public async Task<IActionResult> GetTotalDonations()
        {
            var response = await _blockchainDonationService.GetTotalDonationsAsync();
            
            if (!response.Success)
            {
                return BadRequest(ResponseHelper.Success<object>(null, response.Message));
            }

            return Ok(ResponseHelper.Success<object>(new
            {
                response.TotalAmount,
                response.CurrentAmount,
                response.TotalCount
            }, response.Message));
        }

        [Event("view")]
        [HttpGet("by-amount")]
        public async Task<IActionResult> GetDonationsByAmount([FromQuery] double minAmount)
        {
            var response = await _blockchainDonationService.GetDonationsByAmountAsync(minAmount);
            
            if (!response.Success)
            {
                return BadRequest(ResponseHelper.Success<object>(null, response.Message));
            }

            return Ok(ResponseHelper.Success<object>(response.Data, response.Message));
        }

        [Event("view")]
        [HttpGet("event/{eventCode}")]
        public async Task<IActionResult> GetDonationsByEventCode(string eventCode)
        {
            var response = await _blockchainDonationService.GetDonationsByEventCodeAsync(eventCode);
            
            if (!response.Success)
            {
                return BadRequest(ResponseHelper.Success<object>(null, response.Message));
            }

            return Ok(ResponseHelper.Success<object>(response.Data, response.Message));
        }

        [Event("view")]
        [HttpGet("event/{eventCode}/total")]
        public async Task<IActionResult> GetTotalDonationsByEventCode(string eventCode)
        {
            var response = await _blockchainDonationService.GetTotalDonationsByEventCodeAsync(eventCode);
            
            if (!response.Success)
            {
                return BadRequest(ResponseHelper.Success<object>(null, response.Message));
            }

            return Ok(ResponseHelper.Success<object>(new
            {
                response.TotalAmount,
                response.CurrentAmount,
                response.TotalCount
            }, response.Message));
        }
    }
}
