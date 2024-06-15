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
    public bool IsCompliant()
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
        
        return typesThatNeverDependOnOther.All(t => otherTypes.All(o => DoesntDependOn(t, o)));
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

    private static bool DoesntDependOn(Type sourceType, Type targetType)
    {
        var sourceDependencies = GetDependencies(sourceType).ToList();
        return sourceDependencies.Count == 0 ||
               !sourceDependencies.Any(x => x == targetType || (targetType.IsGenericType && IsGenericTypeOf(targetType.GetGenericTypeDefinition(), x)));
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
        
        return allTypesUsed.DistinctBy(x => x.FullName);
    }
}