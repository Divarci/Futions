using Core.Domain.Entities.Organisations.People;
using Core.Library.ResultPattern;

namespace App.Services.Features.Organisations.Companies;

internal sealed partial class PersonService
{
    public async Task<Result<TDto>> GetByIdAsync<TDto>(
        Guid tenantId,
        Guid id,
        Func<Person, TDto> mapper,
        CancellationToken cancellationToken = default) where TDto : class
    {
        // Get the person by id and tenant id
        Result<Person> entityResult = await _personRepository
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