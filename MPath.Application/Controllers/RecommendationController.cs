using System.Net;
using Microsoft.AspNetCore.Mvc;
using MPath.Application.Commands;
using MPath.Application.Queries.Recommendations;
using MPath.Application.ResponsesDTOs.Recommendations;
using MPath.Application.Shared.Responses;
using MPath.SharedKernel.Primitive;

namespace MPath.Application.Controllers;

[ApiController]
[Route("api/recommendations")]
public class RecommendationController : ControllerBase
{
    private readonly IQueryResolver _queryResolver;
    private readonly ICommandResolver _commandResolver;
    
    public RecommendationController(IQueryResolver queryResolver, ICommandResolver commandResolver)
    {
        _queryResolver = queryResolver;
        _commandResolver = commandResolver;
    }
    
    [HttpGet("get-list-of-recommendations-by-patient-id")]
    public async Task<IActionResult> GetListOfRecommendationsByPatientId([FromQuery] Guid patientId, [FromQuery] int? page, [FromQuery] int? pageSize, [FromQuery] string? orderBy, [FromQuery] string? sortOrder, [FromQuery] string? filterField, [FromQuery] string? filterValue)
    {
        try
        {
            var query = new GetRecommendationByPatientIdQuery
            {
                PatientId = patientId,
                Page = page ?? 1,
                PageSize = pageSize ?? 10,
                OrderBy = orderBy ?? "Title",
                SortOrder = sortOrder ?? "ASC",
                FilterField = filterField,
                FilterValue = filterValue
            };
            var result = await _queryResolver.ResolveHandler<GetRecommendationByPatientIdQuery, PaginationDto<IEnumerable<GetRecommendationByPatientIdResponseDto>>>(query);
            return Ok(new BaseResponse<PaginationDto<IEnumerable<GetRecommendationByPatientIdResponseDto>>>(
                true,
                200,
                "Successfully Retrieve the List of recommendations",
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
    
    [HttpPut("mark-recommendation-as-completed")]
    public async Task<IActionResult> MarkRecommendationAsCompleted([FromQuery] Guid recommendationId, [FromQuery] Guid userId)
    {
        try
        {
            var command = new MarkRecommendationAsCompletedCommand
            (
                recommendationId,
                userId
            );
                
            await _commandResolver.ResolveHandler<MarkRecommendationAsCompletedCommand,bool>(command);
            return Ok(new BaseResponse<string>(
                true,
                200,
                "Successfully Marked the Recommendation as Completed",
                null
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
    [HttpPost("create-recommendation")]
    public async Task<IActionResult> CreateRecommendation([FromBody] CreateRecommendationCommand command)
    {
        try
        {
            await _commandResolver.ResolveHandler<CreateRecommendationCommand,CreateRecommendationCommandResponseDto>(command);
            return Ok(new BaseResponse<string>(
                true,
                200,
                "Successfully Created the Recommendation",
                null
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
    
    
    
}