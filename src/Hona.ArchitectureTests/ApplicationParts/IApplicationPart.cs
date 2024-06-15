namespace Hona.ArchitectureTests.ApplicationParts;

public interface IApplicationPart
{
    public string? Name { get; set; }
    public bool Inverted { get; init; }
    
    public List<Type> GetTypes();
}