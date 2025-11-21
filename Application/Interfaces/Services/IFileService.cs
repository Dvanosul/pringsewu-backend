using Sindika.AspNet.Common.Interfaces;

namespace Sindika.AspNet.app015.Application.Interfaces.Services
{
    public interface IFileService : IBaseService
    {
        Task UploadFile(string filePath, Stream content);
        Task<Stream> GetFile(string filePath);
    }
}
