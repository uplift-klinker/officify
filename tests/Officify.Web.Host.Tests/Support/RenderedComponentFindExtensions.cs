using AngleSharp.Dom;
using Bunit;
using Microsoft.AspNetCore.Components;

namespace Officify.Web.Host.Tests.Support;

public static class RenderedComponentFindExtensions
{
    public static IRefreshableElementCollection<IElement> FindAllByClass<T>(
        this IRenderedComponent<T> component,
        string className
    )
        where T : IComponent
    {
        return component.FindAll($".{className}");
    }

    public static IElement FindByClass<T>(this IRenderedComponent<T> component, string className)
        where T : IComponent
    {
        return component.Find($".{className}");
    }

    public static IRefreshableElementCollection<IElement> FindAllByRole<T>(
        this IRenderedComponent<T> component,
        string role
    )
        where T : IComponent
    {
        return component.FindAll($"[role=\"{role}\"]");
    }

    public static IEnumerable<IElement> FindAllByRole<T>(
        this IRenderedComponent<T> component,
        string role,
        string label
    )
        where T : IComponent
    {
        return component.FindAllByRole(role).Where(e => e.HasLabelOrText(label));
    }

    public static IElement FindByRole<T>(this IRenderedComponent<T> component, string role)
        where T : IComponent
    {
        return component.Find($"[role=\"{role}\"]");
    }

    public static IElement FindByRole<T>(
        this IRenderedComponent<T> component,
        string role,
        string label
    )
        where T : IComponent
    {
        return component.FindAllByRole(role).Single(e => e.HasLabelOrText(label));
    }
}
