using System.Runtime.CompilerServices;

namespace BigBob.Persistence
{
    public class StoredProcedureAttribute([CallerMemberName] string name = "") : Attribute
    {
        public string Name { get; } = name;
    }
}
