## Moq Patterns

### Setup Method Returns

```csharp
// Asynchronous return
_repositoryMock.Setup(r => r
    .GetByIdForTenantAsync(tenantId, companyId, It.IsAny<CancellationToken>()))
    .ReturnsAsync(company);

// Return with callback
_repositoryMock.Setup(r => r
    .GetTypeaheadForTenantAsync<string>(
        tenantId, null, It.IsAny<Func<Company, string>>(), It.IsAny<CancellationToken>()))
    .ReturnsAsync((Guid t, string? f, Func<Company, string> mapper, CancellationToken ct) =>
    {
        return companies.Select(mapper).ToArray();
    });
```

### Verify Method Calls

```csharp
// Verify called once
_repositoryMock.Verify(
    r => r.CreateAsync(It.IsAny<Company>(), It.IsAny<CancellationToken>()), 
    Times.Once);

// Verify never called
_repositoryMock.Verify(
    r => r.CreateAsync(It.IsAny<Company>(), It.IsAny<CancellationToken>()), 
    Times.Never);

// Verify with specific arguments
_repositoryMock.Verify(
    r => r.CreateAsync(
        It.Is<Company>(c => c.Name == "Test" && c.TenantId == tenantId), 
        It.IsAny<CancellationToken>()), 
    Times.Once);
```