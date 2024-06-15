namespace Hona.ArchitectureTests.ApplicationParts;

public record TypesPart(List<Type> Types, bool Inverted = false) : IApplicationPart
{
    public List<Type> GetTypes()
    {
        return Types;
    }
}