using Mapster;
using Microsoft.AspNetCore.Mvc;
using Sindika.AspNet.app015.API.Models.SubDistrict;
using Sindika.AspNet.app015.Application.DTOs.SubDistrict;
using Sindika.AspNet.app015.Application.Interfaces.Services;
using Sindika.AspNet.Authentication.Attributes;
using Sindika.AspNet.Request;
using Sindika.AspNet.Response;

namespace Sindika.AspNet.app015.API.Controllers;

[Page("subdistrict", "SubDistrict Master Data")]
[PrivateScope]
[ApiController]
[Route("api/v1/subdistrict")]
public class SubDistrictController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly ISubDistrictService _subdistrictService;

    public SubDistrictController(IConfiguration configuration, ISubDistrictService subdistrictService)
    {
        _configuration = configuration;
        _subdistrictService = subdistrictService;
    }

    [Event("insert")]
    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody] BaseRequest<CreateSubDistrictRequest> request)
    {
        var param = request.Data.Adapt<SubDistrictParam>();
        await _subdistrictService.CreateAsync(param);
        return Ok(ResponseHelper.Success(null, "Create subdistrict successfully", _subdistrictService.GetInfo(), request));
    }

    [Event("update")]
    [HttpPut("update/{id}")]
    public async Task<IActionResult> Update([FromBody] BaseRequest<UpdateSubDistrictRequest> request, [FromRoute] Guid id)
    {
        var param = request.Data.Adapt<SubDistrictParam>();
        param.Id = id;
        await _subdistrictService.UpdateAsync(param, id);
        return Ok(ResponseHelper.Success(null, "Update subdistrict successfully", _subdistrictService.GetInfo(), request));
    }

    [Event("view")]
    [HttpPost("pagination")]
    public async Task<IActionResult> GetListPagination([FromBody] BaseRequest<PaginationQuery> request)
    {
        var response = await _subdistrictService.GetPaginationAsync(request.Data);
        return Ok(ResponseHelper.Success<object>(response, "Get paginated successfully", _subdistrictService.GetInfo()));
    }

    [Event("view")]
    [HttpGet("list/{districtId}")]
    public async Task<IActionResult> GetList([FromRoute] Guid districtId)
    {
        var response = await _subdistrictService.GetListAsync(districtId);
        return Ok(ResponseHelper.Success<object>(response, "Get list successfully", _subdistrictService.GetInfo()));
    }

    [Event("view")]
    [HttpGet("get/{id}")]
    public async Task<IActionResult> Get([FromRoute] Guid id)
    {
        var data = await _subdistrictService.GetAsync(id);
        return Ok(ResponseHelper.Success<object>(data, "Get subdistrict successfully", _subdistrictService.GetInfo()));
    }

    [Event("delete")]
    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var data = await _subdistrictService.DeleteAsync(id);
        return Ok(ResponseHelper.Success<object>(data, "Delete subdistrict successfully", _subdistrictService.GetInfo()));
    }
}
