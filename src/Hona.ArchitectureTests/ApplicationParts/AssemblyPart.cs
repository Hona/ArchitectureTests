using System.Reflection;

namespace Hona.ArchitectureTests.ApplicationParts;

public record AssemblyPart(Assembly Assembly) : IApplicationPart
{
    public bool Inverted { get; init; }
    public List<Type> GetTypes()
    {
        return Assembly.GetTypes().ToList();
    }
}