using System;

namespace Horizon.XmlRpc.Core
{
    [AttributeUsage(AttributeTargets.Method)]
    public class XmlRpcMethodAttribute : Attribute
    {
        public XmlRpcMethodAttribute()
        {
        }

        public XmlRpcMethodAttribute(string method)
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

        public bool StructParams
        {
            get { return structParams; }
            set { structParams = value; }
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
        private bool structParams = false;
    }
}