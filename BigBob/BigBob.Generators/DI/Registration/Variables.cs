namespace BigBob.Generators.DI.Registration;

internal static class Variables
{
    public static TypedName Services { get; } = new TypedName("IServiceCollection", "services");

    public static TypedName Provider { get; } = new TypedName("IServiceProvider", "provider");
}
