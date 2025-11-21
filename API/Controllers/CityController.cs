using Mapster;
using Microsoft.AspNetCore.Mvc;
using Sindika.AspNet.app015.API.Models.City;
using Sindika.AspNet.app015.Application.DTOs.City;
using Sindika.AspNet.app015.Application.Interfaces.Services;
using Sindika.AspNet.Authentication.Attributes;
using Sindika.AspNet.Request;
using Sindika.AspNet.Response;

namespace Sindika.AspNet.app015.API.Controllers;

[Page("city", "City Master Data")]
[PrivateScope]
[ApiController]
[Route("api/v1/city")]
public class CityController : ControllerBase
{
    private readonly ICityService _cityService;

    public CityController(ICityService cityService)
    {
        _cityService = cityService;
    }

    [Event("insert")]
    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody] BaseRequest<CreateCityRequest> request)
    {
        var param = request.Data.Adapt<CityParam>();
        await _cityService.CreateAsync(param);
        return Ok(ResponseHelper.Success(null, "Create city successfully", _cityService.GetInfo(), request));
    }

    [Event("update")]
    [HttpPut("update/{id}")]
    public async Task<IActionResult> Update([FromBody] BaseRequest<CreateCityRequest> request, [FromRoute] Guid id)
    {
        var param = request.Data.Adapt<CityParam>();
        param.Id = id;
        await _cityService.UpdateAsync(param, id);
        return Ok(ResponseHelper.Success(null, "Update city successfully", _cityService.GetInfo(), request));
    }

    [Event("view")]
    [HttpPost("pagination")]
    public async Task<IActionResult> GetListPagination([FromBody] BaseRequest<PaginationQuery> request)
    {
        var response = await _cityService.GetPaginationAsync(request.Data);
        return Ok(ResponseHelper.Success<object>(response, "Get paginated successfully", _cityService.GetInfo()));
    }

    [Event("view")]
    [HttpGet("list/{id}")]
    public async Task<IActionResult> GetList([FromRoute] Guid id)
    {
        var response = await _cityService.GetListAsync(id);
        return Ok(ResponseHelper.Success<object>(response, "Get list successfully", _cityService.GetInfo()));
    }

    [Event("view")]
    [HttpGet("get/{id}")]
    public async Task<IActionResult> Get([FromRoute] Guid id)
    {
        var data = await _cityService.GetAsync(id);
        return Ok(ResponseHelper.Success<object>(data, "Get city successfully", _cityService.GetInfo()));
    }

    [Event("delete")]
    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var data = await _cityService.DeleteAsync(id);
        return Ok(ResponseHelper.Success<object>(data, "Delete city successfully", _cityService.GetInfo()));
    }
}
