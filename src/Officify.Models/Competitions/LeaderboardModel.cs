namespace Officify.Models.Competitions;

public record LeaderboardItemModel(
    int Rank,
    Guid CompetitorId,
    Guid ResultId,
    string CompetitorCodename,
    CompetitionResultTypeModel ResultType,
    decimal Result
);

public record LeaderboardModel(
    Guid CompetitionId,
    string CompetitionName,
    LeaderboardItemModel[] Items,
    int PageSize,
    int PageNumber,
    int TotalCount
) : PagedListResult<LeaderboardItemModel>(Items, PageSize, PageNumber, TotalCount);
