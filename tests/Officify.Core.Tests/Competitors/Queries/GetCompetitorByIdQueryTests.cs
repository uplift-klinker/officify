using Microsoft.Extensions.DependencyInjection;
using Officify.Core.Common;
using Officify.Core.Competitors.Entities;
using Officify.Core.Competitors.Queries;
using Officify.Core.Competitors.Repositories;
using Officify.Core.Tests.Support;

namespace Officify.Core.Tests.Competitors.Queries;

public class GetCompetitorByIdQueryTests
{
    private readonly ICompetitorRepository _repository;
    private readonly IMessageBus _messageBus;

    public GetCompetitorByIdQueryTests()
    {
        var provider = OfficifyCoreServiceProviderFactory.Create();
        _repository = provider.GetRequiredService<ICompetitorRepository>();
        _messageBus = provider.GetRequiredService<IMessageBus>();
    }

    [Fact]
    public async Task WhenGettingCompetitorByIdThenReturnsCompetitorWithId()
    {
        var competitor = await _repository.SaveAsync(new CompetitorEntity());

        var model = await _messageBus.ExecuteAsync(new GetCompetitorByIdQuery(competitor.Id));

        model?.Id.Should().Be(competitor.Id);
    }

    [Fact]
    public async Task WhenGettingMissingCompetitorByIdThenReturnsNull()
    {
        var model = await _messageBus.ExecuteAsync(new GetCompetitorByIdQuery(Guid.NewGuid()));

        model.Should().BeNull();
    }
}
