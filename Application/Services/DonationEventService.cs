using Sindika.AspNet.app015.Domain.Entities;
using Sindika.AspNet.app015.Application.DTOs.DonationEvent;
using Sindika.AspNet.app015.Application.Interfaces.Repositories;
using Sindika.AspNet.app015.Application.Interfaces.Services;
using Sindika.AspNet.app015.Infrastructure.DataContext;
using Sindika.AspNet.Common.Interfaces;
using Sindika.AspNet.Common.Services;
using Sindika.AspNet.Common.Utilities;
using Sindika.AspNet.Response;
using Sindika.AspNet.Request;
using Mapster;
using Sindika.AspNet.Exceptions.NotFound;
using Sindika.AspNet.Exceptions.BadRequest;
using Microsoft.AspNetCore.Http;

namespace Sindika.AspNet.app015.Application.Services
{
    public class DonationEventService : BaseCrudService<
        DonationEventService,
        Context,
        DonationEventDTO,
        DonationEventPaginationDTO,
        CreateDonationEventParam,
        DonationEvent,
        IDonationEventRepository>, IDonationEventService
    {
        private readonly IDonationGalleryRepository _galleryRepository;
        private readonly IFileService _fileService;
        private readonly string[] _allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif" };
        private const long MaxFileSize = 5 * 1024 * 1024;

        public DonationEventService(
            IConfiguration configuration,
            ILogger<DonationEventService> logger,
            IUnitOfWork<Context> unitOfWork,
            IDonationEventRepository repository,
            IDonationGalleryRepository galleryRepository,
            IFileService fileService
        ) : base(configuration, logger, unitOfWork, repository)
        {
            _galleryRepository = galleryRepository;
            _fileService = fileService;
        }

        public async Task<Guid> CreateWithImageAsync(CreateDonationEventParam param, IFormFile image)
        {
            var validationResult = ValidateImageFile(image);
            if (!validationResult.IsValid)
            {
                throw new BadRequestException("ERR-VAL-001", validationResult.ErrorMessage);
            }

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                StartOperation("INSERT");

                if (await _repository.IsCodeExistAsync(param.Code))
                {
                    throw new BadRequestException("ERR-CTM-001", "Event code already exists");
                }

                if (param.EndDate <= param.StartDate)
                {
                    throw new BadRequestException("ERR-CTM-002", "End date must be after start date");
                }

                var entity = param.Adapt<DonationEvent>();
                entity.CreatedDate = DateTimeOffset.UtcNow;

                Guid id = await _repository.CreateAsync(entity);

                var extension = Path.GetExtension(image.FileName).ToLowerInvariant();
                var fileName = $"donation-events/{id}/image{extension}";

                using var stream = image.OpenReadStream();
                await _fileService.UploadFile(fileName, stream);

                entity.ImgUrl = fileName;
                await _repository.UpdateAsync(entity);

                AppendRecords(id.ToString());
                await _unitOfWork.CommitAsync();
                return id;
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
            finally
            {
                EndOperation();
            }
        }



        public async Task<List<DonationEventDTO>> GetActiveEventsAsync()
        {
            try
            {
                StartOperation("GET");

                var entities = await _repository.GetActiveEventsAsync();
                var result = entities.Adapt<List<DonationEventDTO>>();

                AppendRecords(null, entities.Select(e => e.Id.ToString()).ToList());
                return result;
            }
            finally
            {
                EndOperation();
            }
        }

        public async Task<Guid> UpdateWithImageAsync(UpdateDonationEventParam param, Guid id, IFormFile? image)
        {
            if (image != null)
            {
                var validationResult = ValidateImageFile(image, isRequired: false);
                if (!validationResult.IsValid)
                {
                    throw new BadRequestException("ERR-VAL-001", validationResult.ErrorMessage);
                }
            }

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                StartOperation("UPDATE");

                if (param.EndDate <= param.StartDate)
                {
                    throw new BadRequestException("ERR-CTM-002", "End date must be after start date");
                }

                var entity = await _repository.GetAsync(id) ?? throw new NotFoundException("Event not found.");

                param.Adapt(entity);
                entity.UpdatedDate = DateTimeOffset.UtcNow;

                if (image != null)
                {
                    var extension = Path.GetExtension(image.FileName).ToLowerInvariant();
                    var fileName = $"donation-events/{id}/image{extension}";

                    using var stream = image.OpenReadStream();
                    await _fileService.UploadFile(fileName, stream);

                    entity.ImgUrl = fileName;
                }

                await _repository.UpdateAsync(entity);

                AppendRecords(entity.Id.ToString());
                await _unitOfWork.CommitAsync();
                return entity.Id;
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
            finally
            {
                EndOperation();
            }
        }

        public async Task<Guid> UpdateStatusAsync(UpdateDonationEventStatusParam param, Guid id)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                StartOperation("UPDATE");

                var entity = await _repository.GetAsync(id) ?? throw new NotFoundException("Event not found.");

                entity.IsActive = param.IsActive;
                entity.UpdatedDate = DateTimeOffset.UtcNow;

                await _repository.UpdateAsync(entity);

                AppendRecords(entity.Id.ToString());
                await _unitOfWork.CommitAsync();
                return entity.Id;
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
            finally
            {
                EndOperation();
            }
        }

