using Microsoft.AspNetCore.Mvc;
using Sindika.AspNet.app015.Application.Interfaces.Services;
using Sindika.AspNet.Response;
using Sindika.AspNet.Authentication.Attributes;
using Microsoft.AspNetCore.StaticFiles;
using Sindika.AspNet.app015.API.Models.File;

namespace Sindika.AspNet.app015.API.Controllers
{
    [Page("file", "File Management")]
    [PrivateScope]
    [ApiController]
    [Route("api/v1/file")]
    public class FileController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IFileService _fileService;
        private readonly IContentTypeProvider _contentTypeProvider;

        public FileController(IConfiguration configuration, IFileService fileService, IContentTypeProvider contentTypeProvider)
        {
            _configuration = configuration;
            _fileService = fileService;
            _contentTypeProvider = contentTypeProvider;
        }

        [Event("upsert")]
        [HttpPost("upload-file")]
        public async Task<IActionResult> UploadFile([FromForm] UploadFileRequest request)
        {
            using var stream = request.File.OpenReadStream();

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(request.File.FileName)}";
            var filePath = $"{request.Folder}/{fileName}";

            await _fileService.UploadFile(filePath, stream);
            return Ok(ResponseHelper.Success<object>(filePath, "Upload file successfully"));
        }

        [Event("view")]
        [HttpGet("get-file/{filePath}")]
        [PublicScope]
        public async Task<IActionResult> GetFile(string filePath)
        {
            var stream = await _fileService.GetFile(filePath);
            var contentType = _contentTypeProvider.TryGetContentType(filePath, out var type) ? type : "application/octet-stream";
            return File(stream, contentType, Path.GetFileName(filePath));
        }
    }
}
