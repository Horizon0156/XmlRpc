using System;

namespace Horizon.XmlRpc.Client
{
    [AttributeUsage(AttributeTargets.Method)]
    public class XmlRpcEndAttribute : Attribute
    {
        public XmlRpcEndAttribute()
        {
        }

        public XmlRpcEndAttribute(string method)
        {
            this.method = method;
        }

        public string Method
        {
            get
            { return method; }
        }

        public bool IntrospectionMethod
        {
            get { return introspectionMethod; }
            set { introspectionMethod = value; }
        }

        public override string ToString()
        {
            string value = "Method : " + method;
            return value;
        }

        public string Description = "";
        public bool Hidden = false;
        private string method = "";
        private bool introspectionMethod = false;
    }
}

