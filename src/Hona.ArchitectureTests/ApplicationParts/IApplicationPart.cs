namespace Hona.ArchitectureTests.ApplicationParts;

public interface IApplicationPart
{
    public bool Inverted { get; init; }
    
    public List<Type> GetTypes();
}