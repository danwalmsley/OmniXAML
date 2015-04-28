namespace OmniXaml.Parsers.MarkupExtensions
{
    public class IdentifierNode
    {
        public string Prefix { get; }
        public string TypeName { get; }

        public IdentifierNode(string typeName)
        {
            TypeName = typeName + "Extension";
        }

        public IdentifierNode(string prefix, string typeName)
        {
            Prefix = prefix;
            TypeName = typeName;
        }

        protected bool Equals(IdentifierNode other)
        {
            return string.Equals(Prefix, other.Prefix) && string.Equals(TypeName, other.TypeName);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            if (obj.GetType() != GetType())
            {
                return false;
            }
            return Equals((IdentifierNode) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Prefix != null ? Prefix.GetHashCode() : 0)*397) ^ (TypeName != null ? TypeName.GetHashCode() : 0);
            }
        }
    }
}