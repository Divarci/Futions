namespace App.UseCases.Helpers;

internal static class CacheKeyHelper
{
    internal static string Collection(string entityName, string methodName, object[] parameters)
        => $"collection_{entityName}:{methodName}_{string.Join("_", parameters)}".ToLower();

    internal static string Single(string entityName, params (string Label, object Value)[] segments)
        => $"{entityName}:{string.Join(":", segments.Select(s => $"{s.Label}({s.Value})"))}".ToLower();
}