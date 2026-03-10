namespace Core.Library.Attributes;

/// <summary>
/// Marks an enum field as a source of automatic seed data.
/// Applied at the field level to indicate that the associated data
/// should be seeded into the database automatically.
/// </summary>
[AttributeUsage(AttributeTargets.Field)]
public class AutoSeedDataAttribute : Attribute { }