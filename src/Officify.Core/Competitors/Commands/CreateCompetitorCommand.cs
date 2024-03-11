using FluentValidation;
using Officify.Core.Common;
using Officify.Core.Common.Commands;
using Officify.Core.Competitors.Entities;
using Officify.Core.Competitors.Queries;
using Officify.Core.Competitors.Repositories;
using Officify.Models.Competitors;

namespace Officify.Core.Competitors.Commands;

public record CreateCompetitorCommand(string Codename, string UserId) : ICommand<CompetitorModel>;

public class CreateCompetitorCommandValidator : AbstractValidator<CreateCompetitorCommand>
{
    public CreateCompetitorCommandValidator(IMessageBus messageBus)
    {
        RuleFor(c => c.Codename).IsRequired();
        RuleFor(c => c.UserId).IsRequired();
        RuleFor(c => c.UserId)
            .MustAsync(
                async (userId, cancellationToken) =>
                {
                    var competitor = await messageBus.ExecuteAsync(
                        new GetCompetitorByUserIdQuery(userId),
                        cancellationToken
                    );
                    return competitor == null;
                }
            );
    }
}

internal class CreateCompetitorCommandHandler(ICompetitorRepository repository)
    : ICommandHandler<CreateCompetitorCommand, CompetitorModel>
{
    public async Task<CompetitorModel> Handle(
        CreateCompetitorCommand request,
        CancellationToken cancellationToken
    )
    {
        var entity = await repository
            .SaveAsync(
                new CompetitorEntity { Codename = request.Codename, UserId = request.UserId },
                cancellationToken
            )
            .ConfigureAwait(false);
        return entity.ToModel();
    }
}
