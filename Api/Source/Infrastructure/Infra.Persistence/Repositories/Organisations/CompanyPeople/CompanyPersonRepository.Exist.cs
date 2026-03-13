using Core.Domain.Entities.Organisations.CompanyPeople;
using Core.Library.ResultPattern;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Infra.Persistence.Repositories.Organisations.CompanyPeople;

internal sealed partial class CompanyPersonRepository
{
    public async Task<Result<bool>> HasPeopleAsync(
        Guid tenantId, 
        Guid companyId, 
        CancellationToken cancellationToken = default)
    {
        bool hasCompany = await _context.Set<CompanyPerson>()
            .AsNoTracking()
            .AnyAsync(x =>  
                x.CompanyId == companyId && 
                x.Company.TenantId == tenantId && 
                !x.Company.IsDeleted,
                cancellationToken);

        return Result<bool>.Success(
            message: hasCompany
                ? "Company has associated people." 
                : "Company has no associated people.",
            data: hasCompany,
            statusCode: HttpStatusCode.OK);
    }

    public async Task<Result<bool>> HasCompanyAsync(
        Guid tenantId,
        Guid personId,
        CancellationToken cancellationToken = default)
    {
        bool hasPeople = await _context.Set<CompanyPerson>()
            .AsNoTracking()
            .AnyAsync(x =>
                x.PersonId == personId &&
                x.Person.TenantId == tenantId &&
                !x.Person.IsDeleted,
                cancellationToken);

        return Result<bool>.Success(
            message: hasPeople
                ? "Person has associated companies."
                : "Person has no associated companies.",
            data: hasPeople,
            statusCode: HttpStatusCode.OK);
    }

    public async Task<Result<bool>> CheckIfBelongsToTenantAsync(
        Guid tenantId,
        Guid companyPersonId,
        CancellationToken cancellationToken = default)
    {
        bool belongsToTenant = await _context.Set<CompanyPerson>()
            .AsNoTracking()
            .AnyAsync(x =>
                x.Id == companyPersonId &&
                x.Company.TenantId == tenantId &&
                x.Person.TenantId == tenantId &&
                !x.Company.IsDeleted &&
                !x.Person.IsDeleted,
                cancellationToken);

        return Result<bool>.Success(
            message: belongsToTenant
                ? "Company person belongs to the tenant."
                : "Company person does not belong to the tenant.",
            data: belongsToTenant,
            statusCode: HttpStatusCode.OK);
    }
}