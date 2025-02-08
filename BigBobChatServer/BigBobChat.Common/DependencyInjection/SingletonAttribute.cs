using Microsoft.Extensions.DependencyInjection;

namespace BigBobChat.Common.DependencyInjection;

public class SingletonAttribute : Attribute
{
    public SingletonAttribute(Type serviceType) : base(serviceType, ServiceLifetime.Singleton)
    {
    }
}
