using Microsoft.Extensions.DependencyInjection;

namespace BigBobChat.Common.DependencyInjection;

public class ScopedAttribute : ServiceAttribute
{
    public ScopedAttribute(Type serviceType) : base(serviceType, ServiceLifetime.Scoped)
    {
    }
}
