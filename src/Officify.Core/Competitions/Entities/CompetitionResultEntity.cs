using Officify.Core.Common.Entities;
using Officify.Models.Competitions;

namespace Officify.Core.Competitions.Entities;

public class CompetitionResultEntity : Entity
{
    public Guid CompetitorId { get; set; }
    public Guid CompetitionId { get; set; }
    public decimal Result { get; set; } = 0;
    public CompetitionResultType ResultType { get; set; } = CompetitionResultType.Number;

    public CompetitionResultModel ToModel()
    {
        return new CompetitionResultModel(
            Id,
            CompetitionId,
            CompetitorId,
            ResultType.ToModel(),
            Result
        );
    }
}
