using Officify.Models.Competitions;

namespace Officify.Core.Competitions.Entities;

public enum CompetitionResultType
{
    Number,
    Duration
}

public static class CompetitionResultTypeExtensions
{
    public static CompetitionResultType FromModel(this CompetitionResultTypeModel model)
    {
        return model switch
        {
            CompetitionResultTypeModel.Number => CompetitionResultType.Number,
            CompetitionResultTypeModel.Duration => CompetitionResultType.Duration,
            _ => throw new ArgumentOutOfRangeException(nameof(model), model, null)
        };
    }

    public static CompetitionResultTypeModel ToModel(this CompetitionResultType type)
    {
        return type switch
        {
            CompetitionResultType.Number => CompetitionResultTypeModel.Number,
            CompetitionResultType.Duration => CompetitionResultTypeModel.Duration,
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }
}
