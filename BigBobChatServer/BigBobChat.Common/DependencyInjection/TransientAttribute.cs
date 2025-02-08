using Microsoft.Extensions.DependencyInjection;

namespace BigBobChat.Common.DependencyInjection;

public class TransientAttribute : ServiceAttribute
{
    public TransientAttribute(Type serviceType) : base(serviceType, ServiceLifetime.Transient)
    {
    }
}
