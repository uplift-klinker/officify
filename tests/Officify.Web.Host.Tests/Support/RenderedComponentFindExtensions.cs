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
}
