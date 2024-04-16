using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Ater.Web.Analyzer
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class AterWebAnalyzerAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "AterWebAnalyzer";

        private static readonly LocalizableString Title = new LocalizableResourceString(nameof(Resources.AnalyzerTitle), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString MessageFormat = new LocalizableResourceString(nameof(Resources.AnalyzerMessageFormat), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString Description = new LocalizableResourceString(nameof(Resources.AnalyzerDescription), Resources.ResourceManager, typeof(Resources));
        private const string Category = "Naming";

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Info, isEnabledByDefault: true, description: Description);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(Rule); } }

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();
            context.RegisterSymbolAction(AnalyzeSymbol, SymbolKind.Field);
        }

        private static void AnalyzeSymbol(SymbolAnalysisContext context)
        {
            var fieldSymbol = (IFieldSymbol)context.Symbol;
            // 其所属类是否为枚举类
            if (fieldSymbol.ContainingType.TypeKind == TypeKind.Enum)
            {
                if (!fieldSymbol.GetAttributes().Any(a => a.AttributeClass?.ToDisplayString() == "System.ComponentModel.DescriptionAttribute"))
                {
                    var diagnostic = Diagnostic.Create(Rule, fieldSymbol.Locations.First(), fieldSymbol.Name);
                    context.ReportDiagnostic(diagnostic);
                }
            }
        }
    }
}
