using System.Text.RegularExpressions;

namespace Officify.Features.Support;

public static class PageExtensions
{
    public static ILocator GetByRoleRegex(this IPage page, AriaRole role, string name)
    {
        return page.GetByRole(
            role,
            new PageGetByRoleOptions { NameRegex = new(name, RegexOptions.IgnoreCase) }
        );
    }
}
