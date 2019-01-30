using System;

namespace Horizon.XmlRpc.Core
{
    public class XmlRpcDateTime
    {
        private DateTime _value;

        public XmlRpcDateTime()
        {
            this._value = new DateTime();
        }

        public XmlRpcDateTime(DateTime val)
        {
            this._value = val;
        }

        public override string ToString()
        {
            return _value.ToString();
        }

        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }

        public override bool Equals(
          object o)
        {
            if (o == null || !(o is XmlRpcDateTime))
                return false;
            XmlRpcDateTime dbl = o as XmlRpcDateTime;
            return (dbl._value == _value);
        }

        public static bool operator ==(
          XmlRpcDateTime xi,
          XmlRpcDateTime xj)
        {
            if (((object)xi) == null && ((object)xj) == null)
                return true;
            else if (((object)xi) == null || ((object)xj) == null)
                return false;
            else
                return xi._value == xj._value;
        }

        public static bool operator !=(
          XmlRpcDateTime xi,
          XmlRpcDateTime xj)
        {
            return !(xi == xj);
        }

        public static implicit operator DateTime(XmlRpcDateTime x)
        {
            return x._value;
        }

        public static implicit operator XmlRpcDateTime(DateTime x)
        {
            return new XmlRpcDateTime(x);
        }
    }
}