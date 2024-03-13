using System.Reflection;
using System.Text.Json;
using Azure.Data.Tables;

namespace Officify.Azure.Persistence;

public static class TableEntityExtensions
{
    public static T? ToEntity<T>(this TableEntity tableEntity)
    {
        var entityJson = JsonSerializer.Serialize(tableEntity);
        return JsonSerializer.Deserialize<T>(entityJson);
    }

    public static void PopulateFrom<T>(this TableEntity tableEntity, T value)
    {
        var properties = typeof(T).GetProperties(BindingFlags.GetProperty);
        foreach (var property in properties)
        {
            tableEntity.TryAdd(property.Name, property.GetValue(value));
        }
    }
}
