## Validator Testing Patterns

**CRITICAL** : Followings are just some example patterns, actual validators might have more methods to test.

### Failure Patterns

- **Required Field Failure Tests** : Core tests example for required validation
```csharp
[Theory]
[InlineData(null)]
[InlineData("")]
[InlineData("   ")]
public void Validate_WhenRequiredAndNullOrEmpty_ReturnsValidationError(string? value)
{
    // Act
    Result result = value.Validate("FieldName", maxLength: 50, isRequired: true);

    // Assert
    result.IsFailed.Should().BeTrue();
    result.Error!.Code.Should().Be(ErrorType.ValidationError);
}
```

- **Max Length Failure Tests** : Core tests example for length validation
```csharp
[Fact]
public void Validate_WhenExceedsMaxLength_ReturnsValidationError()
{
    // Arrange
    string value = new('A', 51);

    // Act
    Result result = value.Validate("FieldName", maxLength: 50, isRequired: true);

    // Assert
    result.IsFailed.Should().BeTrue();
    result.Error!.Code.Should().Be(ErrorType.ValidationError);
}

[Theory]
[InlineData(1)]
[InlineData(10)]
[InlineData(100)]
public void Validate_WhenExceedsVariousMaxLengths_ReturnsValidationError(int maxLength)
{
    // Arrange
    string value = new('A', maxLength + 1);

    // Act
    Result result = value.Validate("FieldName", maxLength, isRequired: true);

    // Assert
    result.IsFailed.Should().BeTrue();
    result.Error!.Code.Should().Be(ErrorType.ValidationError);
}
```

- **Email Format Failure Tests** : Core tests example for email validation
```csharp
[Theory]
[InlineData("invalid")]
[InlineData("invalid@")]
[InlineData("@invalid.com")]
[InlineData("invalid@.com")]
[InlineData("invalid@domain")]
public void Validate_WhenInvalidEmailFormat_ReturnsValidationError(string email)
{
    // Act
    Result result = email.Validate("Email", maxLength: 100, isRequired: true, isEmail: true);

    // Assert
    result.IsFailed.Should().BeTrue();
    result.Error!.Code.Should().Be(ErrorType.ValidationError);
}
```

- **Combined Validation Failure Tests** : Core tests for multiple validation errors
```csharp
[Fact]
public void CombineAndValidate_WithMultipleFailures_ReturnsAllErrors()
{
    // Arrange
    Result[] results =
    [
        string.Empty.Validate("Field1", maxLength: 50, isRequired: true),
        string.Empty.Validate("Field2", maxLength: 50, isRequired: true)
    ];

    // Act
    Result combinedResult = results.CombineAndValidate();

    // Assert
    combinedResult.IsFailed.Should().BeTrue();
    combinedResult.Error!.Code.Should().Be(ErrorType.ValidationError);
    combinedResult.Error!.Details.Should().HaveCount(2);
}
```

### Success Patterns

- **Required Field Success Tests** : Core tests for valid required values
```csharp
[Fact]
public void Validate_WhenRequiredAndHasValue_ReturnsSuccess()
{
    // Arrange
    string value = "ValidValue";

    // Act
    Result result = value.Validate("FieldName", maxLength: 50, isRequired: true);

    // Assert
    result.IsSuccess.Should().BeTrue();
}

[Fact]
public void Validate_WhenNotRequiredAndEmpty_ReturnsSuccess()
{
    // Arrange
    string? value = null;

    // Act
    Result result = value.Validate("FieldName", maxLength: 50, isRequired: false);

    // Assert
    result.IsSuccess.Should().BeTrue();
}
```

- **Max Length Success Tests** : Core tests for valid length values
```csharp
[Fact]
public void Validate_WhenExactlyMaxLength_ReturnsSuccess()
{
    // Arrange
    string value = new('A', 50);

    // Act
    Result result = value.Validate("FieldName", maxLength: 50, isRequired: true);

    // Assert
    result.IsSuccess.Should().BeTrue();
}

[Fact]
public void Validate_WhenUnderMaxLength_ReturnsSuccess()
{
    // Arrange
    string value = new('A', 49);

    // Act
    Result result = value.Validate("FieldName", maxLength: 50, isRequired: true);

    // Assert
    result.IsSuccess.Should().BeTrue();
}
```

- **Email Format Success Tests** : Core tests for valid email formats
```csharp
[Theory]
[InlineData("test@example.com")]
[InlineData("user.name@domain.org")]
[InlineData("user+tag@example.co.uk")]
public void Validate_WhenValidEmailFormat_ReturnsSuccess(string email)
{
    // Act
    Result result = email.Validate("Email", maxLength: 100, isRequired: true, isEmail: true);

    // Assert
    result.IsSuccess.Should().BeTrue();
}
```

- **Combined Validation Success Tests** : Core tests for all validations passing
```csharp
[Fact]
public void CombineAndValidate_WhenAllPass_ReturnsSuccess()
{
    // Arrange
    Result[] results =
    [
        "Value1".Validate("Field1", maxLength: 50, isRequired: true),
        "Value2".Validate("Field2", maxLength: 50, isRequired: true)
    ];

    // Act
    Result combinedResult = results.CombineAndValidate();

    // Assert
    combinedResult.IsSuccess.Should().BeTrue();
}

[Fact]
public void CombineAndValidate_WhenSingleValidResult_ReturnsSuccess()
{
    // Arrange
    Result[] results =
    [
        "ValidValue".Validate("FieldName", maxLength: 50, isRequired: true)
    ];

    // Act
    Result combinedResult = results.CombineAndValidate();

    // Assert
    combinedResult.IsSuccess.Should().BeTrue();
}
```
