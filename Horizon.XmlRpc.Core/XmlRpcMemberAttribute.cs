using System;

namespace Horizon.XmlRpc.Core
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class XmlRpcMemberAttribute : Attribute
    {
        public XmlRpcMemberAttribute()
        {
        }

        public XmlRpcMemberAttribute(string member)
        {
            _member = member;
        }

        public string Member
        {
            get
            { return _member; }
            set
            { _member = value; }
        }

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        public override string ToString()
        {
            string value = "Member : " + _member;
            return value;
        }

        private string _member = "";
        private string _description = "";
    }
}