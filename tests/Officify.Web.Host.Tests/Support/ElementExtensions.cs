using AngleSharp.Diffing.Extensions;
using AngleSharp.Dom;

namespace Officify.Web.Host.Tests.Support;

public static class ElementExtensions
{
    public static bool HasLabelOrText(this IElement element, string label)
    {
        return element.TryGetAttrValue("aria-label", out string value)
            ? value.Contains(label)
            : element.GetInnerText().Contains(label);
    }
}
