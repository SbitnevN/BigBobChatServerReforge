using System.Runtime.CompilerServices;

namespace BigBob.Persistencel;

public class StoredProcedureAttribute([CallerMemberName] string name = "") : Attribute
{
    public string Name { get; } = name;
}
