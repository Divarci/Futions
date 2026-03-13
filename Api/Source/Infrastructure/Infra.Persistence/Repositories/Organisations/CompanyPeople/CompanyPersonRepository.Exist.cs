using Core.Domain.Entities.Organisations.CompanyPeople;
using Core.Library.ResultPattern;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Infra.Persistence.Repositories.Organisations.CompanyPeople;

internal sealed partial class CompanyPersonRepository
{
    public async Task<Result<bool>> HasCompanyPeopleAsync(
        Guid tenantId, 
        Guid companyId, 
        CancellationToken cancellationToken = default)
    {
        bool hasCompanyPeople = await _context.Set<CompanyPerson>()
            .AsNoTracking()
            .AnyAsync(x =>  
                x.CompanyId == companyId && 
                x.Company.TenantId == tenantId && 
                !x.Company.IsDeleted,
                cancellationToken);

        return Result<bool>.Success(
            message: hasCompanyPeople 
                ? "Company has associated people." 
                : "Company has no associated people.",
            data: hasCompanyPeople,
            statusCode: HttpStatusCode.OK);
    }
}
