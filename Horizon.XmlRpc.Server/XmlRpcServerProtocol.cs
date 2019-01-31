using System;
using System.IO;
using System.Reflection;
using System.Text;
using Horizon.XmlRpc.Core;

namespace Horizon.XmlRpc.Server
{
    public class XmlRpcServerProtocol : SystemMethodsBase
    {
        public Stream Invoke(Stream requestStream)
        {
            try
            {
                XmlRpcSerializer serializer = new XmlRpcSerializer();
                Type type = this.GetType();
                XmlRpcServiceAttribute serviceAttr = (XmlRpcServiceAttribute)
                  Attribute.GetCustomAttribute(this.GetType(),
                  typeof(XmlRpcServiceAttribute));
                if (serviceAttr != null)
                {
                    if (serviceAttr.XmlEncoding != null)
                        serializer.XmlEncoding = Encoding.GetEncoding(serviceAttr.XmlEncoding);
                    serializer.UseIntTag = serviceAttr.UseIntTag;
                    serializer.UseStringTag = serviceAttr.UseStringTag;
                    serializer.UseIndentation = serviceAttr.UseIndentation;
                    serializer.Indentation = serviceAttr.Indentation;
                }
                XmlRpcRequest xmlRpcReq
                  = serializer.DeserializeRequest(requestStream, this.GetType());
                XmlRpcResponse xmlRpcResp = Invoke(xmlRpcReq);
                Stream responseStream = new MemoryStream();
                serializer.SerializeResponse(responseStream, xmlRpcResp);
                responseStream.Seek(0, SeekOrigin.Begin);
                return responseStream;
            }
            catch (Exception ex)
            {
                XmlRpcFaultException fex;
                if (ex is XmlRpcException)
                    fex = new XmlRpcFaultException(0, ((XmlRpcException)ex).Message);
                else if (ex is XmlRpcFaultException)
                    fex = (XmlRpcFaultException)ex;
                else
                    fex = new XmlRpcFaultException(0, ex.Message);
                XmlRpcSerializer serializer = new XmlRpcSerializer();
                Stream responseStream = new MemoryStream();
                serializer.SerializeFaultResponse(responseStream, fex);
                responseStream.Seek(0, SeekOrigin.Begin);
                return responseStream;
            }
        }

        public XmlRpcResponse Invoke(XmlRpcRequest request)
        {
            MethodInfo mi = null;
            if (request.mi != null)
            {
                mi = request.mi;
            }
            else
            {
                mi = this.GetType().GetMethod(request.method);
            }
            // exceptions thrown during an MethodInfo.Invoke call are
            // package as inner of 
            Object reto;
            try
            {
                reto = mi.Invoke(this, request.args);
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    throw ex.InnerException;
                throw ex;
            }
            XmlRpcResponse response = new XmlRpcResponse(reto);
            return response;
        }

        bool IsVisibleXmlRpcMethod(MethodInfo mi)
        {
            bool ret = false;
            Attribute attr = Attribute.GetCustomAttribute(mi,
              typeof(XmlRpcMethodAttribute));
            if (attr != null)
            {
                XmlRpcMethodAttribute mattr = (XmlRpcMethodAttribute)attr;
                ret = !mattr.Hidden;
            }
            return ret;
        }
    }
}
