using Hona.ArchitectureTests.ApplicationParts;

namespace Hona.ArchitectureTests.Architectures;

public class CleanArchitectureConfig
{
    public IApplicationPart Presentation { get; set; } = null!;
    public IApplicationPart Application { get; set; } = null!;
    public IApplicationPart Domain { get; set; } = null!;
    public IApplicationPart Infrastructure { get; set; } = null!;

    public void EnsureValid()
    {
        if (Presentation is null)
        {
            throw new InvalidOperationException();
        }
        
        if (Application is null)
        {
            throw new InvalidOperationException();
        }
        
        if (Domain is null)
        {
            throw new InvalidOperationException();
        }
        
        if (Infrastructure is null)
        {
            throw new InvalidOperationException();
        }
    }
}