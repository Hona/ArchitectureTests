using System.Net.Sockets;
using System.Reflection;
using Hona.ArchitectureTests;
using Hona.ArchitectureTests.ApplicationParts;
using Hona.ArchitectureTests.Runner;
using SampleApplication.Integrations;

namespace SampleApplication.ArchitectureTests;

public class ArchitectureTests
{
    private static readonly Assembly SampleAppAssembly = typeof(ISampleApp).Assembly;

    private static readonly IApplicationPart Presentation =
        new TypesPart([typeof(ConsolePresenter), typeof(Program), typeof(LogHelper)]);

    private static readonly IApplicationPart Application =new AggregatePart(
    [
        new NamespacePart(SampleAppAssembly, ".Jobs"),
        new NamespacePart(SampleAppAssembly, ".Services")
    ]);

    private static readonly IApplicationPart Domain =
        new NamespacePart(SampleAppAssembly, ".Models");

    private static readonly IApplicationPart Infrastructure = new AggregatePart(
    [
        new NamespacePart(SampleAppAssembly, ".Data"),
        new AssemblyPart(typeof(WebApiClient).Assembly)
    ]);
    
    /*        yield return That(config.Presentation)
            .DependsOn(config.Application);
        yield return That(config.Presentation)
            .DependsOn(config.Domain);
        yield return That(config.Presentation)
            .DependsOn(config.Infrastructure);
        
        yield return That(config.Application)
            .DependsOn(config.Domain);
        
        yield return That(config.Infrastructure)
            .DependsOn(config.Application);
        yield return That(config.Infrastructure)
            .DependsOn(config.Domain);*/
    
    [Fact]
    public void Presentation_DependsOn_Application()
    {
        Ensure.That(Presentation)
            .DependsOn(Application)
            .Assert();
    }
    
    [Fact]
    public void Application_DependsOn_Domain()
    {
        Ensure.That(Presentation)
            .DependsOn(Domain)
            .Assert();
    }
    
    [Fact]
    public void Infrastructure_DependsOn_Application()
    {
        Ensure.That(Presentation)
            .DependsOn(Infrastructure)
            .Assert();
    }
    
    [Fact]
    public void Presentation_DependsOn_Domain()
    {
        Ensure.That(Infrastructure)
            .DependsOn(Application)
            .Assert();
    }
    
    [Fact]
    public void Infrastructure_DependsOn_Domain()
    {
        Ensure.That(Infrastructure)
            .DependsOn(Domain)
            .Assert();
    }
}