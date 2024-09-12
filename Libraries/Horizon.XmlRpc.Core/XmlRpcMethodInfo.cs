using System;
using System.Reflection;

namespace Horizon.XmlRpc.Core
{
    public class XmlRpcMethodInfo : IComparable
    {
        public XmlRpcMethodInfo()
        {
        }

        public bool IsHidden
        {
            get { return isHidden; }
            set { isHidden = value; }
        }

        public String Doc
        {
            get { return doc; }
            set { doc = value; }
        }

        public MethodInfo MethodInfo
        {
            get { return mi; }
            set { mi = value; }
        }

        public String MiName
        {
            get { return name; }
            set { name = value; }
        }

        public XmlRpcParameterInfo[] Parameters
        {
            get { return paramInfos; }
            set { paramInfos = value; }
        }

        public Type ReturnType
        {
            get { return returnType; }
            set { returnType = value; }
        }

        public string ReturnXmlRpcType
        {
            get { return returnXmlRpcType; }
            set { returnXmlRpcType = value; }
        }

        public String ReturnDoc
        {
            get { return returnDoc; }
            set { returnDoc = value; }
        }

        public String XmlRpcName
        {
            get { return xmlRpcName; }
            set { xmlRpcName = value; }
        }

        public int CompareTo(object obj)
        {
            XmlRpcMethodInfo xmi = (XmlRpcMethodInfo)obj;
            return this.xmlRpcName.CompareTo(xmi.xmlRpcName);
        }

        MethodInfo mi;
        bool isHidden;
        string doc = "";
        string name = "";
        string xmlRpcName = "";
        string returnDoc = "";
        Type returnType;
        string returnXmlRpcType;
        XmlRpcParameterInfo[] paramInfos;
    }
}