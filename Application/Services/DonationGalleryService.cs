using Sindika.AspNet.app015.Domain.Entities;
using Sindika.AspNet.app015.Application.DTOs.DonationGallery;
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
    public class DonationGalleryService : BaseCrudService<
        DonationGalleryService,
        Context,
        DonationGalleryDTO,
        DonationGalleryPaginationDTO,
        DonationGalleryParam,
        DonationGallery,
        IDonationGalleryRepository>, IDonationGalleryService
    {
        private readonly IDonationEventRepository _eventRepository;
        private readonly IFileService _fileService;
        private readonly string[] _allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif" };
        private const long MaxFileSize = 5 * 1024 * 1024;

        public DonationGalleryService(
            IConfiguration configuration,
            ILogger<DonationGalleryService> logger,
            IUnitOfWork<Context> unitOfWork,
            IDonationGalleryRepository repository,
            IDonationEventRepository eventRepository,
            IFileService fileService
        ) : base(configuration, logger, unitOfWork, repository)
        {
            _eventRepository = eventRepository;
            _fileService = fileService;
        }

        public new async Task<Guid> CreateAsync(DonationGalleryParam param)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                StartOperation("INSERT");

                var eventEntity = await _eventRepository.GetAsync(param.EventId)
                    ?? throw new NotFoundException("Event not found.");

                var entity = param.Adapt<DonationGallery>();
                entity.CreatedDate = DateTimeOffset.UtcNow;

                Guid id = await _repository.CreateAsync(entity);

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

        public async Task<Guid> CreateWithImageAsync(DonationGalleryParam param, IFormFile image)
        {
            var validationResult = ValidateImageFile(image);
            if (!validationResult.IsValid)
            {
                throw new BadRequestException("ERR-VAL-001", validationResult.ErrorMessage);
            }

            var eventEntity = await _eventRepository.GetAsync(param.EventId)
                ?? throw new NotFoundException("Event not found.");

            var id = await CreateAsync(param);

            var extension = Path.GetExtension(image.FileName).ToLowerInvariant();
            var fileName = $"donation-galleries/{id}/image{extension}";

            using var stream = image.OpenReadStream();
            await _fileService.UploadFile(fileName, stream);

            await UpdateImageAsync(id, fileName);

            return id;
        }

        public async Task<List<DonationGalleryDTO>> GetByEventIdAsync(Guid eventId)
        {
            try
            {
                StartOperation("GET");

                var entities = await _repository.GetByEventIdAsync(eventId);
                var result = entities.Select(e => MapToDTO(e)).ToList();

                AppendRecords(null, entities.Select(e => e.Id.ToString()).ToList());
                return result;
            }
            finally
            {
                EndOperation();
            }
        }

        public async Task<List<DonationGalleryDTO>> GetByEventCodeAsync(string eventCode)
        {
            try
            {
                StartOperation("GET");

                var entities = await _repository.GetByEventCodeAsync(eventCode);
                var result = entities.Select(e => MapToDTO(e)).ToList();

                AppendRecords(null, entities.Select(e => e.Id.ToString()).ToList());
                return result;
            }
            finally
            {
                EndOperation();
            }
        }

        public new async Task<Guid> UpdateAsync(DonationGalleryParam param, Guid id)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                StartOperation("UPDATE");

                var entity = await _repository.GetAsync(id) ?? throw new NotFoundException("Gallery not found.");

                var eventEntity = await _eventRepository.GetAsync(param.EventId)
                    ?? throw new NotFoundException("Event not found.");

                param.Adapt(entity);
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

        public async Task<Guid> UpdateWithImageAsync(DonationGalleryParam param, Guid id, IFormFile? image)
        {
            var existingGallery = await _repository.GetAsync(id)
                ?? throw new NotFoundException("Gallery not found.");

            if (image != null)
            {
                var validationResult = ValidateImageFile(image, isRequired: false);
                if (!validationResult.IsValid)
                {
                    throw new BadRequestException("ERR-VAL-001", validationResult.ErrorMessage);
                }
            }

            var eventEntity = await _eventRepository.GetAsync(param.EventId)
                ?? throw new NotFoundException("Event not found.");

            await UpdateAsync(param, id);

            if (image != null)
            {
                var extension = Path.GetExtension(image.FileName).ToLowerInvariant();
                var fileName = $"donation-galleries/{id}/image{extension}";

                using var stream = image.OpenReadStream();
                await _fileService.UploadFile(fileName, stream);

                await UpdateImageAsync(id, fileName);
            }

            return id;
        }

        public async Task<Guid> UpdateImageAsync(Guid id, string imgUrl)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                StartOperation("UPDATE");

                var entity = await _repository.GetAsync(id) ?? throw new NotFoundException("Gallery not found.");

                entity.ImgUrl = imgUrl;
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

        public new async Task<Guid> DeleteAsync(Guid id)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                StartOperation("DELETE");

                var entity = await _repository.GetAsync(id) ?? throw new NotFoundException("Gallery not found.");

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

        public new async Task<DonationGalleryDTO> GetAsync(Guid id)
        {
            try
            {
                StartOperation("GET");

                var entity = await _repository.GetAsync(id)
                    ?? throw new NotFoundException($"Gallery with ID {id} not found");

                var eventEntity = await _eventRepository.GetAsync(entity.EventId);

                var result = entity.Adapt<DonationGalleryDTO>();
                result.EventCode = eventEntity?.Code ?? string.Empty;
                result.EventName = eventEntity?.Name ?? string.Empty;

                AppendRecords(entity.Id.ToString());
                return result;
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

        public new async Task<PaginationResponse<DonationGalleryPaginationDTO>> GetPaginationAsync(PaginationQuery paginationQuery)
        {
            try
            {
                StartOperation("GET");

                var itemCountResponse = await _repository.GetPaginationAsync(paginationQuery);
                var items = new List<DonationGalleryPaginationDTO>();

                foreach (var item in itemCountResponse.Items)
                {
                    var dto = item.Adapt<DonationGalleryPaginationDTO>();
                    var eventEntity = await _eventRepository.GetAsync(item.EventId);
                    dto.EventCode = eventEntity?.Code ?? string.Empty;
                    dto.EventName = eventEntity?.Name ?? string.Empty;
                    items.Add(dto);
                }

                var result = PaginationUtils.GenerateResponseWithIndex(paginationQuery, items, itemCountResponse.Count);
                AppendRecords(null, itemCountResponse.Items.Select(i => i.Id.ToString()).ToList());
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

        private DonationGalleryDTO MapToDTO(DonationGallery entity)
        {
            var dto = entity.Adapt<DonationGalleryDTO>();
            dto.EventCode = entity.Event?.Code ?? string.Empty;
            dto.EventName = entity.Event?.Name ?? string.Empty;
            return dto;
        }
    }
}
