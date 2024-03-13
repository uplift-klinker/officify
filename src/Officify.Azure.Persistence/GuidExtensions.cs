namespace Officify.Azure.Persistence;

public static class GuidExtensions
{
    public static string ToRowKey(this Guid guid)
    {
        return guid.ToString();
    }
}
