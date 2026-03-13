using Core.Domain.Entities.Organisations.Products.Interfaces;
using Core.Domain.Entities.System.AuditLogs.Interfaces;
using Core.Library.Contracts.Caching;
using Core.Library.Contracts.UnitOfWorks;

namespace App.UseCases.UseCases.Organisations.Products;

internal sealed partial class ProductUseCase(
    IProductService productService,
    IAuditLogService auditLogService,
    ICacheProvider cacheProvider,
    ITransactionalUnitOfWork unitOfWork) : IProductUseCase
{
    private readonly IProductService _productService = productService;
    private readonly IAuditLogService _auditLogService = auditLogService;
    private readonly ICacheProvider _cacheProvider = cacheProvider;
    private readonly ITransactionalUnitOfWork _unitOfWork = unitOfWork;
}
