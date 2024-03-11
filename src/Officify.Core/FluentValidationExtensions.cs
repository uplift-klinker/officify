using FluentValidation;

namespace Officify.Core;

public static class FluentValidationExtensions
{
    public static IRuleBuilder<TModel, TProperty> IsRequired<TModel, TProperty>(
        this IRuleBuilder<TModel, TProperty> rule
    )
    {
        return rule.NotEmpty().NotNull();
    }
}
