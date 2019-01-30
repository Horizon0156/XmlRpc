using System;

namespace Horizon.XmlRpc.Core
{
    public class XmlRpcParameterInfo
    {
        public XmlRpcParameterInfo()
        {
        }

        public String Doc
        {
            get { return doc; }
            set { doc = value; }
        }

        public bool IsParams
        {
            get { return isparams; }
            set { isparams = value; }
        }

        public String Name
        {
            get { return name; }
            set
            {
                name = value;
                if (xmlRpcName == "")
                    xmlRpcName = name;
            }
        }

        public String XmlRpcName
        {
            get { return xmlRpcName; }
            set { xmlRpcName = value; }
        }

        public Type Type
        {
            get { return type; }
            set { type = value; }
        }

        public string XmlRpcType
        {
            get { return xmlRpcType; }
            set { xmlRpcType = value; }
        }

        string doc;
        string name;
        Type type;
        string xmlRpcName;
        string xmlRpcType;
        bool isparams;
    }
}