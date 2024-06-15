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

    public static IEnumerable<RuleOutcome> CleanArchitecture(Action<CleanArchitectureConfig> setup)
    {
        var config = new CleanArchitectureConfig();

        setup(config);
        
        config.EnsureValid();

        config.Presentation.Name = nameof(config.Presentation);
        config.Application.Name = nameof(config.Application);
        config.Domain.Name = nameof(config.Domain);
        config.Infrastructure.Name = nameof(config.Infrastructure);
        
        yield return That(config.Presentation)
            .DependsOn(config.Application);
        yield return That(config.Presentation)
            .DependsOn(config.Domain);
        yield return That(config.Presentation)
            .DependsOn(config.Infrastructure);
        
        yield return That(config.Application)
            .DependsOn(config.Domain);
        
        yield return That(config.Infrastructure)
            .DependsOn(config.Application);
        yield return That(config.Infrastructure)
            .DependsOn(config.Domain);
    }
}