using Hona.ArchitectureTests.ApplicationParts;

namespace Hona.ArchitectureTests.Rules;

public class RuleRunner(IApplicationPart source)
{
    public void DependsOn(IApplicationPart target)
    {
        new DependencyDirectionRule()
        {
            Source = source,
            Target = target,
            Direction = DependencyDirection.SourceToTarget
        }.IsCompliant().Assert();
    }

    public void DoesNotDependOn(IApplicationPart target)
    {
        new DependencyDirectionRule()
        {
            Source = source,
            Target = target,
            Direction = DependencyDirection.TargetToSource
        }.IsCompliant().Assert();
    }
}