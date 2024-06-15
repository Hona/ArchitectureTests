using System.Net.Sockets;
using System.Reflection;
using Hona.ArchitectureTests;
using Hona.ArchitectureTests.ApplicationParts;

namespace SampleApplication.ArchitectureTests;

public class ArchitectureTests
{
    private static readonly Assembly SampleAppAssembly = typeof(ISampleApp).Assembly;

    private static readonly IApplicationPart Presentation =
        new TypesPart([typeof(ConsolePresenter), typeof(Program), typeof(LogHelper)]);

    private static readonly IApplicationPart Application =
        new NamespacePart(SampleAppAssembly, ".Jobs");

    private static readonly IApplicationPart Domain =
        new NamespacePart(SampleAppAssembly, ".Models");

    private static readonly IApplicationPart Infrastructure =
        new NamespacePart(SampleAppAssembly, ".Data");
    
    [Fact]
    public void Jobs_DontDependOn_PresentationLogic()
    {
        Ensure.That(Presentation)
            .DoesNotDependOn(Application);
    }

    [Fact]
    public void Presentation_DependsOn_Domain()
    {
        Ensure.That(Presentation)
            .DependsOn(Domain);
    }
    
    [Fact]
    public void Application_DependsOn_Domain()
    {
        Ensure.That(Application)
            .DependsOn(Domain);
    }

    [Fact]
    public void Domain_DoesntDependOn_Infrastructure()
    {
        Ensure.That(Domain)
            .DoesNotDependOn(Infrastructure);
    }
    
    
}