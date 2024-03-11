namespace Officify.Models.Competitions;

public record GetLeaderboardModel(Guid CompetitionId, int PageSize, int PageNumber)
    : GetPagedResultsModel(PageSize, PageNumber);
