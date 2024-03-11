using Officify.Core.Common.Entities;
using Officify.Models.Competitors;

namespace Officify.Core.Competitors.Entities;

public class CompetitorEntity : Entity
{
    public string Codename { get; set; } = "";

    public string UserId { get; set; } = "";

    public CompetitorModel ToModel()
    {
        return new CompetitorModel(Id, Codename, UserId);
    }
}
