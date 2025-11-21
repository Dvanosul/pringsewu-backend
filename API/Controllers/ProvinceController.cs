using Mapster;
using Microsoft.AspNetCore.Mvc;
using Sindika.AspNet.app015.API.Models.Province;
using Sindika.AspNet.app015.Application.DTOs.Province;
using Sindika.AspNet.app015.Application.Interfaces.Services;
using Sindika.AspNet.Authentication.Attributes;
using Sindika.AspNet.Request;
using Sindika.AspNet.Response;

namespace Sindika.AspNet.app015.API.Controllers;

[Page("province", "Province Master Data")]
[PrivateScope]
[ApiController]
[Route("api/v1/province")]
public class ProvinceController : ControllerBase
{
    private readonly IProvinceService _provinceService;

    public ProvinceController(IProvinceService provinceService)
    {
        _provinceService = provinceService;
    }

    [Event("insert")]
    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody] BaseRequest<CreateProvinceRequest> request)
    {
        var param = request.Data.Adapt<ProvinceParam>();
        await _provinceService.CreateAsync(param);
        return Ok(ResponseHelper.Success(null, "Create province successfully", _provinceService.GetInfo(), request));
    }

    [Event("update")]
    [HttpPut("update/{id}")]
    public async Task<IActionResult> Update([FromBody] BaseRequest<CreateProvinceRequest> request, [FromRoute] Guid id)
    {
        var param = request.Data.Adapt<ProvinceParam>();
        param.Id = id;
        await _provinceService.UpdateAsync(param, id);
        return Ok(ResponseHelper.Success(null, "Update province successfully", _provinceService.GetInfo(), request));
    }

    [Event("view")]
    [HttpPost("pagination")]
    public async Task<IActionResult> GetListPagination([FromBody] BaseRequest<PaginationQuery> request)
    {
        var response = await _provinceService.GetPaginationAsync(request.Data);
        return Ok(ResponseHelper.Success<object>(response, "Get paginated successfully", _provinceService.GetInfo()));
    }

    [Event("view")]
    [HttpGet("list/{countryId}")]
    public async Task<IActionResult> GetList([FromRoute] Guid countryId)
    {
        var response = await _provinceService.GetListAsync(countryId);
        return Ok(ResponseHelper.Success<object>(response, "Get list successfully", _provinceService.GetInfo()));
    }


    [Event("view")]
    [HttpGet("get/{id}")]
    public async Task<IActionResult> Get([FromRoute] Guid id)
    {
        var data = await _provinceService.GetAsync(id);
        return Ok(ResponseHelper.Success<object>(data, "Get province successfully", _provinceService.GetInfo()));
    }

    [Event("delete")]
    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var data = await _provinceService.DeleteAsync(id);
        return Ok(ResponseHelper.Success<object>(data, "Delete province successfully", _provinceService.GetInfo()));
    }
}
