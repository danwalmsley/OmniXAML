namespace OmniXaml.Assembler.NodeWriters
{
    using Typing;

    internal class GetObjectWriter
    {
        private readonly ObjectAssembler objectAssembler;
        private readonly StateBag bag;

        public GetObjectWriter(ObjectAssembler objectAssembler)
        {
            this.objectAssembler = objectAssembler;
            bag = objectAssembler.Bag;
        }

        public void WriteGetObject()
        {
            objectAssembler.SetUnfinishedResult();

            if (bag.Current.Type != null)
            {
                bag.PushScope();
            }

            var property = GetPropertyForGetObject();

            bag.Current.IsCollectionHoldingObject = true;

            var valueOfProperty = GetValueOfMemberFromParentInstance(property);

            bag.Current.Instance = valueOfProperty;
            bag.Current.Type = property.XamlType;

            if (property.XamlType.IsContainer)
            {
                bag.Current.Collection = valueOfProperty;
            }
        }

        private MutableXamlMember GetPropertyForGetObject()
        {
            if (bag.Current.Type != null || bag.Depth <= 1)
            {
                return (MutableXamlMember)bag.Current.Property;
            }

            return (MutableXamlMember) bag.Parent.Property;
        }

        private object GetValueOfMemberFromParentInstance(MutableXamlMember property)
        {
            var parentInstance = bag.Parent.Instance;
            var valueOfProperty = TypeOperations.GetValue(parentInstance, property);
            return valueOfProperty;
        }
    }
}