namespace BigBob.DI;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public sealed class TransientAttribute<T> : Attribute
{
    public Type Interface => typeof(T);
}

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public sealed class TransientAttribute(Type serviceType) : Attribute
{
    public Type Interface { get; } = serviceType ?? throw new ArgumentNullException(nameof(serviceType));
}
