using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using xMap.Tools.Contracts;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace xMap.Generation.ClassSection;

internal class MethodSectionLink : ILink
{
    public ILink Next { get; set; } = null!;

    public void Process(SourceProductionContext context, SemanticModel semanticModel, CompilationUnitSyntax source)
    {
        var attributeArguments = semanticModel.SyntaxTree.GetRoot()
            .DescendantNodes()
            .OfType<ClassDeclarationSyntax>()
            .First()
            .AttributeLists
            .First()
            .Attributes
            .SelectMany(x => x.ArgumentList!.Arguments)
            .Select(argument => argument.Expression.ToString())
            .Select(typeSyntax => typeSyntax.TrimStart("typeof(".ToCharArray()).TrimEnd(')'))
            .ToList();

        // semanticModel.GetTypeInfo(ParseTypeName(x.Expression.ToString())));

        source = source
            .WithMembers(
                SingletonList<MemberDeclarationSyntax>(
                    ClassDeclaration("Mapper")
                        .WithModifiers(
                            TokenList(
                                Token(SyntaxKind.PublicKeyword),
                                Token(SyntaxKind.StaticKeyword)))
                        .WithTypeParameterList(
                            TypeParameterList(
                                SeparatedList<TypeParameterSyntax>(
                                    new SyntaxNodeOrToken[]
                                    {
                                        TypeParameter(
                                            Identifier(attributeArguments.First())),
                                        Token(SyntaxKind.CommaToken),
                                        TypeParameter(
                                            Identifier(attributeArguments.Last()))
                                    })))
                        .WithMembers(
                            SingletonList<MemberDeclarationSyntax>(
                                MethodDeclaration(
                                        IdentifierName(attributeArguments.Last()),
                                        Identifier("ToDto"))
                                    .WithModifiers(
                                        TokenList(
                                            Token(SyntaxKind.PublicKeyword),
                                            Token(SyntaxKind.StaticKeyword)))
                                    .WithParameterList(
                                        ParameterList(
                                            SingletonSeparatedList(
                                                Parameter(
                                                        Identifier("source"))
                                                    .WithModifiers(
                                                        TokenList(
                                                            Token(SyntaxKind.ThisKeyword)))
                                                    .WithType(
                                                        IdentifierName(attributeArguments.First())))))
                                    .WithBody(
                                        Block(
                                            SingletonList<StatementSyntax>(
                                                ReturnStatement(
                                                    IdentifierName("source")))))))))
            .NormalizeWhitespace();

        Next?.Process(context, semanticModel, source);
    }
}