using System.Runtime.CompilerServices;

namespace Hona.ArchitectureTests.Runner;

public class ModuleInitializer
{
#pragma warning disable CA2255
    [ModuleInitializer]
#pragma warning restore CA2255
    public static void Initialize()
    {
        FluentAssertions.Formatting.Formatter.AddFormatter(new FailureFormatter());
    }
}