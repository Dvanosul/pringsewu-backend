using Mapster;
using Microsoft.AspNetCore.Mvc;
using Sindika.AspNet.app015.API.Models.Country;
using Sindika.AspNet.app015.Application.DTOs.Country;
using Sindika.AspNet.app015.Application.Interfaces.Services;
using Sindika.AspNet.Authentication.Attributes;
using Sindika.AspNet.Request;
using Sindika.AspNet.Response;

namespace Sindika.AspNet.app015.API.Controllers;

[Page("country", "Country Master Data")]
[PrivateScope]
[ApiController]
[Route("api/v1/country")]
public class CountryController : ControllerBase
{
    private readonly ICountryService _countryService;

    public CountryController(ICountryService countryService)
    {
        _countryService = countryService;
    }

    [Event("insert")]
    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody] BaseRequest<CreateCountryRequest> request)
    {
        var param = request.Data.Adapt<CountryParam>();
        await _countryService.CreateAsync(param);
        return Ok(ResponseHelper.Success(null, "Create country successfully", _countryService.GetInfo(), request));
    }

    [Event("update")]
    [HttpPut("update/{id}")]
    public async Task<IActionResult> Update([FromBody] BaseRequest<CreateCountryRequest> request, [FromRoute] Guid id)
    {
        var param = request.Data.Adapt<CountryParam>();
        param.Id = id;
        await _countryService.UpdateAsync(param, id);
        return Ok(ResponseHelper.Success(null, "Update country successfully", _countryService.GetInfo(), request));
    }

    [Event("view")]
    [HttpPost("pagination")]
    public async Task<IActionResult> GetListPagination([FromBody] BaseRequest<PaginationQuery> request)
    {
        var response = await _countryService.GetPaginationAsync(request.Data);
        return Ok(ResponseHelper.Success<object>(response, "Get paginated successfully", _countryService.GetInfo()));
    }

    [Event("view")]
    [HttpGet("list")]
    public async Task<IActionResult> GetList()
    {
        var response = await _countryService.GetListAsync();
        return Ok(ResponseHelper.Success<object>(response, "Get list successfully", _countryService.GetInfo()));
    }


    [Event("view")]
    [HttpGet("get/{id}")]
    public async Task<IActionResult> Get([FromRoute] Guid id)
    {
        var data = await _countryService.GetAsync(id);
        return Ok(ResponseHelper.Success<object>(data, "Get country successfully", _countryService.GetInfo()));
    }

    [Event("delete")]
    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var data = await _countryService.DeleteAsync(id);
        return Ok(ResponseHelper.Success<object>(data, "Delete country successfully", _countryService.GetInfo()));
    }
}
