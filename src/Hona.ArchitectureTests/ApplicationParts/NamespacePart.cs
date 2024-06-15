using System.Reflection;

namespace Hona.ArchitectureTests.ApplicationParts;

public record NamespacePart(Assembly Assembly, string NamespacePartial, bool Inverted = false) : IApplicationPart
{
    public List<Type> GetTypes()
    {
        return Assembly.GetTypes().Where(t => t.Namespace?.Contains(NamespacePartial, StringComparison.OrdinalIgnoreCase) ?? false).ToList();
    }
}