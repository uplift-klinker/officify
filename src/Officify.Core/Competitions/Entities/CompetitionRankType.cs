using Officify.Models.Competitions;

namespace Officify.Core.Competitions.Entities;

public enum CompetitionRankType
{
    LowestScore,
    HighestScore
}

public static class CompetitionRankTypeExtensions
{
    public static CompetitionRankTypeModel ToModel(this CompetitionRankType type)
    {
        return type switch
        {
            CompetitionRankType.LowestScore => CompetitionRankTypeModel.LowestScore,
            CompetitionRankType.HighestScore => CompetitionRankTypeModel.HighestScore,
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }

    public static CompetitionRankType FromModel(this CompetitionRankTypeModel model)
    {
        return model switch
        {
            CompetitionRankTypeModel.LowestScore => CompetitionRankType.LowestScore,
            CompetitionRankTypeModel.HighestScore => CompetitionRankType.HighestScore,
            _ => throw new ArgumentOutOfRangeException(nameof(model), model, null)
        };
    }
}
