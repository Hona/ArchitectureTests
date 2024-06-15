namespace Hona.ArchitectureTests.ApplicationParts;

public class AggregatePart(List<IApplicationPart>? parts = null) : IApplicationPart
{
    public string? Name { get; set; }
    public bool Inverted { get; init; }
    public List<Type> GetTypes()
    {
        return Parts.SelectMany(p => p.GetTypes()).ToList();
    }

    public List<IApplicationPart> Parts => parts ?? [];

    public override string ToString() => Name ?? '[' + string.Join(", ", Parts.Select(p => p.ToString())) + ']';
}