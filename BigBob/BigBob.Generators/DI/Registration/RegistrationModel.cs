namespace BigBob.Generators.DI.Registration;

public enum LifeTime
{
    Scoped,
    Transient,
    Singleton,
}

internal class RegistrationModel
{
    public bool IsOpenGeneric { get; set; }

    public string Type { get; set; } = string.Empty;

    public string Interface { get; set; } = string.Empty;

    public LifeTime LifeTime { get; set; }

    public bool NeedFactory { get; set; }
}
