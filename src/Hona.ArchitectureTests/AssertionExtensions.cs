namespace Hona.ArchitectureTests;

public static class AssertionExtensions
{
    public static void Assert(this bool ruleOutcome)
    {
        if (ruleOutcome is false)
        {
            throw new NotImplementedException("Depending on the test framework, assert a failure");
        }
    }
}