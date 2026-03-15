using Adapter.RestApi.AspNetCore.Extensions;
using Adapter.RestApi.Controllers.Shared.Models;
using Adapter.RestApi.Controllers.VersionOne.Organisations.Products.Models.Requests;
using Adapter.RestApi.Controllers.VersionOne.Organisations.Products.Models.Responses;
using Adapter.RestApi.Controllers.VersionOne.System.AuditLogs;
using Asp.Versioning;
using Core.Domain.Entities.Organisations.Products;
using Core.Domain.Entities.Organisations.Products.Interfaces;
using Core.Domain.Entities.Organisations.Products.Models;
using Core.Domain.ValueObjects.AuditStampValueObject;
using Core.Library.ResultPattern;
using Microsoft.AspNetCore.Mvc;

namespace Adapter.RestApi.Controllers.VersionOne.Organisations.Products;

[ApiVersion(ApiVersion.V1)]
[Route("api/v{version:apiVersion}/tenants/{tenantId:guid}/products")]
[ApiController]
public class ProductController(
    IProductUseCase productUseCase) : BaseController
{
    private readonly IProductUseCase _productUseCase = productUseCase;

    [HttpGet]
    public async Task<IActionResult> GetProductsAsync(
        Guid tenantId,
        [FromQuery] PaginationFilterModel filter,
        CancellationToken cancellationToken)
    {
        PaginatedResult<ProductResponse[]> paginatedProducts = await _productUseCase.GetPaginatedAsync(
            tenantId,
            filter.Page,
            filter.PageSize,
            filter.SortBy,
            filter.IsAscending,
            filter.Filter,
            ProductMapper.ToArrayResponse,
            cancellationToken);

        return HandleResult(paginatedProducts);
    }

    [HttpGet("{productId}")]
    [ProducesResponseType<PaginatedResult<ProductResponse[]>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetProductAsync(
        Guid tenantId,
        Guid productId,
        CancellationToken cancellationToken)
    {
        Result<ProductResponse> product = await _productUseCase.GetByIdAsync(
            tenantId,
            productId,
            ProductMapper.ToResponse,
            cancellationToken);

        return HandleResult(product);
    }

    [HttpPost]
    [ProducesResponseType<Result<ProductResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateProductAsync(
        Guid tenantId,
        CreateProductRequest request,
        CancellationToken cancellationToken = default)
    {
        ProductCreateModel productCreateModel = ProductMapper.ToCreateModel(request, tenantId);
        AuditStampCreateModel auditLogCreateModel = AuditLogMapper.ToCreateModel(
            Guid.NewGuid(),
            "asd@asd.dasd",
            tenantId);

        Result<Product> createdProduct = await _productUseCase.CreateAsync(
            productCreateModel,
            auditLogCreateModel,
            cancellationToken);

        return HandleResult(
            result: createdProduct,
            mapper: ProductMapper.ToResponse);
    }

    [HttpPatch("{productId}")]
    [ProducesResponseType<Result<ProductResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateProductAsync(
        Guid tenantId,
        Guid productId,
        UpdateProductRequest request,
        CancellationToken cancellationToken = default)
    {
        ProductUpdateModel productUpdateModel = ProductMapper.ToUpdateModel(request, productId);
        AuditStampCreateModel auditStampCreateModel = AuditLogMapper.ToCreateModel(
            Guid.NewGuid(),
            "asd@asd.dasd",
            tenantId);

        Result<Product> updatedProduct = await _productUseCase.UpdateAsync(
            tenantId,
            productUpdateModel,
            auditStampCreateModel,
            cancellationToken);

        return HandleResult(
            result: updatedProduct,
            mapper: ProductMapper.ToResponse);
    }

    [HttpDelete("{productId}")]
    [ProducesResponseType<Result<ProductResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteProductAsync(
        Guid tenantId,
        Guid productId,
        CancellationToken cancellationToken = default)
    {
        AuditStampCreateModel auditStampCreateModel = AuditLogMapper.ToCreateModel(
            Guid.NewGuid(),
            "asd@asd.dasd",
            tenantId);

        Result deletedProduct = await _productUseCase.DeleteAsync(
            tenantId,
            productId,
            auditStampCreateModel,
            cancellationToken);

        return HandleResult(result: deletedProduct);
    }
}
