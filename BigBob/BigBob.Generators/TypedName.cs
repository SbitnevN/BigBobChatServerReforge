namespace BigBob.Generators;

public struct TypedName(string type, string name)
{
    public string Name { get; set; } = name;

    public string Type { get; set; } = type;
}
