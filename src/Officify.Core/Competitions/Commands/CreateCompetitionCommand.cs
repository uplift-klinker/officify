using FluentValidation;
using Officify.Core.Common.Commands;
using Officify.Core.Common.Repositories;
using Officify.Core.Competitions.Entities;
using Officify.Models.Competitions;

namespace Officify.Core.Competitions.Commands;

public record CreateCompetitionCommand(
    string Name,
    CompetitionRankTypeModel RankType = CompetitionRankTypeModel.HighestScore
) : ICommand<CompetitionModel>;

public class CreateCompetitionCommandValidator : AbstractValidator<CreateCompetitionCommand>
{
    public CreateCompetitionCommandValidator()
    {
        RuleFor(c => c.Name).IsRequired();
        RuleFor(c => c.RankType).IsInEnum();
    }
}

internal class CreateCompetitionCommandHandler(
    IRepository<CompetitionEntity, QueryParameters> repository
) : ICommandHandler<CreateCompetitionCommand, CompetitionModel>
{
    public async Task<CompetitionModel> Handle(
        CreateCompetitionCommand request,
        CancellationToken cancellationToken
    )
    {
        var competition = new CompetitionEntity
        {
            Name = request.Name,
            RankType = request.RankType.FromModel()
        };
        await repository.SaveAsync(competition, cancellationToken).ConfigureAwait(false);
        return competition.ToModel();
    }
}
