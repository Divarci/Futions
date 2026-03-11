using Core.Library.Abstractions.Interfaces;
using Core.Library.Attributes;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Infra.Persistence.Context;

public class AppDbContext(
    DbContextOptions<AppDbContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        ProcessAutoseedData(modelBuilder);
    }   

    private static void ProcessAutoseedData(ModelBuilder modelBuilder)
    {
        List<Type> entityTypes = [.. modelBuilder.Model.GetEntityTypes()
            .Select(x => x.ClrType)
            .Where(x => typeof(IHaveAutoSeedData).IsAssignableFrom(x))];

        foreach (var entityType in entityTypes)
        {
            List<FieldInfo> fields = [.. entityType
                .GetFields(BindingFlags.Public | BindingFlags.Static)
                .Where(p => p.GetCustomAttribute<AutoSeedDataAttribute>() != null)];

            if (fields.Count == 0)
                continue;

            List<object> seedData = [];

            foreach (var field in fields)
            {
                object? value = field.GetValue(null);

                if (value != null)
                    seedData.Add(value);
            }

            if (seedData.Count > 0)
                modelBuilder.Entity(entityType).HasData(seedData);
        }
    }
}

