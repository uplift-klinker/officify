using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Officify.Core.Common;
using Officify.Core.Common.Repositories;
using Officify.Core.Competitions.Commands;
using Officify.Core.Competitions.Entities;
using Officify.Core.Tests.Support;
using Officify.Models.Competitions;

namespace Officify.Core.Tests.Competitions.Commands;

public class CreateCompetitionCommandTests
{
    private readonly IMessageBus _messageBus;
    private readonly IRepository<CompetitionEntity, QueryParameters> _repository;

    public CreateCompetitionCommandTests()
    {
        var provider = OfficifyCoreServiceProviderFactory.Create();

        _repository = provider.GetRequiredService<
            IRepository<CompetitionEntity, QueryParameters>
        >();
        _messageBus = provider.GetRequiredService<IMessageBus>();
    }

    [Fact]
    public async Task WhenCompetitionCreatedThenAddsCompetitionToDatabase()
    {
        await _messageBus.ExecuteAsync(new CreateCompetitionCommand("Gilded Rose"));

        var result = await _repository.GetAllAsync();
        result.Items.Should().HaveCount(1);
        result.TotalCount.Should().Be(1);
    }

    [Fact]
    public async Task WhenCompetitionCreatedThenReturnsCompetitionModel()
    {
        var model = await _messageBus.ExecuteAsync(
            new CreateCompetitionCommand("Vending Machine", CompetitionRankTypeModel.LowestScore)
        );

        var all = await _repository.GetAllAsync();
        all.Items.Should().Satisfy(i => i.Id == model.Id);
        model.Name.Should().Be("Vending Machine");
        model.RankType.Should().Be(CompetitionRankTypeModel.LowestScore);
    }

    [Fact]
    public async Task WhenCommandIsMissingNameThenThrowsError()
    {
        var command = new CreateCompetitionCommand("");
        var act = () => _messageBus.ExecuteAsync(command);

        await act.Should().ThrowAsync<ValidationException>();
    }

    [Fact]
    public async Task WhenCommandHasInvalidRankTypeThenThrowsError()
    {
        var command = new CreateCompetitionCommand("bob", (CompetitionRankTypeModel)99);
        var act = () => _messageBus.ExecuteAsync(command);

        await act.Should().ThrowAsync<ValidationException>();
    }
}
