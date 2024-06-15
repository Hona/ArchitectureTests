using Hona.ArchitectureTests.ApplicationParts;

namespace Hona.ArchitectureTests.Rules;

public interface IRule
{
    public IApplicationPart Source { get; }
    public IApplicationPart Target { get; }

    RuleOutcome IsCompliant();
}