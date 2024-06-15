namespace Hona.ArchitectureTests.Rules;

public record Failure(string Message);
public class RuleOutcome
{
    public required IRule Rule { get; set; }
    public bool IsCompliant => Failures.Count == 0;
    public HashSet<Failure> Failures { get; set; } = [];
}