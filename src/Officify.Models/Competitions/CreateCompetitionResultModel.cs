namespace Officify.Models.Competitions;

public record CreateCompetitionResultModel(
    Guid CompetitionId,
    Guid CompetitorId,
    CompetitionResultTypeModel ResultType,
    decimal Result
);
