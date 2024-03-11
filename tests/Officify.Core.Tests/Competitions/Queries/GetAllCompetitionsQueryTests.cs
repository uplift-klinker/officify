using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Officify.Core.Common;
using Officify.Core.Common.Repositories;
using Officify.Core.Competitions.Entities;
using Officify.Core.Competitions.Queries;
using Officify.Core.Tests.Support;

namespace Officify.Core.Tests.Competitions.Queries;

public class GetAllCompetitionsQueryTests
{
    private readonly IRepository<CompetitionEntity, QueryParameters> _repository;
    private readonly IMessageBus _messageBus;

    public GetAllCompetitionsQueryTests()
    {
        var provider = OfficifyCoreServiceProviderFactory.Create();
        _repository = provider.GetRequiredService<
            IRepository<CompetitionEntity, QueryParameters>
        >();
        _messageBus = provider.GetRequiredService<IMessageBus>();
    }

    [Fact]
    public async Task WhenGettingAllCompetitionsThenReturnsAllCompetitions()
    {
        await _repository.SaveAsync(new CompetitionEntity());
        await _repository.SaveAsync(new CompetitionEntity());
        await _repository.SaveAsync(new CompetitionEntity());
        await _repository.SaveAsync(new CompetitionEntity());

        var result = await _messageBus.ExecuteAsync(new GetAllCompetitionsQuery());

        result.Items.Should().HaveCount(4);
        result.TotalCount.Should().Be(4);
    }
}
