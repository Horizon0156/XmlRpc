using CookComputing.XmlRpc;

namespace CcuDemo
{
    public interface IHomeMaticProxy : IXmlRpcProxy
    {
        [XmlRpcMethod("listDevices")]
        DeviceDescription[] ListDevices();

        [XmlRpcMethod("setValue")]
        void SetValue(string address, string valueKey, object value);

        [XmlRpcMethod("getValue")]
        object GetValue(string address, string valueKey);
    }
}