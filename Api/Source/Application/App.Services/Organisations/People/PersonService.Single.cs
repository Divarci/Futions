using Core.Domain.Entities.Organisations.People;
using Core.Library.ResultPattern;

namespace App.Services.Features.Organisations.People;

internal sealed partial class PersonService
{
    public async Task<Result<TDto>> GetPersonByIdAsync<TDto>(
        Guid tenantId,
        Guid personId,
        Func<Person, TDto> mapper,
        CancellationToken cancellationToken = default) where TDto : class
    {
        // Get the person by id and tenant id
        Result<Person> entityResult = await _personRepository
            .GetByIdAsync(personId, tenantId, cancellationToken);

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