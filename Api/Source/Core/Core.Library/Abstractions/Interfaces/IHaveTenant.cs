namespace Core.Library.Abstractions.Interfaces;

public interface IHaveTenant
{
    public Guid TenantId { get; }
}
