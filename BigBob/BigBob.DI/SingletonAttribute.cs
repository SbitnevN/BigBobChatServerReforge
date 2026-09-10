namespace BigBob.DI;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public sealed class SingletonAttribute<T> : Attribute
{
    public Type Interface => typeof(T);
}

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public sealed class SingletonAttribute(Type serviceType) : Attribute
{
    public Type Interface { get; } = serviceType ?? throw new ArgumentNullException(nameof(serviceType));
}