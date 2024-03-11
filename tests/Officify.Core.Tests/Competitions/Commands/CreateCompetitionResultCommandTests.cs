using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Officify.Core.Common;
using Officify.Core.Common.Repositories;
using Officify.Core.Competitions.Commands;
using Officify.Core.Competitions.Entities;
using Officify.Core.Competitions.Repositories;
using Officify.Core.Competitors.Entities;
using Officify.Core.Competitors.Repositories;
using Officify.Core.Tests.Support;
using Officify.Models.Competitions;

namespace Officify.Core.Tests.Competitions.Commands;

public class CreateCompetitionResultCommandTests : IAsyncLifetime
{
    private readonly CompetitionEntity _competition;
    private readonly CompetitorEntity _competitor;
    private readonly ICompetitorRepository _competitorRepository;
    private readonly IRepository<CompetitionEntity, QueryParameters> _competitionRepository;
    private readonly ICompetitionResultRepository _resultRepository;
    private readonly IMessageBus _messageBus;

    public CreateCompetitionResultCommandTests()
    {
        _competition = new CompetitionEntity();
        _competitor = new CompetitorEntity();

        var provider = OfficifyCoreServiceProviderFactory.Create();
        _competitorRepository = provider.GetRequiredService<ICompetitorRepository>();
        _competitionRepository = provider.GetRequiredService<
            IRepository<CompetitionEntity, QueryParameters>
        >();
        _resultRepository = provider.GetRequiredService<ICompetitionResultRepository>();
        _messageBus = provider.GetRequiredService<IMessageBus>();
    }

    public async Task InitializeAsync()
    {
        await _competitionRepository.SaveAsync(_competition);
        await _competitorRepository.SaveAsync(_competitor);
    }

    [Fact]
    public async Task WhenResultIsAddedToMissingCompetitionThenThrowsError()
    {
        var command = CreateCommand() with { CompetitionId = Guid.NewGuid() };
        var act = () => _messageBus.ExecuteAsync(command);

        await act.Should().ThrowAsync<ValidationException>();
    }

    [Fact]
    public async Task WhenResultIsAddedToMissingCompetitorThenThrowsError()
    {
        var command = CreateCommand() with { CompetitorId = Guid.NewGuid() };
        var act = () => _messageBus.ExecuteAsync(command);

        await act.Should().ThrowAsync<ValidationException>();
    }

    [Fact]
    public async Task WhenResultIsAddedWithInvalidResultType()
    {
        var command = CreateCommand() with { ResultType = (CompetitionResultTypeModel)99 };

        var act = () => _messageBus.ExecuteAsync(command);

        await act.Should().ThrowAsync<ValidationException>();
    }

    [Fact]
    public async Task WhenResultIsAddedThenSavesResult()
    {
        var command = CreateCommand();

        await _messageBus.ExecuteAsync(command);

        var results = await _resultRepository.GetAllAsync();
        results.Items.Should().HaveCount(1);
        results.TotalCount.Should().Be(1);
    }

    [Fact]
    public async Task WhenResultIsAddedThenReturnsResultModel()
    {
        var command = CreateCommand(resultType: CompetitionResultTypeModel.Number, result: 700);

        var model = await _messageBus.ExecuteAsync(command);

        var results = await _resultRepository.GetAllAsync();
        model.Id.Should().Be(results.Items[0].Id);
        model.CompetitionId.Should().Be(_competition.Id);
        model.CompetitorId.Should().Be(_competitor.Id);
        model.Result.Should().Be(700);
        model.ResultType.Should().Be(CompetitionResultTypeModel.Number);
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }

    private CreateCompetitionResultCommand CreateCommand(
        CompetitionResultTypeModel resultType = CompetitionResultTypeModel.Duration,
        decimal result = 0
    )
    {
        return new CreateCompetitionResultCommand(
            _competition.Id,
            _competitor.Id,
            resultType,
            result
        );
    }
}
