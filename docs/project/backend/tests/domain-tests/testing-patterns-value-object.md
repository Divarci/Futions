## Value Object Testing Patterns

**CRITICAL** : Followings are just some example patterns, actual value objects might have more methods to test.

### Failure Patterns

**Note** : Failure test must have validation use cases, too. They will not be here explicitly because they are very depend on the use case. Falire validations are mandatory.

- **Create Failure Example** : Core tests example and mostly all value objects expected to have this
```csharp
[Fact]
public void Create_WithNullModel_ReturnsInternalError()
{
    // Arrange
    FullnameModel model = null!;

    // Act
    Result<Fullname> result = Fullname.Create(model);

    // Assert
    result.IsFailed.Should().BeTrue();
    result.Error!.Code.Should().Be(ErrorType.InternalError);
}
```

- **Update Failure Tests** : Core tests example for update validation errors and mostly all value objects expected to have this
```csharp
[Fact]
public void Update_WithNullModel_ReturnsInternalError()
{
    // Arrange
    FullnameModel model = null!;

    // Act
    Result<Fullname> result = Fullname.Update(model);

    // Assert
    result.IsFailed.Should().BeTrue();
    result.Error!.Code.Should().Be(ErrorType.InternalError);
}
```

### Success Patterns

**Note** : Success test must **NOT** have validation use cases. Failure test will cover the boundries.

- **Create Success Tests** : Core tests example and mostly all value objects expected to have this
```csharp
[Fact]
public void Create_WithValidData_ReturnsSuccessResult()
{
    // Arrange
    FullnameModel model = new()
    {
        Firstname = "John",
        Lastname = "Doe"
    };

    // Act
    Result<Fullname> result = Fullname.Create(model);

    // Assert
    result.Should().NotBeNull();
    result.IsSuccess.Should().BeTrue();
    result.Data.Should().NotBeNull();
    result.Data!.Firstname.Should().Be("John");
    result.Data.Lastname.Should().Be("Doe");
}

```

- **Update Success Tests** : Core tests example for update operations and mostly all value objects expected to have this
```csharp
[Fact]
public void Update_WithValidData_ReturnsSuccessAndUpdatesProperties()
{
    // Arrange
    Fullname fullname = CreateValidFullname();
    FullnameModel model = new()
    {
        Firstname = "Jane",
        Lastname = "Smith"
    };

    // Act
    Result result = fullname.Update(model);

    // Assert
    result.IsSuccess.Should().BeTrue();
    fullname.Firstname.Should().Be("Jane");
    fullname.Lastname.Should().Be("Smith");
}

[Fact]
public void Update_WithSameValues_ReturnsSuccess()
{
    // Arrange
    Fullname fullname = CreateValidFullname();
    string originalFirstname = fullname.Firstname;
    string originalLastname = fullname.Lastname;
    FullnameModel model = new()
    {
        Firstname = originalFirstname,
        Lastname = originalLastname
    };

    // Act
    Result result = fullname.Update(model);

    // Assert
    result.IsSuccess.Should().BeTrue();
    fullname.Firstname.Should().Be(originalFirstname);
    fullname.Lastname.Should().Be(originalLastname);
}
```