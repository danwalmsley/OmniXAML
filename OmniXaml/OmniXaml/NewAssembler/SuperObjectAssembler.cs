namespace OmniXaml.NewAssembler
{
    using System;
    using Assembler;
    using Commands;
    using Glass;
    using Typing;

    public class SuperObjectAssembler : IObjectAssembler
    {
        private readonly ITopDownMemberValueContext topDownMemberValueContext;
        private readonly Type rootInstanceType;
        private readonly object rootInstance;
        
        public SuperObjectAssembler(WiringContext wiringContext, ITopDownMemberValueContext topDownMemberValueContext, ObjectAssemblerSettings settings = null) : this(new StackingLinkedList<Level>(), wiringContext, topDownMemberValueContext)
        {
            Guard.ThrowIfNull(wiringContext, nameof(wiringContext));
            Guard.ThrowIfNull(topDownMemberValueContext, nameof(topDownMemberValueContext));

            this.topDownMemberValueContext = topDownMemberValueContext;
            StateCommuter.RaiseLevel();
            rootInstance = settings?.RootInstance;
            rootInstanceType = settings?.RootInstance?.GetType();            
        }

        public SuperObjectAssembler(StackingLinkedList<Level> state, WiringContext wiringContext, ITopDownMemberValueContext topDownMemberValueContext)
        {
            WiringContext = wiringContext;          
            StateCommuter = new StateCommuter(state, wiringContext, topDownMemberValueContext);
        }

        public object Result { get; set; }
        public EventHandler<XamlSetValueEventArgs> XamlSetValueHandler { get; set; }
        public WiringContext WiringContext { get; }

        public StateCommuter StateCommuter { get; }

        public void Process(XamlNode node)
        {
            Command command;

            switch (node.NodeType)
            {
                case XamlNodeType.NamespaceDeclaration:
                    command = new NamespaceDeclarationCommand(this, node.NamespaceDeclaration);
                    break;
                case XamlNodeType.StartObject:
                    command = new StartObjectCommand(this, node.XamlType, rootInstance);
                    break;
                case XamlNodeType.StartMember:
                    command = new StartMemberCommand(this, GetMember(node.Member));
                    break;
                case XamlNodeType.Value:
                    command = new ValueCommand(this, (string)node.Value);
                    break;
                case XamlNodeType.EndObject:
                    command = new EndObjectCommand(this);
                    break;
                case XamlNodeType.EndMember:
                    command = new EndMemberCommand(this, topDownMemberValueContext);
                    break;
                case XamlNodeType.GetObject:
                    command = new GetObjectCommand(this);
                    break;
                default:
                    throw new InvalidOperationException();
            }

            command.Execute();
        }

        private XamlMemberBase GetMember(XamlMemberBase member)
        {
            if (IsLevelOneAndThereIsRootInstance)
            {
                var xamlMember = WiringContext.GetType(rootInstanceType).GetMember(member.Name);
                return rootInstanceType == null ? member : xamlMember;
            }

            return member;
        }

        private bool IsLevelOneAndThereIsRootInstance => StateCommuter.Level == 1 && rootInstance != null;

        public void OverrideInstance(object instance)
        {
            StateCommuter.RaiseLevel();
            StateCommuter.Instance = instance;
        }
    }
}