        public async Task<(Stream? Stream, string ContentType)> GetImageStreamAsync(Guid id)
        {
            var entity = await _repository.GetAsync(id);
            if (entity == null || string.IsNullOrEmpty(entity.ImgUrl))
            {
                return (null, string.Empty);
            }

            try
            {
                var imageStream = await _fileService.GetFile(entity.ImgUrl);
                if (imageStream == null || imageStream.Length == 0)
                {
                    return (null, string.Empty);
                }

                var extension = Path.GetExtension(entity.ImgUrl).ToLowerInvariant();
                var contentType = extension switch
                {
                    ".png" => "image/png",
                    ".gif" => "image/gif",
                    ".jpg" or ".jpeg" => "image/jpeg",
                    _ => "image/jpeg"
                };

                return (imageStream, contentType);
            }
            catch
            {
                return (null, string.Empty);
            }
        }

        public new async Task<Guid> DeleteAsync(Guid id)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                StartOperation("DELETE");

                var entity = await _repository.GetAsync(id) ?? throw new NotFoundException("Event not found.");

                var galleries = await _galleryRepository.GetByEventIdAsync(id);
                if (galleries.Any())
                {
                    throw new BadRequestException("ERR-CTM-003", "Cannot delete event with existing galleries. Please delete galleries first.");
                }

                entity.IsActive = false;
                entity.DeletedDate = DateTimeOffset.UtcNow;
                entity.DeletedBy = "";

                await _repository.UpdateAsync(entity);

                AppendRecords(entity.Id.ToString());
                await _unitOfWork.CommitAsync();
                return entity.Id;
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
            finally
            {
                EndOperation();
            }
        }

        public new async Task<PaginationResponse<DonationEventPaginationDTO>> GetPaginationAsync(PaginationQuery paginationQuery)
        {
            try
            {
                StartOperation("GET");

                var itemCountResponse = await _repository.GetPaginationAsync(paginationQuery);
                var items = itemCountResponse.Items.Adapt<List<DonationEventPaginationDTO>>();

                var eventIds = itemCountResponse.Items.Select(i => i.Id).ToList();
                foreach (var item in items)
                {
                    var galleries = await _galleryRepository.GetByEventIdAsync(item.Id);
                    item.GalleryCount = galleries.Count;
                }

                var result = PaginationUtils.GenerateResponseWithIndex(paginationQuery, items, itemCountResponse.Count);
                AppendRecords(null, eventIds.Select(id => id.ToString()).ToList());
                return result;
            }
            finally
            {
                EndOperation();
            }
        }

        public (bool IsValid, string ErrorMessage) ValidateImageFile(IFormFile? file, bool isRequired = true)
        {
            if (file == null || file.Length == 0)
            {
                return isRequired ? (false, "Image file is required") : (true, string.Empty);
            }

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!_allowedExtensions.Contains(extension))
            {
                return (false, $"Invalid file type. Allowed: {string.Join(", ", _allowedExtensions)}");
            }

            if (file.Length > MaxFileSize)
            {
                return (false, "File size exceeds 5MB limit");
            }

            return (true, string.Empty);
        }
    }
}
