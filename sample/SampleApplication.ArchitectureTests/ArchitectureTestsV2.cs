using System.Net.Sockets;
using System.Reflection;
using Hona.ArchitectureTests;
using Hona.ArchitectureTests.ApplicationParts;
using Hona.ArchitectureTests.Architectures;
using SampleApplication.Integrations;

namespace SampleApplication.ArchitectureTests;

public class ArchitectureTestsV2
{
    private static readonly Assembly SampleAppAssembly = typeof(ISampleApp).Assembly;

    [Fact]
    public void CleanArchitecture()
    {
        Ensure.CleanArchitecture(x =>
        {
            x.Presentation = new TypesPart([typeof(ConsolePresenter), typeof(Program), typeof(LogHelper)]);
            x.Application = new AggregatePart(
            [
                new NamespacePart(SampleAppAssembly, ".Jobs"),
                new NamespacePart(SampleAppAssembly, ".Services")
            ]);
            x.Domain = new NamespacePart(SampleAppAssembly, ".Models");
            x.Infrastructure = new AggregatePart(
            [
                new NamespacePart(SampleAppAssembly, ".Data"),
                new AssemblyPart(typeof(WebApiClient).Assembly)
            ]);
        });
    }
}