namespace Officify.Models.Settings;

public record TelemetrySettings(string ConnectionString);

public record SettingsModel(TelemetrySettings Telemetry);
