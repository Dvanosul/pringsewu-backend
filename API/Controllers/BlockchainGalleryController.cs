using Microsoft.AspNetCore.Mvc;
using Sindika.AspNet.app015.API.Models.Blockchain;
using Sindika.AspNet.app015.Application.Interfaces.Services.Blockchain;
using Sindika.AspNet.app015.Common.Helpers;
using Sindika.AspNet.Response;
using Sindika.AspNet.Request;
using Sindika.AspNet.Authentication.Attributes;

namespace Sindika.AspNet.app015.API.Controllers
{
    [Page("blockchain-gallery", "Blockchain gallery data from VaFund API")]
    [PublicScope]
    [ApiController]
    [Route("api/v1/blockchain/gallery")]
    public class BlockchainGalleryController : ControllerBase
    {
        private readonly IBlockchainGalleryService _blockchainGalleryService;

        public BlockchainGalleryController(IBlockchainGalleryService blockchainGalleryService)
        {
            _blockchainGalleryService = blockchainGalleryService;
        }

        [Event("insert")]
        [HttpPost("create")]
        public async Task<IActionResult> CreateGallery([FromForm] CreateBlockchainGalleryRequest request)
        {
            var response = await _blockchainGalleryService.CreateGalleryAsync(request);
            return Ok(ResponseHelper.Success<object>(response.Data, response.Message));
        }

        [Event("view")]
        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetGallery(string id)
        {
            var response = await _blockchainGalleryService.GetGalleryAsync(id);
            return Ok(ResponseHelper.Success<object>(response.Data, response.Message));
        }

        [Event("view")]
        [HttpGet("list")]
        public async Task<IActionResult> GetAllGalleries()
        {
            var response = await _blockchainGalleryService.GetAllGalleriesAsync();
            return Ok(ResponseHelper.Success<object>(response.Data, response.Message));
        }

        [Event("view")]
        [HttpGet("event/{eventCode}")]
        public async Task<IActionResult> GetGalleriesByEventCode(string eventCode)
        {
            var response = await _blockchainGalleryService.GetGalleriesByEventCodeAsync(eventCode);
            return Ok(ResponseHelper.Success<object>(response.Data, response.Message));
        }

        [Event("view")]
        [HttpGet("{id}/image")]
        public async Task<IActionResult> GetGalleryImage(string id)
        {
            var response = await _blockchainGalleryService.GetGalleryImageAsync(id);
            if (!response.Success || response.ImageData == null)
            {
                return NotFound(ResponseHelper.Error<object, object>(null, "ERR-404", response.Message));
            }
            return File(response.ImageData, response.ContentType ?? "image/jpeg");
        }

        [Event("update")]
        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateGallery(string id, [FromForm] UpdateBlockchainGalleryRequest request)
        {
            var response = await _blockchainGalleryService.UpdateGalleryAsync(id, request);
            return Ok(ResponseHelper.Success<object>(response.Data, response.Message));
        }

        [Event("delete")]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteGallery(string id)
        {
            var response = await _blockchainGalleryService.DeleteGalleryAsync(id);
            return Ok(ResponseHelper.Success<object>(response.Data, response.Message));
        }
    }
}
