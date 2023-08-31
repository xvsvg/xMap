using System.Diagnostics;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using xMap.Generation.ClassSection;
using xMap.Generation.CommentSection;
using xMap.Generation.DirectiveSection;
using xMap.Generation.Sources;
using xMap.Tools;

namespace xMap.Generation.Generator;

[Generator]
public class Generator : GeneratorBase
{
    public override void Initialize(IncrementalGeneratorInitializationContext context)
    {
        Chain = Chain.Builder
            .StartWith<UsingSectionLink>()
            .Then<CommentSectionLink>()
            // .Then<MethodSectionLink>()
            .FinishWith<CompilationHandlingLink>()
            .BuildChain();

        var classDeclarations = context.SyntaxProvider
            .CreateSyntaxProvider(
                (s, _) => IsSyntaxTarget(s),
                (ctx, _) => GetSemanticTarget(ctx))
            .Where(static c => c is not null);

        var compilationAndClasses = context
            .CompilationProvider.Combine(classDeclarations.Collect());

        context.RegisterSourceOutput(compilationAndClasses,
            (spc, source) => Execute(source.Left, source.Right, spc));
    }

    private static bool IsSyntaxTarget(SyntaxNode node)
    {
        return node is ClassDeclarationSyntax cds
               && cds.Modifiers.Any(SyntaxKind.PartialKeyword);
    }

    private static ClassDeclarationSyntax GetSemanticTarget(GeneratorSyntaxContext context)
    {
        var classDeclaration = (ClassDeclarationSyntax)context.Node;

        foreach (var attributeList in classDeclaration.AttributeLists)
        foreach (var attribute in attributeList.Attributes)
            if (attribute.Name.ToString().Equals("Mapper"))
                return classDeclaration;

        return null!;
    }
}