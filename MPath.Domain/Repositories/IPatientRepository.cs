using MPath.Domain.Core.Repositories;
using MPath.Domain.Entities;

namespace MPath.Domain.Repositories;

public interface IPatientRepository : IGenericRepository<Patient>
{
    public Task<Patient> GetPatientByEmail(string email);
    Task<(IEnumerable<Patient> Patients, int TotalPages)> GetPatientsWithPaginationAsync(int page, int pageSize, string orderBy, string sortOrder, string filterField, string filterValue);
    Task<(IEnumerable<Recommendation> Recommendations, int TotalPages)> GetRecommendationsByPatientId(Guid patientId, int page, int pageSize, string orderBy, string sortOrder,string filterField, string filterValue);
    Task<Patient> GetPatientWithRecommendations(Guid patientId);
}