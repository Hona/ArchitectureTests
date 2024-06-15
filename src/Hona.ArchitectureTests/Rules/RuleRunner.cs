using Hona.ArchitectureTests.ApplicationParts;

namespace Hona.ArchitectureTests.Rules;

public class RuleRunner(IApplicationPart source)
{
    public RuleOutcome DependsOn(IApplicationPart target) =>
        new DependencyDirectionRule
        {
            Source = source,
            Target = target,
            Direction = DependencyDirection.SourceToTarget
        }.IsCompliant();

    public RuleOutcome DoesNotDependOn(IApplicationPart target) =>
        new DependencyDirectionRule
        {
            Source = source,
            Target = target,
            Direction = DependencyDirection.TargetToSource
        }.IsCompliant();
}