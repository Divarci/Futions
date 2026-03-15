using Core.Domain.Entities.Organisations.Companies;
using Core.Library.ResultPattern;

namespace App.Services.Features.Organisations.Companies;

internal sealed partial class CompanyService
{
    public async Task<Result<TDto>> GetByIdAsync<TDto>(
        Guid tenantId, 
        Guid id, 
        Func<Company, TDto> mapper,
        CancellationToken cancellationToken = default) where TDto : class
    {
        // Get the company by id and tenant id
        Result<Company> entityResult = await _companyRepository
            .GetByIdAsync(id, tenantId, cancellationToken);

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