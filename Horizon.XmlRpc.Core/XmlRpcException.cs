using System;

namespace Horizon.XmlRpc.Core
{
    public class XmlRpcException : ApplicationException
    {
        public XmlRpcException() { }

        public XmlRpcException(string msg)
          : base(msg) { }

        public XmlRpcException(string msg, Exception innerEx)
          : base(msg, innerEx) { }
    }

    public class XmlRpcUnsupportedTypeException : XmlRpcException
    {
        Type _unsupportedType;

        public XmlRpcUnsupportedTypeException(Type t)
          : base(string.Format("Unable to map type {0} onto XML-RPC type", t))
        {
            _unsupportedType = t;
        }

        public XmlRpcUnsupportedTypeException(Type t, string msg)
          : base(msg)
        {
            _unsupportedType = t;
        }

        public XmlRpcUnsupportedTypeException(Type t, string msg, Exception innerEx)
          : base(msg, innerEx)
        {
            _unsupportedType = t;
        }

        public Type UnsupportedType { get { return _unsupportedType; } }
    }

    public class XmlRpcUnexpectedTypeException : XmlRpcException
    {
        public XmlRpcUnexpectedTypeException() { }

        public XmlRpcUnexpectedTypeException(string msg)
          : base(msg) { }

        public XmlRpcUnexpectedTypeException(string msg, Exception innerEx)
          : base(msg, innerEx) { }
    }

    public class XmlRpcIllFormedXmlException : XmlRpcException
    {
        public XmlRpcIllFormedXmlException() { }

        public XmlRpcIllFormedXmlException(string msg)
          : base(msg) { }

        public XmlRpcIllFormedXmlException(string msg, Exception innerEx)
          : base(msg, innerEx) { }
    }

    public class XmlRpcUnsupportedMethodException : XmlRpcException
    {
        public XmlRpcUnsupportedMethodException() { }

        public XmlRpcUnsupportedMethodException(string msg)
          : base(msg) { }

        public XmlRpcUnsupportedMethodException(string msg, Exception innerEx)
          : base(msg, innerEx) { }
    }

    public class XmlRpcInvalidParametersException : XmlRpcException
    {
        public XmlRpcInvalidParametersException() { }

        public XmlRpcInvalidParametersException(string msg)
          : base(msg) { }

        public XmlRpcInvalidParametersException(string msg, Exception innerEx)
          : base(msg, innerEx) { }
    }

    public class XmlRpcNonRegularArrayException : XmlRpcException
    {
        public XmlRpcNonRegularArrayException() { }

        public XmlRpcNonRegularArrayException(string msg)
          : base(msg) { }

        public XmlRpcNonRegularArrayException(string msg, Exception innerEx)
          : base(msg, innerEx) { }
    }

    public class XmlRpcInvalidXmlRpcException : XmlRpcException
    {
        public XmlRpcInvalidXmlRpcException() { }

        public XmlRpcInvalidXmlRpcException(string msg)
          : base(msg) { }

        public XmlRpcInvalidXmlRpcException(string msg, Exception innerEx)
          : base(msg, innerEx) { }
    }

    public class XmlRpcMethodAttributeException : XmlRpcException
    {
        public XmlRpcMethodAttributeException() { }

        public XmlRpcMethodAttributeException(string msg)
          : base(msg) { }

        public XmlRpcMethodAttributeException(string msg, Exception innerEx)
          : base(msg, innerEx) { }
    }

    public class XmlRpcTypeMismatchException : XmlRpcException
    {
        public XmlRpcTypeMismatchException() { }

        public XmlRpcTypeMismatchException(string msg)
          : base(msg) { }

        public XmlRpcTypeMismatchException(string msg, Exception innerEx)
          : base(msg, innerEx) { }
    }

    public class XmlRpcNullReferenceException : XmlRpcException
    {
        public XmlRpcNullReferenceException() { }

        public XmlRpcNullReferenceException(string msg)
          : base(msg) { }

        public XmlRpcNullReferenceException(string msg, Exception innerEx)
          : base(msg, innerEx) { }
    }

    public class XmlRpcServerException : XmlRpcException
    {
        public XmlRpcServerException() { }

        public XmlRpcServerException(string msg)
          : base(msg) { }

        public XmlRpcServerException(string msg, Exception innerEx)
          : base(msg, innerEx) { }
    }

    public class XmlRpcInvalidReturnType : XmlRpcException
    {
        public XmlRpcInvalidReturnType() { }

        public XmlRpcInvalidReturnType(string msg)
          : base(msg) { }

        public XmlRpcInvalidReturnType(string msg, Exception innerEx)
          : base(msg, innerEx) { }
    }

    public class XmlRpcMappingSerializeException : XmlRpcException
    {
        public XmlRpcMappingSerializeException() { }

        public XmlRpcMappingSerializeException(string msg)
          : base(msg) { }

        public XmlRpcMappingSerializeException(string msg, Exception innerEx)
          : base(msg, innerEx) { }
    }

    public class XmlRpcNullParameterException : XmlRpcException
    {
        public XmlRpcNullParameterException() { }

        public XmlRpcNullParameterException(string msg)
          : base(msg) { }

        public XmlRpcNullParameterException(string msg, Exception innerEx)
          : base(msg, innerEx) { }
    }

    public class XmlRpcMissingUrl : XmlRpcException
    {
        public XmlRpcMissingUrl() { }

        public XmlRpcMissingUrl(string msg)
          : base(msg) { }
    }

    public class XmlRpcDupXmlRpcMethodNames : XmlRpcException
    {
        public XmlRpcDupXmlRpcMethodNames() { }

        public XmlRpcDupXmlRpcMethodNames(string msg)
          : base(msg) { }
    }

    public class XmlRpcNonSerializedMember : XmlRpcException
    {
        public XmlRpcNonSerializedMember() { }

        public XmlRpcNonSerializedMember(string msg)
          : base(msg) { }

        public XmlRpcNonSerializedMember(string msg, Exception innerEx)
          : base(msg, innerEx) { }
    }

}
