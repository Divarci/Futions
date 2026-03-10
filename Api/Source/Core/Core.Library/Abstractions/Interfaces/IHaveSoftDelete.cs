using Core.Library.ResultPattern;

namespace Core.Library.Abstractions.Interfaces;

public interface IHaveSoftDelete
{
    bool IsDeleted { get; }

    Result SoftDelete();
}