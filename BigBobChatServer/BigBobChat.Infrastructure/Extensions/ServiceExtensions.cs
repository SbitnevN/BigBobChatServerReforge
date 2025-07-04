using BigBobChat.Common.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace BigBobChat.Infrastructure.Extensions;

public static class ServiceExtensions
{
    public static void AddMarkedService(this IServiceCollection services)
    {
        Assembly assembly = Assembly.GetExecutingAssembly();

        foreach (Type type in assembly.GetTypes())
        {
            ServiceAttribute? attribute = type.GetCustomAttribute<ServiceAttribute>();
            if (attribute is null)
                continue;

            services.Add(new ServiceDescriptor(attribute.ServiceType, type, attribute.LifeTime));
        }
    }
}
