using Microsoft.Extensions.DependencyInjection;

namespace Template;

partial class ConstructorTemplate
{
    public ConstructorTemplate(IServiceProvider provider)
    {
        provider.GetRequiredService<object>();
    }
}