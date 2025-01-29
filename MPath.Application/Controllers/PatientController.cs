using System.Net;
using Microsoft.AspNetCore.Mvc;
using MPath.Application.Commands;
using MPath.Application.Queries.Patients;
using MPath.Application.ResponsesDTOs.Patients;
using MPath.Application.Shared.Responses;
using MPath.SharedKernel.Primitive;

namespace MPath.Application.Controllers;

[ApiController]
[Route("api/patients")]
public class PatientController : ControllerBase
{
    private IQueryResolver _queryResolver;
    private ICommandResolver _commandResolver;
    
    public PatientController(IQueryResolver queryResolver, ICommandResolver commandResolver)
    {
        _queryResolver = queryResolver;
        _commandResolver = commandResolver;
    }
    
    [HttpGet("get-list")]
    public async Task<IActionResult> GetListOfPatients([FromQuery] int? page, [FromQuery] int? pageSize, [FromQuery] string? orderBy, [FromQuery] string? sortOrder, [FromQuery] string? filterField, [FromQuery] string? filterValue)
    {
        try
        {
            var query = new GetListOfPatientsQuery
            {
                Page = page ?? 1,
                PageSize = pageSize ?? 10,
                OrderBy = orderBy ?? "Name",
                SortOrder = sortOrder ?? "ASC",
                FilterField = filterField ,
                FilterValue = filterValue 
            };
            var result = await _queryResolver.ResolveHandler<GetListOfPatientsQuery, PaginationDto<IEnumerable<GetListOfPatientResponseDto>>>(query); 
            return Ok(new BaseResponse<PaginationDto<IEnumerable<GetListOfPatientResponseDto>>>(
                true,
                200,
                "List of patients",
                result
                ));
        }
        catch (Exception e)
        {
            return Ok(
                new BaseResponse<string>(
                    false,
                    (int) HttpStatusCode.InternalServerError,
                    e.Message,
                    null
                ));
        }
        
    }
    
    [HttpPost("create")]
    public async Task<IActionResult> CreatePatient([FromBody] CreatePatientCommand request)
    {
        try
        {
            var result = await _commandResolver.ResolveHandler<CreatePatientCommand, PatientCreatedResponseDto>(request); 
            return Ok(new BaseResponse<PatientCreatedResponseDto>(
                true,
                200,
                "Patient created successfully",
                result
            ));
        }
        catch (Exception e)
        {
            return Ok(new BaseResponse<string>(
                false,
                (int) HttpStatusCode.InternalServerError,
                e.Message,
                null
            ));
        }
    }
    //GetPatientDetails
    [HttpGet("{id}")]
    public async Task<IActionResult> GetPatientDetails(Guid id)
    {
        try
        {
            var query = new GetPatientDetailByIdQuery(
                id
            );
            var result = await _queryResolver.ResolveHandler<GetPatientDetailByIdQuery, PatientDetailResponseDto>(query);
            return Ok(new BaseResponse<PatientDetailResponseDto>(
                true,
                200,
                "Patient details",
                result
            ));
        }
        catch (Exception e)
        {
            return Ok(new BaseResponse<string>(
                false,
                (int) HttpStatusCode.InternalServerError,
                e.Message,
                null
            ));
        }
    }
}