namespace OmniXaml.Wpf
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Xaml;
    using AppServices;
    using SystemXamlNsDeclaration = System.Xaml.NamespaceDeclaration;
    using SystemXamlType = System.Xaml.XamlType;
    using SystemXamlNodeType = System.Xaml.XamlNodeType;

    public class TemplateContentToWpfXamlReaderAdapter : XamlReader, IXamlIndexingReader
    {
        private readonly IEnumerator<XamlNode> nodeStream;
        private readonly TemplateContent templateContent;
        private bool hasReadSuccess;

        public TemplateContentToWpfXamlReaderAdapter(TemplateContent templateContent,
            InflatableTypeFactory inflatableTypeFactory,
            XamlSchemaContext xamlSchemaContext)
        {
            this.templateContent = templateContent;
            this.SchemaContext = xamlSchemaContext;

            var inflater = new NodeInflater(inflatableTypeFactory.Inflatables, templateContent.Context);
            var inflatedNodes = inflater.Inflate(templateContent.Nodes);

            nodeStream = inflatedNodes.GetEnumerator();
        }

        public override SystemXamlNodeType NodeType => XamlTypeConversion.ToWpf(Current.NodeType);
        public override bool IsEof => !hasReadSuccess;
        public override SystemXamlNsDeclaration Namespace => XamlTypeConversion.ToWpf(Current.NamespaceDeclaration);
        public override SystemXamlType Type => XamlTypeConversion.ToWpf(Current.XamlType, SchemaContext);
        private XamlNode Current => nodeStream.Current;
        public override object Value => Current.Value;
        public override XamlMember Member => XamlTypeConversion.ToWpf(Current.Member, SchemaContext);
        public override XamlSchemaContext SchemaContext { get; }

        public int Count => templateContent.Nodes.Count();
        public int CurrentIndex { get; set; } = -1;

        public override bool Read()
        {
            hasReadSuccess = nodeStream.MoveNext();
            if (hasReadSuccess)
            {
                CurrentIndex++;
            }

            return hasReadSuccess;
        }
    }
}