using System;

namespace Horizon.XmlRpc.Client
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
    public class XmlRpcUrlAttribute : Attribute
    {
        public XmlRpcUrlAttribute(string UriString)
        {
            this.uri = UriString;
        }
        public string Uri
        {
            get
            { return uri; }
        }
        public override string ToString()
        {
            string value = "Uri : " + uri;
            return value;
        }
        private string uri;
    }
}
