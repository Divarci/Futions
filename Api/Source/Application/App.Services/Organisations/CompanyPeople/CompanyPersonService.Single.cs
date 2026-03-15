using Core.Domain.Entities.Organisations.CompanyPeople;
using Core.Library.ResultPattern;

namespace App.Services.Features.Organisations.Companies;

internal sealed partial class CompanyPersonService
{
    public async Task<Result<TDto>> GetCompanyPersonByIdAsync<TDto>(
        Guid tenantId,
        Guid companyId,
        Guid companyPersonId,
        Func<CompanyPerson, TDto> mapper,
        CancellationToken cancellationToken = default) where TDto : class
    {
        // Get the company person by id for tenant.
        Result<CompanyPerson> entityResult = await _companyPersonRepository
            .GetCompanyPersonByIdAsync(tenantId, companyId, companyPersonId,cancellationToken);

        if (entityResult.IsFailureAndNoData)
            return Result<TDto>.Failure(
                message: entityResult.Message,
                statusCode: entityResult.StatusCode);

        return Result<TDto>.Success(
            message: entityResult.Message,
            data: mapper(entityResult.Data),
            statusCode: entityResult.StatusCode);
    }
}