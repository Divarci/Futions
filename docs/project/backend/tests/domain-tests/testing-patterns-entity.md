## Entity Testing Patterns

**CRITICAL** : Followings are just some example patterns, actual entities might have more method to test.

### Failure Patterns

**Note** : Failure test must have validation use cases, too. They will not be here explicitly because they are very depend on the use case. Falire validations are mandatory.

- **Create Failure Example** : Core tests example and mostly all entities expected to have this
```csharp
  [Fact]
  public void Create_WithNullModel_ReturnsInternalError()
  {
      // Arrange
      CreateTenantModel model = null!;

      // Act
      Result<Core.Domain.Identity.Tenant> result = Core.Domain.Identity.Tenant.Create(model);

      // Assert
      result.IsFailed.Should().BeTrue();
      result.Error!.Code.Should().Be(ErrorType.InternalError);
  }
```
- **Update Failure Tests**: Core tests example and mostly all entities expected to have this with their own properties

```csharp
    [Fact]
    public void UpdateName_WhenDeleted_ReturnsConflictError()
    {
        // Arrange
        var company = CreateValidCompany();
        company.Delete();

        // Act
        var result = company.UpdateName("New Name");

        // Assert
        result.IsFailed.Should().BeTrue();
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().NotBeNull();
        result.Error!.Code.Should().Be(ErrorType.Conflict);
    }

    [Fact]
    public void UpdateName_WhenPending_ReturnsConflictError()
    {
        // Arrange
        var tenant = CreateValidTenant();
        const string newName = "New Tenant Name";

        // Act
        Result result = tenant.UpdateName(newName);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Error!.Code.Should().Be(ErrorType.Conflict);
    }

```

- **Delete Failure Tests**: Core tests example and mostly all entities expected to have this
```csharp
    [Fact]
    public void Delete_WhenAlreadyDeleted_ReturnsConflictError()
    {
        // Arrange
        UserEntity user = CreateValidDeletedUser();

        // Act
        Result result = user.Delete();

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Error!.Code.Should().Be(ErrorType.Conflict);
    }
```

- **Entity Specific Method Failure Tests** : Very specific test example might be belong to some entities
```csharp
[Fact]
public void Activate_WhenAlreadyActive_ReturnsConflictError()
{
    // Arrange
    Core.Domain.Identity.Tenant tenant = CreateValidActiveTenant();

    // Act
    Result result = tenant.Activate();

    // Assert
    result.IsFailed.Should().BeTrue();
    result.Error!.Code.Should().Be(ErrorType.Conflict);
}

[Fact]
public void Activate_WhenAlreadyActive_ReturnsConflictError()
{
    // Arrange
    var tenant = CreateValidActiveTenant();
    bool originalPendingState = tenant.IsPending;

    // Act
    Result result = tenant.Activate();

    // Assert
    result.IsFailed.Should().BeTrue();
    result.Error!.Code.Should().Be(ErrorType.Conflict);
}
```
### Success Patterns

**Note** : Success test must **NOT** have validation use cases. Failure test will cover the boundries.

- **Create Success Tests** : Core tests example and mostly all entities expected to have this.
```csharp
 [Fact]
 public void Create_WithValidData_ReturnsSuccessResult()
 {
     // Arrange
     CreateProductModel model = CreateValidModel();
     Guid tenantId = GetValidTenantId();

     // Act
     Result<ProductEntity> result = ProductEntity.Create(model, tenantId);

     // Assert
     result.Should().NotBeNull();
     result.IsSuccess.Should().BeTrue();
     result.Data.Should().NotBeNull();
     result.Data!.Name.Should().Be("Test Product");
     result.Data.Description.Should().Be("Test Description");
     result.Data.CanDiscount.Should().BeTrue();
     result.Data.UnitOfMeasure.Should().Be("Unit");
     result.Data.UnitPrice.Should().Be(100.00m);
     result.Data.TenantId.Should().Be(tenantId);
     result.Data.Id.Should().NotBeEmpty();
     result.Data.IsDeleted.Should().BeFalse();
 }
```

- **Update Test Example** : Core tests example and mostly all entities expected to have this
```csharp
    [Fact]
    public void UpdateName_WithValidData_ReturnsSuccessAndUpdatesName()
    {
        // Arrange
        Core.Domain.Identity.Tenant tenant = CreateValidActiveTenant();
        const string newName = "Updated Tenant Name";

        // Act
        Result result = tenant.UpdateName(newName);

        // Assert
        result.IsSuccess.Should().BeTrue();
        tenant.Name.Should().Be(newName);
    }
```

- **Delete Test Example** : Core tests example and mostly all entities expected to have this
```csharp
    [Fact]
    public void Delete_WithValidProduct_ReturnsSuccessAndSetsIsDeleted()
    {
        // Arrange
        ProductEntity product = CreateValidProduct();

        // Act
        Result result = product.Delete();

        // Assert
        result.IsSuccess.Should().BeTrue();
        product.IsDeleted.Should().BeTrue();
    }

    [Fact]
    public void Delete_DoesNotChangeOtherProperties()
    {
        // Arrange
        ProductEntity product = CreateValidProduct();
        Guid originalId = product.Id;
        string originalName = product.Name;
        decimal originalUnitPrice = product.UnitPrice;

        // Act
        Result result = product.Delete();

        // Assert
        result.IsSuccess.Should().BeTrue();
        product.Id.Should().Be(originalId);
        product.Name.Should().Be(originalName);
        product.UnitPrice.Should().Be(originalUnitPrice);
    }
```


- **Entity Specific Method Success Tests** : Very specific test example might be belong to some entities
```csharp
 [Fact]
 public void UpdateUnitOfMeasure_WithValidData_ReturnsSuccessAndUpdatesUnitOfMeasure()
 {
     // Arrange
     ProductEntity product = CreateValidProduct();
     const string newUom = "Kilogram";

     // Act
     Result result = product.UpdateUnitOfMeasure(newUom);

     // Assert
     result.IsSuccess.Should().BeTrue();
     product.UnitOfMeasure.Should().Be(newUom);
 }

   [Fact]
  public void UpdateUnitOfMeasure_DoesNotChangeOtherProperties()
  {
      // Arrange
      ProductEntity product = CreateValidProduct();
      string originalName = product.Name;
      decimal originalUnitPrice = product.UnitPrice;

      // Act
      Result result = product.UpdateUnitOfMeasure("New UOM");

      // Assert
      result.IsSuccess.Should().BeTrue();
      product.Name.Should().Be(originalName);
      product.UnitPrice.Should().Be(originalUnitPrice);
  }
```
