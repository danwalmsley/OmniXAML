namespace OmniXaml.Assembler
{
    using Typing;

    internal class Scope
    {
        public XamlType XamlType { get; set; }
        public XamlMember Member { get; set; }
        public object Instance { get; set; }
        public bool IsPropertyValueSet { get; set; }
        public bool IsObjectFromMember { get; set; }
        public object Collection { get; set; }
    }
}