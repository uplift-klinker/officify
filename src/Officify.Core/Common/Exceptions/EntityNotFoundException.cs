namespace Officify.Core.Common.Exceptions;

public class EntityNotFoundException<TEntity>(Guid id) : Exception(FormatMessage(id))
{
    private static string FormatMessage(Guid id)
    {
        return $"Could not find {typeof(TEntity).Name} with id {id}";
    }
}
