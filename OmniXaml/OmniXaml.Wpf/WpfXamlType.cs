﻿namespace OmniXaml.Wpf
{
    using System;
    using Typing;

    public class WpfXamlType : XamlType
    {
        private readonly IXamlTypeRepository wiringContext;
        private readonly ITypeFactory typeFactory;

        public WpfXamlType(Type type, IXamlTypeRepository wiringContext, ITypeFactory typeFactory) : base(type, wiringContext, typeFactory)
        {
            this.wiringContext = wiringContext;
            this.typeFactory = typeFactory;
        }

        protected override XamlMember LookupMember(string name)
        {
            return new WpfXamlMember(name, this, wiringContext, typeFactory);
        }

        protected override AttachableXamlMember LookupAttachableMember(string name)
        {
            return new WpfXamlAttachableMember(name, this, wiringContext, typeFactory);
        }
    }
}