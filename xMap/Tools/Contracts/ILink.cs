using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace xMap.Tools.Contracts;

internal interface ILink
{
    ILink Next { get; set; }
    void Process(SourceProductionContext context, SemanticModel semanticModel, CompilationUnitSyntax source);
}