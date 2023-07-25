using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using NUnit.Framework;
using xMap.Generation.Generator;
using xMap.Tests.Tools;

namespace xMap.Tests;

public class GenerationTests
{
    [Test]
    public async Task ProvideValidSource_ShouldGenerate()
    {
        var source = await File.ReadAllTextAsync(@"../../../../xMap.Sample/Receiver.cs");
        var syntaxTree = CSharpSyntaxTree.ParseText(source);
        var refs = new[]
        {
            MetadataReference.CreateFromFile(typeof(object).Assembly.Location)
        };

        var compilation = CSharpCompilation.Create(
            typeof(object).Assembly.FullName,
            new[] { syntaxTree },
            refs);

        var updatedCompilation = TestHelper.RunGenerators(
            compilation,
            out var diagnostics,
            new Generator());

        var files = updatedCompilation.SyntaxTrees
            .Where(t => Path.GetFileName(t.FilePath) is not null);

        foreach (var file in files)
        {
            Console.WriteLine(file);
            Console.WriteLine(new string('-', 30));
        }
    }
}