using Microsoft.Extensions.DependencyInjection;

namespace BigBobChat.Common.DependencyInjection;

public class ServiceAttribute : Attribute
{
    public Type ServiceType { get; set; }
    public ServiceLifetime LifeTime { get; set; }

    public ServiceAttribute(Type serviceType, ServiceLifetime lifetime)
    {
        ServiceType = serviceType;
        LifeTime = lifetime;
    }
}
