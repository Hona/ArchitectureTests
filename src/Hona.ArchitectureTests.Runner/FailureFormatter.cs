using FluentAssertions.Formatting;
using Hona.ArchitectureTests.Rules;

namespace Hona.ArchitectureTests.Runner;

public class FailureFormatter : IValueFormatter
{
    public bool CanHandle(object value)
    {
        return value is Failure;
    }

    public void Format(object value, FormattedObjectGraph formattedGraph, FormattingContext context, FormatChild formatChild)
    {
        var failure = (Failure)value;

        formattedGraph.AddFragmentOnNewLine(failure.Message);
    }
}