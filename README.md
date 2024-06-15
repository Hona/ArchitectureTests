# Hona.ArchitectureTests

## Usage

The most simple way to use the library is to use the predefined architecture suites.

```csharp
[Fact]
public void CleanArchitecture()
{
    Ensure.CleanArchitecture(x =>
    {
        x.Presentation = new AggregatePart(
        [
            new AssemblyPart(typeof(WebApi.DependencyInjection).Assembly),
            new AssemblyPart(typeof(GraphQL.DependencyInjection).Assembly)
        ]);

        x.Application = new AssemblyPart(typeof(Application.DependencyInjection).Assembly);

        x.Domain = new AssemblyPart(typeof(Domain.Customers.CustomerId).Assembly);

        x.Infrastructure = new AggregatePart(
        [
            new AssemblyPart(typeof(Infrastructure.DependencyInjection).Assembly),
            /*new TypesPart(
            [
                typeof(Microsoft.EntityFrameworkCore.DbContext),
                typeof(Microsoft.EntityFrameworkCore.DbSet<>),
            ])*/
        ]);
    });
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
```