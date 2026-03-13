using Core.Domain.Entities.Organisations.People;
using Core.Domain.Entities.Organisations.People.Models;
using Core.Library.ResultPattern;
using System.Net;

namespace App.Services.Features.Organisations.Companies;

internal sealed partial class PersonService
{
    public async Task<Result> UpdateAsync(
        Guid tenantId,
        PersonUpdateModel updateModel,
        CancellationToken cancellationToken = default)
    {
        // Get the person by id and tenant id
        Result<Person> personResult = await _personRepository
            .GetByIdAsync(updateModel.PersonId, tenantId, cancellationToken);

        if (personResult.IsFailureAndNoData)
            return personResult;

        Person person = personResult.Data;

        // Update the person's properties based on the provided update model
        // If fullname is provided, update it; otherwise, keep the existing fullname
        if (updateModel.FullnameModel is not null)
        {
            Result updateFullnameResult = person
                .UpdateFullname(updateModel.FullnameModel);

            if (updateFullnameResult.IsFailure)
                return updateFullnameResult;
        }

        // If email is provided, update it; otherwise, keep the existing email
        if (!string.IsNullOrWhiteSpace(updateModel.Email))
        {
            Result updateEmailResult = person
                .UpdateEmail(updateModel.Email);

            if (updateEmailResult.IsFailure)
                return updateEmailResult;
        }

        _personRepository.Update(person);

        return Result.Success(
            message: "Person updated successfully.",
            statusCode: HttpStatusCode.OK);
    }
}