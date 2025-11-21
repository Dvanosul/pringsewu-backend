using Sindika.AspNet.app015.Application.DTOs.Location;
using Sindika.AspNet.Common.Interfaces;

namespace Sindika.AspNet.app015.Application.Interfaces.Services;

public interface ILocationService : IBaseCrudService<LocationDTO, LocationPaginationDTO, LocationParam> { }
