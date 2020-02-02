using System;
using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace Vs.Graph.Core.Data
{
    public class Attribute : IAttribute, ISerialize
    {
        private AttributeType _type;
        private string _name;

        public AttributeType Type => _type;

        public string Name => _name;

        public Attribute() { }

        public Attribute(string name, AttributeType type)
        {
            _name = name;
            _type = type;
        }
        private class DeserializeTemplate
        {
            public string Name;
            public AttributeType Type;
        }

        public void Read(IParser parser, Type expectedType, ObjectDeserializer nestedObjectDeserializer)
        {
            var o = (DeserializeTemplate)nestedObjectDeserializer(typeof(DeserializeTemplate));
            _name = o.Name;
            _type = o.Type;
        }

        public void Write(IEmitter emitter, ObjectSerializer nestedObjectSerializer)
        {
            nestedObjectSerializer(new { Name, Type});
        }
    }
}
