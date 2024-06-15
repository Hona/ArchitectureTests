using System.Reflection;
using Hona.ArchitectureTests.ApplicationParts;

namespace Hona.ArchitectureTests.Rules;


public enum DependencyDirection
{
    SourceToTarget,
    TargetToSource,
    Neither
}

/// <summary>
/// Source, Target, Direction:SourceToTarget, reads as Target never can depend on Source.
/// And Source may depend on Target
/// </summary>
public class DependencyDirectionRule : IRule
{
    public required IApplicationPart Source { get; init; }
    public required IApplicationPart Target { get; init;}
    public required DependencyDirection Direction { get; init;}
    
    /// <summary>
    /// The base check is that the source does NOT depend on the target.
    /// To check that x depends on y, then check that y does not depend on x.
    /// The Direction property is used to determine the direction of the check.
    /// </summary>
    public RuleOutcome IsCompliant()
    {
        var sourceTypes = Source.GetTypes();
        AddTransientTypesIfAssemblyPart(Source, sourceTypes);
        
        var targetTypes = Target.GetTypes();
        AddTransientTypesIfAssemblyPart(Target, targetTypes);

        var typesThatNeverDependOnOther = Direction switch
        {
            DependencyDirection.SourceToTarget => targetTypes,
            DependencyDirection.TargetToSource => sourceTypes,
            _ => throw new ArgumentOutOfRangeException()
        };
        
        var otherTypes = Direction switch
        {
            DependencyDirection.SourceToTarget => sourceTypes,
            DependencyDirection.TargetToSource => targetTypes,
            _ => throw new ArgumentOutOfRangeException()
        };
        
        var outcome = new RuleOutcome
        {
            Rule = this
        };

        foreach (var t in typesThatNeverDependOnOther)
        {
            foreach (var o in otherTypes)
            {
                var failures = DoesntDependOn(t, o);
                if (failures is not null)
                {
                    foreach (var failure in failures)
                    {
                        outcome.Failures.Add(failure);
                    }
                }
            }
        }

        return outcome;
    }
    
    private void AddTransientTypesIfAssemblyPart(IApplicationPart part, List<Type> dependencies)
    {
        if (part is AssemblyPart assemblyPart)
        {
            dependencies.AddRange(assemblyPart.GetTypes());
        }
        else if (part is AggregatePart aggregatePart)
        {
            var assemblyParts = aggregatePart.Parts.OfType<AssemblyPart>();
            
            foreach (var subAssemblyPart in assemblyParts)
            {
                dependencies.AddRange(subAssemblyPart.GetTypes());
            }
        }
    }

    private static IEnumerable<Failure>? DoesntDependOn(Type sourceType, Type targetType)
    {
        var sourceDependencies = GetDependencies(sourceType).ToList();

        if (sourceDependencies.Count is 0)
        {
            yield break;
        }

        var filteredSourceDependencies = sourceDependencies
            .Where(x => !x.FullName.StartsWith("System."))
            .Where(x => !x.FullName.StartsWith("Microsoft."));

        foreach (var x in filteredSourceDependencies)
        {
            if ((x == targetType || (targetType.IsGenericType && 
                                      IsGenericTypeOf(targetType.GetGenericTypeDefinition(), x))))
            {
                yield return new Failure($"{sourceType.FullName} depends on {x.FullName}");
            }
        }
    }

    private static bool IsGenericTypeOf(Type genericType, Type someType)
    {   
        if (someType.IsGenericType 
            && genericType == someType.GetGenericTypeDefinition()) return true;

        return someType.BaseType != null 
               && IsGenericTypeOf(genericType, someType.BaseType);
    }

    private static IEnumerable<Type> GetDependencies(Type type)
    {
        var allTypesUsed = new List<Type>();

        var typesInConstructor = type
            .GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
            .SelectMany(c => c.GetParameters())
            .Select(p => p.ParameterType);
        allTypesUsed.AddRange(typesInConstructor);

        var typesInProperties = type.GetProperties()
            .Select(p => p.PropertyType);
        allTypesUsed.AddRange(typesInProperties);

        var typesInMethods = type.GetMethods()
            .SelectMany(m => m.GetParameters())
            .Select(p => p.ParameterType);
        allTypesUsed.AddRange(typesInMethods);
        
        return allTypesUsed.Distinct();
    }

    public override string ToString() => $"{Source} {DirectionFriendlyName} {Target}";

    private string DirectionFriendlyName => Direction switch
    {
        DependencyDirection.SourceToTarget => "depends on",
        DependencyDirection.TargetToSource => "does not depend on",
        DependencyDirection.Neither => "mutually exclusive",
        _ => throw new ArgumentOutOfRangeException()
    };
}