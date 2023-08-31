using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using xMap.Tools;

namespace xMap.Generation;

public abstract class GeneratorBase : IIncrementalGenerator
{
    internal Chain Chain = null!;

    public abstract void Initialize(IncrementalGeneratorInitializationContext context);

    protected void Execute(
        Compilation compilation,
        ImmutableArray<ClassDeclarationSyntax> classes,
        SourceProductionContext context)
    {
        context.CancellationToken.ThrowIfCancellationRequested();

        if (classes.IsDefaultOrEmpty)
            return;

        foreach (var syntaxTree in compilation.SyntaxTrees)
        {
            var semanticModel = compilation.GetSemanticModel(syntaxTree);
            // if (semanticModel is null)
                // return;
            Chain.Process(context, semanticModel);
        }
    }
}