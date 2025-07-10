using Microsoft.Extensions.DependencyInjection;

namespace BigBobChat.Common.DependencyInjection;

public class SingletonAttribute : ServiceAttribute
{
    public SingletonAttribute(Type serviceType) : base(serviceType, ServiceLifetime.Singleton)
    {
    }
}
