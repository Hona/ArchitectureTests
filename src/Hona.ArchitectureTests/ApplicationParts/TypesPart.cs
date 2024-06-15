namespace Hona.ArchitectureTests.ApplicationParts;

public record TypesPart(List<Type> Types, bool Inverted = false) : IApplicationPart
{
    public string? Name { get; set; }
    public List<Type> GetTypes()
    {
        return Types;
    }

    public override string ToString() => Name ?? '[' + string.Join(", ", Types.Select(t => t.Name)) + ']';
}