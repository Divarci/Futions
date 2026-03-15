using Core.Domain.Entities.Organisations.CompanyPeople;
using Core.Library.ResultPattern;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Infra.Persistence.Repositories.Organisations.CompanyPeople;

internal sealed partial class CompanyPersonRepository
{
    public async Task<Result<CompanyPerson>> GetCompanyPersonByIdAsync(
        Guid tenantId,
        Guid companyId,
        Guid companyPersonId,
        CancellationToken cancellationToken = default)
    {
        CompanyPerson? companyPerson = await _context.Set<CompanyPerson>()
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.Id == companyPersonId && 
                x.Company.TenantId == tenantId && 
                x.Person.TenantId == tenantId && 
                x.CompanyId == companyId &&
                !x.Company.IsDeleted && 
                !x.Person.IsDeleted, 
                cancellationToken);

        if (companyPerson is null)
            return Result<CompanyPerson>.Failure(
                message: "Company person not found.",
                statusCode: HttpStatusCode.NotFound);

        return Result<CompanyPerson>.Success(
            message: "Company person retrieved successfully.",
            data: companyPerson,
            statusCode: HttpStatusCode.OK);
    }
}
