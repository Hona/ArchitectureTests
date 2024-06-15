using FluentAssertions;
using FluentAssertions.Execution;
using Hona.ArchitectureTests.Rules;

namespace Hona.ArchitectureTests.Runner;

public static class RuleOutcomeExtensions
{
    public static void Assert(this RuleOutcome outcome)
    {
        using var scope = new AssertionScope();

        outcome.IsCompliant.Should().BeTrue(outcome.Rule.ToString());
        
        var failures = outcome.Failures;
        
        failures.Should().BeEmpty();
    }

    public static void Assert(this IEnumerable<RuleOutcome> outcome)
    {
        using var scope = new AssertionScope();
        
        foreach (var ruleOutcome in outcome)
        {
            ruleOutcome.Assert();
        }
    }
}