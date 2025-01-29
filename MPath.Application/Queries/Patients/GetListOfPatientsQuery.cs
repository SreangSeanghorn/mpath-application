using Azure.Core;
using MPath.Application.ResponsesDTOs.Patients;
using MPath.Application.Shared.Responses;
using MPath.SharedKernel.Primitive;

namespace MPath.Application.Queries.Patients;

public class GetListOfPatientsQuery : IQuery<PaginationDto<IEnumerable<GetListOfPatientResponseDto>>>
{
    public int Page { get; set; } 
    public int PageSize { get; set; } 
    public string OrderBy { get; set; } 
    public string SortOrder { get; set; } 
    public string FilterField { get; set; }
    public string FilterValue { get; set; }
    public string GetOrderBy()
    {
        var validOrderBy = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { 
            "Name", 
            "Email",
            "PhoneNumber",
            "Address",
            "DateOfBirth"
        }; 
        string orderBy = "Name";
        if (!string.IsNullOrEmpty(OrderBy))
        {
            if (!validOrderBy.Contains(OrderBy))
            {
                throw new InvalidOperationException("");
            }
            orderBy = OrderBy;
        }

        return orderBy;
    }
    
    public string GetSortOrder()
    {
        var validSortOrder = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "ASC", "DESC" }; 
        string sortOrder = "ASC";
        if (!string.IsNullOrEmpty(SortOrder))
        {
            if (!validSortOrder.Contains(SortOrder))
            {
                throw new InvalidOperationException("");
            }
            sortOrder = SortOrder;
        }

        return sortOrder;
    }
    
    public string GetFilterField()
    {
        var validFilter = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { 
            "Name", 
            "Email",
            "PhoneNumber",
            "Address",
            "DateOfBirth"
        }; 
        string filterField = "Name";
        if (!string.IsNullOrEmpty(FilterField))
        {
            if (!validFilter.Contains(FilterField))
            {
                throw new InvalidOperationException("Invalid FilterField field");
            }
            filterField = FilterField;
        }

        return filterField;
    }
    
}