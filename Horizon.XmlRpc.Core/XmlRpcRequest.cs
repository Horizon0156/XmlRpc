using System;
using System.Reflection;

namespace Horizon.XmlRpc.Core
{
    public class XmlRpcRequest
    {
        public XmlRpcRequest()
        {
        }

        public XmlRpcRequest(string methodName, object[] parameters, MethodInfo methodInfo)
        {
            method = methodName;
            args = parameters;
            mi = methodInfo;
        }

        public XmlRpcRequest(string methodName, object[] parameters,
          MethodInfo methodInfo, string XmlRpcMethod, Guid proxyGuid)
        {
            method = methodName;
            args = parameters;
            mi = methodInfo;
            xmlRpcMethod = XmlRpcMethod;
            proxyId = proxyGuid;
        }

        public XmlRpcRequest(string methodName, Object[] parameters)
        {
            method = methodName;
            args = parameters;
        }

        public String method = null;
        public Object[] args = null;
        public MethodInfo mi = null;
        public Guid proxyId;
        static int _created;
        public int number = System.Threading.Interlocked.Increment(ref _created);
        public String xmlRpcMethod = null;
    }
}