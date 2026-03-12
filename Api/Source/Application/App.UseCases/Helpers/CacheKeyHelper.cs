namespace App.UseCases.Helpers;

internal static class CacheKeyHelper
{
    internal static string Collection(string entityName, string methodName, object[] parameters)
        => $"{entityName}:collection:{methodName}_{string.Join("_", parameters)}".ToLower();   
}