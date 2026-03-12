using Core.Domain.Entities.Organisations.People.Models;
using Core.Library.ResultPattern;

namespace App.Services.Features.Organisations.Companies;

internal sealed partial class PeopleService
{
    public Task<Result> UpdateAsync(Guid tenantId, PersonUpdateModel updateModel, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}