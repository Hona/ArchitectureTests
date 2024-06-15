# Hona.ArchitectureTests

[![](https://img.shields.io/nuget/v/Hona.ArchitectureTests.Runner)](https://www.nuget.org/packages/Hona.ArchitectureTests.Runner)

## Usage

The most simple way to use the library is to use the predefined architecture suites.

```csharp
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
    }).Assert();
}
```

For custom architectures, you can define application parts, then setup the rules. For example a basic 'Clean Architecture' implementation without the suite, would look like this

```csharp
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
        .DependsOn(Application)
        .Assert();
}

[Fact]
public void Application_DependsOn_Domain()
{
    Ensure.That(Application)
        .DependsOn(Domain)
        .Assert();
}

[Fact]
public void Infrastructure_DependsOn_Application()
{
    Ensure.That(Infrastructure)
        .DependsOn(Application)
        .Assert();
}
```
