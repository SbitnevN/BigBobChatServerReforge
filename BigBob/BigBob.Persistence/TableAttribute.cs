using System.Runtime.CompilerServices;

namespace BigBob.Persistence;

public class TableAttribute([CallerMemberName] string name = "") : Attribute
{
    public string Name { get; } = name;
}