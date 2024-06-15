using Hona.ArchitectureTests.ApplicationParts;
using Hona.ArchitectureTests.Architectures;
using Hona.ArchitectureTests.Rules;

namespace Hona.ArchitectureTests;

public static class Ensure
{
    public static RuleRunner That(IApplicationPart part)
    {
        return new RuleRunner(part);
    }

    public static void CleanArchitecture(Action<CleanArchitectureConfig> setup)
    {
        var config = new CleanArchitectureConfig();

        setup(config);
        
        config.EnsureValid();
        
        That(config.Presentation)
            .DependsOn(config.Application);
        That(config.Presentation)
            .DependsOn(config.Domain);
        That(config.Presentation)
            .DependsOn(config.Infrastructure);
        
        That(config.Application)
            .DependsOn(config.Domain);
        
        That(config.Infrastructure)
            .DependsOn(config.Application);
        That(config.Infrastructure)
            .DependsOn(config.Domain);
    }
}