using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Ater.Web.Analyzer
{
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(AterWebAnalyzerCodeFixProvider)), Shared]
    public class AterWebAnalyzerCodeFixProvider : CodeFixProvider
    {
        public sealed override ImmutableArray<string> FixableDiagnosticIds
        {
            get { return ImmutableArray.Create(AterWebAnalyzerAnalyzer.DiagnosticId); }
        }

        public sealed override FixAllProvider GetFixAllProvider()
        {
            return WellKnownFixAllProviders.BatchFixer;
        }

        public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);

            var diagnostic = context.Diagnostics.First();
            var diagnosticSpan = diagnostic.Location.SourceSpan;

            var declaration = root.FindToken(diagnosticSpan.Start).Parent.AncestorsAndSelf().OfType<EnumMemberDeclarationSyntax>().First();

            context.RegisterCodeFix(
                CodeAction.Create(
                    title: CodeFixResources.CodeFixTitle,
                    createChangedSolution: c => AddDescriptionAsync(context.Document, declaration, c),
                    equivalenceKey: nameof(CodeFixResources.CodeFixTitle)),
                diagnostic);
        }

        private async Task<Solution> AddDescriptionAsync(Document document, EnumMemberDeclarationSyntax typeDecl, CancellationToken cancellationToken)
        {
            // 获取枚举类中的所有字段
            var semanticModel = await document.GetSemanticModelAsync(cancellationToken).ConfigureAwait(false);
            var typeSymbol = semanticModel.GetDeclaredSymbol(typeDecl);

            var fieldSymbol = typeSymbol;

            var xmlString = fieldSymbol.GetDocumentationCommentXml();
            if (string.IsNullOrWhiteSpace(xmlString)) return document.Project.Solution;
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlString);

            var description = xmlDoc.SelectSingleNode("member/summary")?.InnerText.Trim();

            if (!string.IsNullOrWhiteSpace(description))
            {
                var newAttribute = SyntaxFactory.Attribute(SyntaxFactory.ParseName("Description"), SyntaxFactory.ParseAttributeArgumentList($"(\"{description}\")"));
                var newAttributeList = SyntaxFactory.AttributeList(SyntaxFactory.SingletonSeparatedList(newAttribute));

                // 使用新的属性列表替换原有的属性列表
                var newField = fieldSymbol.DeclaringSyntaxReferences.First().
                    GetSyntax(cancellationToken) as EnumMemberDeclarationSyntax;

                var newFieldWithAttribute = newField.AddAttributeLists(newAttributeList);

                var newRoot = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);

                newRoot = newRoot.ReplaceNode(newField, newFieldWithAttribute);
                // 如果不存在引用:System.ComponentModel, 则添加
                if (!newRoot.DescendantNodes().OfType<UsingDirectiveSyntax>().Any(u => u.Name.ToString() == "System.ComponentModel"))
                {
                    var newNamespace = SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("System.ComponentModel"));
                    newRoot = newRoot.InsertNodesBefore(newRoot.ChildNodes().First(), new[] { newNamespace });
                }
                var newDocument = document.WithSyntaxRoot(newRoot);
                return newDocument.Project.Solution;
            }
            return document.Project.Solution;
        }
    }
}
