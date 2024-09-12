using System;
using System.Runtime.Serialization;

namespace Horizon.XmlRpc.Core
{
    [Serializable]
    public class XmlRpcFaultException : ApplicationException
    {

        public XmlRpcFaultException(int TheCode, string TheString)
          : base("Server returned a fault exception: [" + TheCode.ToString() +
                  "] " + TheString)
        {
            m_faultCode = TheCode;
            m_faultString = TheString;
        }

        protected XmlRpcFaultException(
          SerializationInfo info,
          StreamingContext context)
          : base(info, context)
        {
            m_faultCode = (int)info.GetValue("m_faultCode", typeof(int));
            m_faultString = (String)info.GetValue("m_faultString", typeof(string));
        }

        public int FaultCode
        {
            get { return m_faultCode; }
        }

        public string FaultString
        {
            get { return m_faultString; }
        }

        // public methods
        //
        public override void GetObjectData(
          SerializationInfo info,
          StreamingContext context)
        {
            info.AddValue("m_faultCode", m_faultCode);
            info.AddValue("m_faultString", m_faultString);
            base.GetObjectData(info, context);
        }

        // data
        //
        int m_faultCode;
        string m_faultString;
    }
}
