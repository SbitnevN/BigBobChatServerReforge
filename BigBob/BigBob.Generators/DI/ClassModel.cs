namespace BigBob.Generators.DI;

public class ClassModel
{
    public string Name { get; private set; } = null!;

    public IEnumerable<PropertyModel> Properties { get; private set; } = null!;
}
