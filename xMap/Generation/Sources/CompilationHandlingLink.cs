using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using xMap.Tools.Contracts;

namespace xMap.Generation.Sources;

internal class CompilationHandlingLink : ILink
{
    public ILink Next { get; set; } = null!;

    public void Process(SourceProductionContext context, SemanticModel semanticModel, CompilationUnitSyntax source)
    {
        context.AddSource("MapperExtensions.g.s", source.GetText(Encoding.UTF8));
        Next?.Process(context, semanticModel, source);
    }
}