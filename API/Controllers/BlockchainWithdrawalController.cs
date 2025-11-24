using Microsoft.AspNetCore.Mvc;
using Sindika.AspNet.app015.Application.DTOs.Blockchain;
using Sindika.AspNet.app015.Application.Interfaces.Services.Blockchain;
using Sindika.AspNet.Response;
using Sindika.AspNet.Authentication.Attributes;

namespace Sindika.AspNet.app015.API.Controllers
{
    [Page("blockchain-withdrawal", "Blockchain withdrawal data from VaFund API")]
    [PublicScope]
    [ApiController]
    [Route("api/v1/blockchain/withdrawals")]
    public class BlockchainWithdrawalController : ControllerBase
    {
        private readonly IBlockchainWithdrawalService _blockchainWithdrawalService;

        public BlockchainWithdrawalController(IBlockchainWithdrawalService blockchainWithdrawalService)
        {
            _blockchainWithdrawalService = blockchainWithdrawalService;
        }

        [Event("insert")]
        [HttpPost("create")]
        public async Task<IActionResult> CreateWithdrawal([FromBody] CreateBlockchainWithdrawalRequest request)
        {
            var response = await _blockchainWithdrawalService.CreateWithdrawalAsync(request);
            
            if (!response.Success)
            {
                return BadRequest(ResponseHelper.Success<object>(null, response.Message));
            }

            return Ok(ResponseHelper.Success<object>(response.Data, response.Message));
        }

        [Event("view")]
        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetWithdrawal(string id)
        {
            var response = await _blockchainWithdrawalService.GetWithdrawalAsync(id);
            
            if (!response.Success)
            {
                return NotFound(ResponseHelper.Success<object>(null, response.Message));
            }

            return Ok(ResponseHelper.Success<object>(response.Data, response.Message));
        }

        [Event("view")]
        [HttpGet("list")]
        public async Task<IActionResult> GetAllWithdrawals()
        {
            var response = await _blockchainWithdrawalService.GetAllWithdrawalsAsync();
            
            if (!response.Success)
            {
                return BadRequest(ResponseHelper.Success<object>(null, response.Message));
            }

            return Ok(ResponseHelper.Success<object>(response.Data, response.Message));
        }

        [Event("view")]
        [HttpGet("total")]
        public async Task<IActionResult> GetTotalWithdrawals()
        {
            var response = await _blockchainWithdrawalService.GetTotalWithdrawalsAsync();
            
            if (!response.Success)
            {
                return BadRequest(ResponseHelper.Success<object>(null, response.Message));
            }

            return Ok(ResponseHelper.Success<object>(response.Data, response.Message));
        }

        [Event("view")]
        [HttpGet("event/{eventCode}")]
        public async Task<IActionResult> GetWithdrawalsByEventCode(string eventCode)
        {
            var response = await _blockchainWithdrawalService.GetWithdrawalsByEventCodeAsync(eventCode);
            
            if (!response.Success)
            {
                return BadRequest(ResponseHelper.Success<object>(null, response.Message));
            }

            return Ok(ResponseHelper.Success<object>(response.Data, response.Message));
        }

        [Event("view")]
        [HttpGet("event/{eventCode}/total")]
        public async Task<IActionResult> GetTotalWithdrawalsByEventCode(string eventCode)
        {
            var response = await _blockchainWithdrawalService.GetTotalWithdrawalsByEventCodeAsync(eventCode);
            
            if (!response.Success)
            {
                return BadRequest(ResponseHelper.Success<object>(null, response.Message));
            }

            return Ok(ResponseHelper.Success<object>(response.Data, response.Message));
        }
    }
}
