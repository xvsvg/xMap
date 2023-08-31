using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using xMap.Tools.Contracts;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace xMap.Generation.DirectiveSection;

internal class UsingSectionLink : ILink
{
    public ILink Next { get; set; } = null!;

    public void Process(SourceProductionContext context, SemanticModel semanticModel, CompilationUnitSyntax source)
    {
        var usedTypes = new List<ITypeSymbol>();

        var root = semanticModel.SyntaxTree.GetRoot() as CompilationUnitSyntax;
        var uniqueNamespaces = root!.DescendantNodes()
            .OfType<QualifiedNameSyntax>()
            .Select(qn => qn.Left.ToString())
            .Distinct();

        var usingDirectives = uniqueNamespaces
            .Select(ns => UsingDirective(ParseName(ns))).ToList();

        var firstNamespaceType = root.ChildNodes()
            .OfType<FileScopedNamespaceDeclarationSyntax>().FirstOrDefault();

        var secondNamespaceType =root.ChildNodes()
            .OfType<NamespaceDeclarationSyntax>().FirstOrDefault();
        
        // var attributes = classDeclaration.AttributeLists.First().Attributes;
        //
        // var references = new List<Assembly>();
        // foreach (var argument in attributes.SelectMany(x => x.ArgumentList.Arguments))
        // {
        //     var argumentType = semanticModel.GetTypeInfo(argument.Expression).Type;
        //     var assembly = argumentType.GetType().Assembly;
        //     references.Add(assembly);
        // }
        //
        // references.ForEach(x => semanticModel.Compilation.AddReferences(
        //     MetadataReference.CreateFromFile(x.Location)));

        source = source
            .WithUsings(
                new SyntaxList<UsingDirectiveSyntax>(usingDirectives))
            .WithMembers(
                SingletonList<MemberDeclarationSyntax>(
                    NamespaceDeclaration(
                            IdentifierName(firstNamespaceType?.Name.GetText(Encoding.UTF8).ToString() ??
                                           secondNamespaceType!.Name.GetText(Encoding.UTF8).ToString()))
                        .WithCloseBraceToken(
                            MissingToken(SyntaxKind.CloseBraceToken))))
            .NormalizeWhitespace();

        Next?.Process(context, semanticModel, source);
    }
}