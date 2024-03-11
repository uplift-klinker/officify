namespace Officify.Core.Common.Exceptions;

public class EntityNotFoundException(Type type, Guid id) : Exception(FormatMessage(type, id))
{
    private static string FormatMessage(Type type, Guid id)
    {
        return $"Could not find {type.Name} with id {id}";
    }
}

public class EntityNotFoundException<TEntity>(Guid id)
    : EntityNotFoundException(typeof(TEntity), id);
