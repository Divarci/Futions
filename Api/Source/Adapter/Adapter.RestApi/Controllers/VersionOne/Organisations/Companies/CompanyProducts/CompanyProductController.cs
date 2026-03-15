using Adapter.RestApi.Controllers.Shared.Mappers;
using Adapter.RestApi.Controllers.Shared.Models;
using Adapter.RestApi.Controllers.VersionOne.Organisations.Companies.CompanyProducts.Models.Requests;
using Adapter.RestApi.Controllers.VersionOne.Organisations.Companies.CompanyProducts.Models.Responses;
using Asp.Versioning;
using Core.Domain.Entities.Organisations.Products;
using Core.Domain.Entities.Organisations.Products.Interfaces;
using Core.Domain.Entities.Organisations.Products.Models;
using Core.Domain.ValueObjects.AuditStampValueObject;
using Core.Library.ResultPattern;
using Microsoft.AspNetCore.Mvc;

namespace Adapter.RestApi.Controllers.VersionOne.Organisations.Companies.CompanyProducts;

[ApiVersion(ApiVersion.V1)]
[Route("api/v{version:apiVersion}/tenants/{tenantId:guid}/companies/{companyId:guid}/products")]
[ApiController]
public class CompanyProductController(
    IProductUseCase productUseCase) : BaseController
{
    private readonly IProductUseCase _productUseCase = productUseCase;

    [HttpGet]
    public async Task<IActionResult> GetCompanyProductsAsync(
        Guid tenantId,
        Guid companyId,
        [FromQuery] PaginationFilterModel filter,
        CancellationToken cancellationToken)
    {
        PaginatedResult<CompanyProductResponse[]> paginatedProducts = await _productUseCase.GetPaginatedCompanyProductsAsync(
            tenantId,
            companyId,
            filter.Page,
            filter.PageSize,
            filter.SortBy,
            filter.IsAscending,
            filter.Filter,
            CompanyProductMapper.ToArrayResponse,
            cancellationToken);

        return HandleResult(paginatedProducts);
    }

    [HttpGet("{productId}")]
    [ProducesResponseType<PaginatedResult<CompanyProductResponse[]>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetCompanyProductAsync(
        Guid tenantId,
        Guid companyId,
        Guid productId,
        CancellationToken cancellationToken)
    {
        Result<CompanyProductResponse> product = await _productUseCase.GetCompanyProductByIdAsync(
            tenantId,
            companyId,
            productId,
            CompanyProductMapper.ToResponse,
            cancellationToken);

        return HandleResult(product);
    }

    [HttpPost]
    [ProducesResponseType<Result<CompanyProductResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateCompanyProductAsync(
        Guid tenantId,
        Guid companyId,
        CreateCompanyProductRequest request,
        CancellationToken cancellationToken = default)
    {
        ProductCreateModel productCreateModel = CompanyProductMapper.ToCreateModel(request, tenantId, companyId);
        AuditStampCreateModel auditLogCreateModel = AuditLogMapper.ToCreateModel(
            Guid.NewGuid(),
            "asd@asd.dasd",
            tenantId);

        Result<Product> createdProduct = await _productUseCase.CreateCompanyProductAsync(
            productCreateModel,
            auditLogCreateModel,
            cancellationToken);

        return HandleResult(
            result: createdProduct,
            mapper: CompanyProductMapper.ToResponse);
    }

    [HttpPatch("{productId}")]
    [ProducesResponseType<Result<CompanyProductResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateCompanyProductAsync(
        Guid tenantId,
        Guid companyId,
        Guid productId,
        UpdateCompanyProductRequest request,
        CancellationToken cancellationToken = default)
    {
        ProductUpdateModel productUpdateModel = CompanyProductMapper.ToUpdateModel(request, tenantId, companyId, productId);
        AuditStampCreateModel auditStampCreateModel = AuditLogMapper.ToCreateModel(
            Guid.NewGuid(),
            "asd@asd.dasd",
            tenantId);

        Result<Product> updatedProduct = await _productUseCase.UpdateCompanyProductAsync(
            productUpdateModel,
            auditStampCreateModel,
            cancellationToken);

        return HandleResult(result: updatedProduct);
    }

    [HttpDelete("{productId}")]
    [ProducesResponseType<Result<CompanyProductResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteCompanyProductAsync(
        Guid tenantId,
        Guid companyId,
        Guid productId,
        CancellationToken cancellationToken = default)
    {
        AuditStampCreateModel auditStampCreateModel = AuditLogMapper.ToCreateModel(
            Guid.NewGuid(),
            "asd@asd.dasd",
            tenantId);

        Result deletedProduct = await _productUseCase.DeleteCompanyProductAsync(
            tenantId,
            companyId,
            productId,
            auditStampCreateModel,
            cancellationToken);

        return HandleResult(result: deletedProduct);
    }
}
