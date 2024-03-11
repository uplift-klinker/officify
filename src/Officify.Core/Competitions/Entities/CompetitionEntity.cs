using Officify.Core.Common.Entities;
using Officify.Models.Competitions;

namespace Officify.Core.Competitions.Entities;

public class CompetitionEntity : Entity
{
    public string Name { get; set; } = "";
    public CompetitionRankType RankType { get; set; } = CompetitionRankType.HighestScore;

    public CompetitionModel ToModel()
    {
        return new CompetitionModel(Id, Name, RankType.ToModel());
    }
}
