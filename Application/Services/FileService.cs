using Sindika.AspNet.app015.Application.Interfaces.Services;
using Sindika.AspNet.Storage.Interfaces;
using Sindika.AspNet.Common.Interfaces;
using Sindika.AspNet.Common.Services;
using Sindika.AspNet.app015.Infrastructure.DataContext;

namespace Sindika.AspNet.app015.Application.Services
{
    public class FileService : BaseAppService<FileService, Context>, IFileService
    {
        private readonly IStorageService _storageService;

        public FileService(
            IConfiguration configuration,
            ILogger<FileService> logger,
            IUnitOfWork<Context> unitOfWork,
            IStorageService storageService
            ) : base(configuration, logger, unitOfWork)
        {
            _storageService = storageService;
        }

        public async Task UploadFile(string filePath, Stream content)
        {
            try
            {
                StartOperation("UPSERT");
                await _storageService.UploadFile(filePath, content);
            }
            finally
            {
                EndOperation();
            }
        }

        public async Task<Stream> GetFile(string filePath)
        {
            try
            {
                StartOperation("GET");
                var stream = await _storageService.GetFile(filePath);
                if (stream == null || stream.Length == 0)
                {
                    throw new FileNotFoundException("The requested file is not found or is empty.");
                }
                return stream;
            }
            finally
            {
                EndOperation();
            }
        }
    }
}
