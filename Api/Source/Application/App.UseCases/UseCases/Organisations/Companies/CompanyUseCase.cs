using Core.Domain.Entities.Organisations.Companies.Interfaces;
using Core.Domain.Entities.System.AuditLogs.Interfaces;
using Core.Library.Contracts.Caching;
using Core.Library.Contracts.UnitOfWorks;

namespace App.UseCases.UseCases.Organisations.Companies;

internal sealed partial class CompanyUseCase(
    ICompanyService companyService,
    IAuditLogService auditLogService,
    ICacheProvider cacheProvider,
    ITransactionalUnitOfWork unitOfWork) : ICompanyUseCase
{
    private readonly ICompanyService _companyService = companyService;
    private readonly IAuditLogService _auditLogService = auditLogService;
    private readonly ICacheProvider _cacheProvider = cacheProvider;
    private readonly ITransactionalUnitOfWork _unitOfWork = unitOfWork;
}
