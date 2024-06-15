using System.Reflection;

namespace Hona.ArchitectureTests.ApplicationParts;

public record AssemblyPart(Assembly Assembly) : IApplicationPart
{
    public string? Name { get; set; }
    public bool Inverted { get; init; }
    public List<Type> GetTypes()
    {
        return Assembly.GetTypes().ToList();
    }

    public override string ToString() => Name ?? Assembly.GetName().Name ?? "Unknown Assembly";
}