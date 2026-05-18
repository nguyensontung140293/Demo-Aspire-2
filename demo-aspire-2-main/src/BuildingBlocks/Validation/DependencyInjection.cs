using System.Reflection;
using BuildingBlocks.Validation.Behaviors;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks.Validation;

public static class DependencyInjection
{
    /// <summary>
    /// Đăng ký FluentValidation validators và MediatR ValidationBehavior.
    /// Gọi trong từng service và truyền assembly chứa các Validator.
    /// </summary>
    public static IServiceCollection AddValidation(
        this IServiceCollection services,
        Assembly assembly)
    {
        services.AddValidatorsFromAssembly(assembly);

        services.AddTransient(
            typeof(IPipelineBehavior<,>),
            typeof(ValidationBehavior<,>));

        return services;
    }
}
