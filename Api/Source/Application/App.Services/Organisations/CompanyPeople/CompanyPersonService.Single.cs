using Core.Domain.Entities.Organisations.CompanyPeople;
using Core.Library.ResultPattern;

namespace App.Services.Features.Organisations.Companies;

internal sealed partial class CompanyPersonService
{
    public async Task<Result<TDto>> GetByIdAsync<TDto>(
        Guid tenantId,
        Guid id,
        Func<CompanyPerson, TDto> mapper,
        CancellationToken cancellationToken = default) where TDto : class
    {
        // Get the company person by id for tenant.
        Result<CompanyPerson> entityResult = await _companyPersonRepository
            .GetByIdAsync(tenantId, id, cancellationToken);

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