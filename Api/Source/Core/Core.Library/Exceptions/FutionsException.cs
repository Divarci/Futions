namespace Core.Library.Exceptions;

/// <summary>
/// Represents an application-level exception indicating a developer error.
/// All instances result in HTTP 500 Internal Server Error.
/// </summary>
public sealed class FutionsException(
    string assemblyName,
    string className,
    string methodName,
    string message,
    Exception? innerException = null) : Exception(message, innerException)
{
    public string AssemblyName { get; } = assemblyName;
    public string ClassName { get; } = className;
    public string MethodName { get; } = methodName;
}
