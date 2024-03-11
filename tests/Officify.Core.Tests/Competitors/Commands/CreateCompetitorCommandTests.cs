using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Officify.Core.Common;
using Officify.Core.Competitors.Commands;
using Officify.Core.Competitors.Entities;
using Officify.Core.Competitors.Repositories;
using Officify.Core.Tests.Support;

namespace Officify.Core.Tests.Competitors.Commands;

public class CreateCompetitorCommandTests
{
    private readonly ICompetitorRepository _repository;
    private readonly IMessageBus _messageBus;

    public CreateCompetitorCommandTests()
    {
        var provider = OfficifyCoreServiceProviderFactory.Create();
        _repository = provider.GetRequiredService<ICompetitorRepository>();
        _messageBus = provider.GetRequiredService<IMessageBus>();
    }

    [Fact]
    public async Task WhenCompetitorIsCreatedThenSavesCompetitor()
    {
        var command = new CreateCompetitorCommand("Ice Man", "auth0|userid");
        await _messageBus.ExecuteAsync(command);

        var competitors = await _repository.GetAllAsync();
        competitors.Items.Should().HaveCount(1);
        competitors.Items[0].Codename.Should().Be("Ice Man");
        competitors.Items[0].UserId.Should().Be("auth0|userid");
    }

    [Fact]
    public async Task WhenCompetitorIsCreatedThenReturnsPopulatedModel()
    {
        var command = new CreateCompetitorCommand("Goose", "auth0|goose");
        var model = await _messageBus.ExecuteAsync(command);

        var competitors = await _repository.GetAllAsync();
        model.Id.Should().Be(competitors.Items[0].Id);
        model.Codename.Should().Be("Goose");
        model.UserId.Should().Be("auth0|goose");
    }

    [Fact]
    public async Task WhenCompetitorIsCreatedWithoutCodenameThenThrowsError()
    {
        var command = new CreateCompetitorCommand("", "auth0|goose");
        var act = () => _messageBus.ExecuteAsync(command);

        await act.Should().ThrowAsync<ValidationException>();
    }

    [Fact]
    public async Task WhenCompetitorIsCreatedWithoutUserIdThenThrowsError()
    {
        var command = new CreateCompetitorCommand("Maverick", "");
        var act = () => _messageBus.ExecuteAsync(command);

        await act.Should().ThrowAsync<ValidationException>();
    }

    [Fact]
    public async Task WhenCompetitorWithUserIdAlreadyExistsThenThrowsError()
    {
        await _repository.SaveAsync(new CompetitorEntity { UserId = "goose" });

        var command = new CreateCompetitorCommand("Goose", "goose");
        var act = () => _messageBus.ExecuteAsync(command);

        await act.Should().ThrowAsync<ValidationException>();
    }
}
