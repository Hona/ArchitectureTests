using System.Reflection;
using Hona.ArchitectureTests;
using Hona.ArchitectureTests.ApplicationParts;

namespace SampleApplication.ArchitectureTests;

public class ArchitectureTestsInnerwards
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
    public void Presentation_DependsOn_Application()
    {
        Ensure.That(Presentation)
            .DependsOn(Application);
    }
    
    [Fact]
    public void Application_DependsOn_Domain()
    {
        Ensure.That(Application)
            .DependsOn(Domain);
    }
    
    [Fact]
    public void Infrastructure_DependsOn_Application()
    {
        Ensure.That(Infrastructure)
            .DependsOn(Application);
    }
    
    
}