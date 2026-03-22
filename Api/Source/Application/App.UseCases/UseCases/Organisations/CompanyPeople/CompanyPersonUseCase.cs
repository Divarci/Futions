using Core.Domain.Entities.Organisations.CompanyPeople.Interfaces;
using Core.Domain.Entities.System.AuditLogs.Interfaces;
using Core.Library.Contracts.Caching;
using Core.Library.Contracts.UnitOfWorks;
using Microsoft.Extensions.Logging;

namespace App.UseCases.UseCases.Organisations.CompanyPeople;

internal sealed partial class CompanyPersonUseCase(
    ICompanyPersonService companyPersonService,
    IAuditLogService auditLogService,
    ICacheProvider cacheProvider,
    ITransactionalUnitOfWork unitOfWork,
    ILogger<CompanyPersonUseCase> logger) : ICompanyPersonUseCase
{
    private readonly ICompanyPersonService _companyPersonService = companyPersonService;
    private readonly IAuditLogService _auditLogService = auditLogService;
    private readonly ICacheProvider _cacheProvider = cacheProvider;
    private readonly ITransactionalUnitOfWork _unitOfWork = unitOfWork;
    private readonly ILogger<CompanyPersonUseCase> _logger = logger;

    // CompanyPerson relationship data changes more frequently than master data.
    private readonly TimeSpan _timeout = TimeSpan.FromMinutes(30);
}
