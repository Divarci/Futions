using Core.Domain.Entities.Organisations.People.Interfaces;
using Core.Domain.Entities.System.AuditLogs.Interfaces;
using Core.Library.Contracts.Caching;
using Core.Library.Contracts.UnitOfWorks;

namespace App.UseCases.UseCases.Organisations.People;

internal sealed partial class PersonUseCase(
    IPersonService personService,
    IAuditLogService auditLogService,
    ICacheProvider cacheProvider,
    ITransactionalUnitOfWork unitOfWork) : IPersonUseCase
{
    private readonly IPersonService _personService = personService;
    private readonly IAuditLogService _auditLogService = auditLogService;
    private readonly ICacheProvider _cacheProvider = cacheProvider;
    private readonly ITransactionalUnitOfWork _unitOfWork = unitOfWork;
}
