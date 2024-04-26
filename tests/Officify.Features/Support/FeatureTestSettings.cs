namespace Officify.Features.Support;

public record FeatureUser(string Username, string Password);

public class FeatureTestSettings
{
    public static readonly FeatureTestSettings Instance = new Lazy<FeatureTestSettings>(
        new FeatureTestSettings()
    ).Value;

    public string LoginUrl => $"https://{GetEnvironmentVariable("AUTH0_DOMAIN")}/login";

    public FeatureUser DefaultUser =>
        new(
            GetEnvironmentVariable("AUTH_DEFAULT_USER_NAME"),
            GetEnvironmentVariable("AUTH_DEFAULT_USER_PASSWORD")
        );

    private static string GetEnvironmentVariable(string name)
    {
        var value = Environment.GetEnvironmentVariable(name);
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException($"Environment variable '{name}' was empty or not set");

        return value;
    }
}
