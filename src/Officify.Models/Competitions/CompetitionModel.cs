using System.Text.Json.Serialization;

namespace Officify.Models.Competitions;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum CompetitionRankTypeModel
{
    LowestScore,
    HighestScore
}

public record CompetitionModel(Guid Id, string Name, CompetitionRankTypeModel RankType);
