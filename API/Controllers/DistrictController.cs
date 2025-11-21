using Mapster;
using Microsoft.AspNetCore.Mvc;
using Sindika.AspNet.app015.API.Models.District;
using Sindika.AspNet.app015.Application.DTOs.District;
using Sindika.AspNet.app015.Application.Interfaces.Services;
using Sindika.AspNet.Authentication.Attributes;
using Sindika.AspNet.Request;
using Sindika.AspNet.Response;

namespace Sindika.AspNet.app015.API.Controllers;

[Page("district", "District Master Data")]
[PrivateScope]
[ApiController]
[Route("api/v1/district")]
public class DistrictController : ControllerBase
{
    private readonly IDistrictService _districtService;

    public DistrictController(IDistrictService districtService)
    {
        _districtService = districtService;
    }

    [Event("insert")]
    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody] BaseRequest<CreateDistrictRequest> request)
    {
        var param = request.Data.Adapt<DistrictParam>();
        await _districtService.CreateAsync(param);
        return Ok(ResponseHelper.Success(null, "Create district successfully", _districtService.GetInfo(), request));
    }

    [Event("update")]
    [HttpPut("update/{id}")]
    public async Task<IActionResult> Update([FromBody] BaseRequest<CreateDistrictRequest> request, [FromRoute] Guid id)
    {
        var param = request.Data.Adapt<DistrictParam>();
        param.Id = id;
        await _districtService.UpdateAsync(param, id);
        return Ok(ResponseHelper.Success(null, "Update district successfully", _districtService.GetInfo(), request));
    }

    [Event("view")]
    [HttpPost("pagination")]
    public async Task<IActionResult> GetListPagination([FromBody] BaseRequest<PaginationQuery> request)
    {
        var response = await _districtService.GetPaginationAsync(request.Data);
        return Ok(ResponseHelper.Success<object>(response, "Get paginated successfully", _districtService.GetInfo()));
    }

    [Event("view")]
    [HttpGet("list/{cityId}")]
    public async Task<IActionResult> GetList([FromRoute] Guid cityId)
    {
        var response = await _districtService.GetListAsync(cityId);
        return Ok(ResponseHelper.Success<object>(response, "Get list successfully", _districtService.GetInfo()));
    }

    [Event("view")]
    [HttpGet("get/{id}")]
    public async Task<IActionResult> Get([FromRoute] Guid id)
    {
        var data = await _districtService.GetAsync(id);
        return Ok(ResponseHelper.Success<object>(data, "Get district successfully", _districtService.GetInfo()));
    }

    [Event("delete")]
    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var data = await _districtService.DeleteAsync(id);
        return Ok(ResponseHelper.Success<object>(data, "Delete district successfully", _districtService.GetInfo()));
    }
}
