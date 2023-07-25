using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace xMap.Tests.Tools;

public static class TestHelper
{
    public static Compilation RunGenerators(
        Compilation compilation,
        out ImmutableArray<Diagnostic> diagnostics,
        IIncrementalGenerator generator)
    {
        CSharpGeneratorDriver
            .Create(generator)
            .RunGeneratorsAndUpdateCompilation(compilation, out var newCompilation, out diagnostics);

        return newCompilation;
    }
}