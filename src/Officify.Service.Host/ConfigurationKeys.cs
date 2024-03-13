namespace Officify.Service.Host;

public static class ConfigurationKeys
{
    public static class AzurePersistence
    {
        private const string SectionName = nameof(AzurePersistence);
        public const string StorageConnectionString =
            $"{SectionName}:{nameof(StorageConnectionString)}";
        public const string TableName = $"{SectionName}:{nameof(TableName)}";
    }
}
