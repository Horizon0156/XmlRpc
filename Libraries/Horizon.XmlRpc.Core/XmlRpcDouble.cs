
namespace Horizon.XmlRpc.Core
{
    public class XmlRpcDouble
    {
        private double _value;

        public XmlRpcDouble()
        {
            this._value = 0;
        }

        public XmlRpcDouble(
          double val)
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
            if (o == null || !(o is XmlRpcDouble))
                return false;
            XmlRpcDouble dbl = o as XmlRpcDouble;
            return dbl._value == _value;
        }

        public static bool operator ==(
          XmlRpcDouble xi,
          XmlRpcDouble xj)
        {
            if (((object)xi) == null && ((object)xj) == null)
                return true;
            else if (((object)xi) == null || ((object)xj) == null)
                return false;
            else
                return xi._value == xj._value;
        }

        public static bool operator !=(
          XmlRpcDouble xi,
          XmlRpcDouble xj)
        {
            return !(xi == xj);
        }

        public static implicit operator double(XmlRpcDouble x)
        {
            return x._value;
        }

        public static implicit operator XmlRpcDouble(double x)
        {
            return new XmlRpcDouble(x);
        }
    }
}