using System;

namespace Horizon.XmlRpc.Core
{
    public enum MappingAction
    {
        Ignore,
        Error
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Struct
       | AttributeTargets.Property | AttributeTargets.Class)]
    public class XmlRpcMissingMappingAttribute : Attribute
    {
        public XmlRpcMissingMappingAttribute()
        {
        }

        public XmlRpcMissingMappingAttribute(MappingAction action)
        {
            _action = action;
        }

        public MappingAction Action
        {
            get
            { return _action; }
        }

        public override string ToString()
        {
            string value = _action.ToString();
            return value;
        }

        private MappingAction _action = MappingAction.Error;
    }
}