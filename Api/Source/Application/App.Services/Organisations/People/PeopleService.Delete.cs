using Core.Library.ResultPattern;

namespace App.Services.Features.Organisations.Companies;

internal sealed partial class PeopleService
{
    public Task<Result> DeleteAsync(Guid tenantId, Guid id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}