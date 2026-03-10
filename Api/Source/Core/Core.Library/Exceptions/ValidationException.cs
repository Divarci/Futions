namespace Core.Library.Exceptions;

/// <summary>
/// Represents an application-level exception that carries a named context,
/// allowing the source or category of the error to be identified alongside the message.
/// </summary>
public class ValidationException(string name, string? message) : Exception(message)
{
    public string Name { get; set; } = name;
}