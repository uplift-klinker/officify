using Microsoft.Extensions.DependencyInjection;
using Officify.Core.Common;
using Officify.Core.Competitors.Entities;
using Officify.Core.Competitors.Queries;
using Officify.Core.Competitors.Repositories;
using Officify.Core.Tests.Support;

namespace Officify.Core.Tests.Competitors.Queries;

public class GetCompetitorByUserIdQueryTests
{
    private readonly ICompetitorRepository _repository;
    private readonly IMessageBus _messageBus;

    public GetCompetitorByUserIdQueryTests()
    {
        var provider = OfficifyCoreServiceProviderFactory.Create();
        _repository = provider.GetRequiredService<ICompetitorRepository>();
        _messageBus = provider.GetRequiredService<IMessageBus>();
    }

    [Fact]
    public async Task WhenGettingCompetitorByUserIdThenReturnsCompetitorWithUserId()
    {
        var competitor = await _repository.SaveAsync(new CompetitorEntity { UserId = "bill" });

        var model = await _messageBus.ExecuteAsync(new GetCompetitorByUserIdQuery("bill"));

        model?.Id.Should().Be(competitor.Id);
    }

    [Fact]
    public async Task WhenGettingCompetitorByMissingUserIdThenReturnsNull()
    {
        var model = await _messageBus.ExecuteAsync(
            new GetCompetitorByUserIdQuery($"{Guid.NewGuid()}")
        );

        model.Should().BeNull();
    }
}
