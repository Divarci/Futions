using Core.Domain.Entities.Organisations.CompanyPeople;
using Core.Library.ResultPattern;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Infra.Persistence.Repositories.Organisations.CompanyPeople;

internal sealed partial class CompanyPersonRepository
{
    public async Task<Result<int>> CountCompanyPeopleAsync(
        Guid tenantId,
        Guid companyId,
        CancellationToken cancellationToken = default)
    {
        int companyPeopleCount = await _context.Set<CompanyPerson>()
            .AsNoTracking()
            .CountAsync(x =>
                x.Company.TenantId == tenantId &&
                x.Person.TenantId == tenantId &&
                x.CompanyId == companyId &&
                !x.Person.IsDeleted &&
                !x.Company.IsDeleted, cancellationToken);

        return Result<int>.Success(
            message: "Company people count retrieved successfully.",
            data: companyPeopleCount,
            statusCode: HttpStatusCode.OK);
    }
}
