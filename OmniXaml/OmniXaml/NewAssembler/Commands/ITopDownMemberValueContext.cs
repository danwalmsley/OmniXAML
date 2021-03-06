namespace OmniXaml.NewAssembler.Commands
{
    using Typing;

    public interface ITopDownMemberValueContext
    {
        void SetMemberValue(XamlType member, object instance);
        object GetMemberValue(XamlType type);
    }
}