using System.Text.Json.Serialization;

namespace Officify.Models.Competitions;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum CompetitionResultTypeModel
{
    Number,
    Duration
}

public record CompetitionResultModel(
    Guid Id,
    Guid CompetitionId,
    Guid CompetitorId,
    CompetitionResultTypeModel ResultType,
    decimal Result
);
