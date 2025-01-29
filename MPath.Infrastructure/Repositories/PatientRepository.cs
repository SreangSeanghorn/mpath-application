using Microsoft.EntityFrameworkCore;
using MPath.Domain.Entities;
using MPath.Domain.Repositories;
using MPath.Infrastructure.Persistence.DBContext;

namespace MPath.Infrastructure.Repositories;

public class PatientRepository : GenericRepository<Patient>, IPatientRepository
{
    public PatientRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Patient> GetPatientByEmail(string email)
    {
        return await dbContext.Patients.FirstOrDefaultAsync(x => x.Email.Value == email);
    }

    public async Task<(IEnumerable<Patient> Patients, int TotalPages)> GetPatientsWithPaginationAsync(int page, int pageSize, string orderBy, string sortOrder,string filterField, string filterValue)
    {
       var query = dbContext.Patients.AsQueryable();
         if (!string.IsNullOrEmpty(filterField) && !string.IsNullOrEmpty(filterValue))
         {
              query = query.Where(x => EF.Property<string>(x, filterField).Contains(filterValue));
         }
         
            var totalPatients = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalPatients / (double)pageSize);
            
            query = sortOrder.ToUpper() == "ASC" ? query.OrderBy(x => EF.Property<string>(x, orderBy)) : query.OrderByDescending(x => EF.Property<string>(x, orderBy));
            var patients = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            return (patients, totalPages);
    }

    public async Task<(IEnumerable<Recommendation> Recommendations, int TotalPages)> GetRecommendationsByPatientId(Guid patientId, int page, int pageSize, string orderBy, string sortOrder,
        string filterField, string filterValue)
    {
        var query = dbContext.Patients.Where(x => x.Id == patientId).SelectMany(x => x.Recommendations).AsQueryable();
        if (!string.IsNullOrEmpty(filterField) && !string.IsNullOrEmpty(filterValue))
        {
            query = query.Where(x => EF.Property<string>(x, filterField).Contains(filterValue));
        }
        var totalRecommendations = await query.CountAsync();
        var totalPages = (int)Math.Ceiling(totalRecommendations / (double)pageSize);
        query = sortOrder.ToUpper() == "ASC" ? query.OrderBy(x => EF.Property<string>(x, orderBy)) : query.OrderByDescending(x => EF.Property<string>(x, orderBy));
        var recommendations = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        return (recommendations, totalPages);
    }

    public async Task<Patient> GetPatientWithRecommendations(Guid patientId)
    {
        return await dbContext.Patients.Include(x => x.Recommendations).FirstOrDefaultAsync(x => x.Id == patientId);
    }
}