using FluentValidation;
using Officify.Core.Common.Commands;
using Officify.Core.Common.Repositories;
using Officify.Core.Competitions.Entities;
using Officify.Core.Competitions.Repositories;
using Officify.Core.Competitors.Repositories;
using Officify.Models.Competitions;

namespace Officify.Core.Competitions.Commands;

public record CreateCompetitionResultCommand(
    Guid CompetitionId,
    Guid CompetitorId,
    CompetitionResultTypeModel ResultType,
    decimal Result
) : ICommand<CompetitionResultModel>;

public class CreateCompetitionResultCommandValidator
    : AbstractValidator<CreateCompetitionResultCommand>
{
    public CreateCompetitionResultCommandValidator(
        IRepository<CompetitionEntity, QueryParameters> competitionRepository,
        ICompetitorRepository competitorRepository
    )
    {
        RuleFor(e => e.CompetitionId).MustAsync(competitionRepository.ExistsByIdAsync);

        RuleFor(e => e.CompetitorId).MustAsync(competitorRepository.ExistsByIdAsync);
        RuleFor(e => e.ResultType).IsInEnum();
    }
}

internal class CreateCompetitionResultCommandHandler(ICompetitionResultRepository repository)
    : ICommandHandler<CreateCompetitionResultCommand, CompetitionResultModel>
{
    public async Task<CompetitionResultModel> Handle(
        CreateCompetitionResultCommand request,
        CancellationToken cancellationToken
    )
    {
        var result = new CompetitionResultEntity
        {
            CompetitionId = request.CompetitionId,
            CompetitorId = request.CompetitorId,
            Result = request.Result,
            ResultType = request.ResultType.FromModel()
        };
        await repository.SaveAsync(result, cancellationToken).ConfigureAwait(false);
        return result.ToModel();
    }
}
