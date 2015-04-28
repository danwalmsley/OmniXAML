﻿namespace OmniXaml.Assembler
{
    using System;
    using Glass;
    using NodeWriters;

    public class ObjectAssembler : IObjectAssembler
    {
        private readonly EndMemberWriter endMemberWriter;
        private readonly EndObjectWriter endObjectWriter;
        private readonly GetObjectWriter getObjectWriter;
        private readonly NamespaceWriter namespaceWriter;
        private readonly StartMemberWriter startMemberWriter;
        private readonly StartObjectWriter startObjectWriter;
        private readonly TypeOperations typeOperations;
        private readonly ValueWriter valueWriter;

        public ObjectAssembler(WiringContext wiringContext)
        {
            typeOperations = new TypeOperations(wiringContext.TypeContext.TypeFactory);
            startMemberWriter = new StartMemberWriter(this);
            getObjectWriter = new GetObjectWriter(this);
            startObjectWriter = new StartObjectWriter(this);
            valueWriter = new ValueWriter(this);
            namespaceWriter = new NamespaceWriter();
            endMemberWriter = new EndMemberWriter(this, wiringContext);
            endObjectWriter = new EndObjectWriter(this, wiringContext.TypeContext);
        }

        internal StateBag Bag { get; } = new StateBag();

        public object Result { get; internal set; }

        public void WriteNode(IXamlReader reader)
        {
            Guard.ThrowIfNull(reader, nameof(reader));

            switch (reader.NodeType)
            {
                case XamlNodeType.None:
                    break;
                case XamlNodeType.StartObject:
                    startObjectWriter.WriteStartObject(reader.Type);
                    break;
                case XamlNodeType.EndObject:
                    endObjectWriter.WriteEndObject();
                    break;
                case XamlNodeType.StartMember:
                    startMemberWriter.WriteStartMember(reader.Member);
                    break;
                case XamlNodeType.EndMember:
                    endMemberWriter.WriteEndMember();
                    break;
                case XamlNodeType.Value:
                    valueWriter.WriteValue(reader.Value);
                    break;
                case XamlNodeType.GetObject:
                    getObjectWriter.WriteGetObject();
                    break;
                case XamlNodeType.NamespaceDeclaration:
                    namespaceWriter.WriteNamespace(reader.Namespace);
                    break;
                default:
                    throw new InvalidOperationException($"Cannot handle this kind of node type: {reader.NodeType}");
            }
        }

        public void SetUnfinishedResult()
        {
            Result = null;
        }

        internal void PrepareNewInstanceBecauseWeWantToConfigureIt(StateBag bag)
        {
            var newInstance = typeOperations.Create(bag.Current.Type);
            bag.Current.Instance = newInstance;
        }
    }
}