using CookComputing.XmlRpc;

namespace CcuDemo
{
    public class DeviceDescription
    {
        [XmlRpcMember("ADDRESS")]
        public string Address
        {
            get;
            set;
        }

        [XmlRpcMissingMapping(MappingAction.Ignore), XmlRpcMember("CHILDREN")]
        public string[] Children
        {
            get;
            set;
        }

        [XmlRpcMissingMapping(MappingAction.Ignore), XmlRpcMember("DIRECTION")]
        public int Direction
        {
            get;
            set;
        }

        [XmlRpcMissingMapping(MappingAction.Ignore), XmlRpcMember("FIRMWARE")]
        public string Firmware
        {
            get;
            set;
        }

        [XmlRpcMember("PARENT")]
        public string Parent
        {
            get;
            set;
        }

        [XmlRpcMissingMapping(MappingAction.Ignore), XmlRpcMember("PARENT_TYPE")]
        public string ParentType
        {
            get;
            set;
        }

        [XmlRpcMember("TYPE")]
        public string Type
        {
            get;
            set;
        }

        public string GetCategory()
        {
            switch (this.Direction)
            {
                case 0:
                    return "nicht verknüpfbar";

                case 1:
                    return "Sender (Sensor)";

                case 2:
                    return "Empfänger (Aktor)";

                default:
                    return string.Empty;
            }
        }

        public bool IsChannel()
        {
            return !this.IsDevice();
        }

        public bool IsDevice()
        {
            return string.IsNullOrEmpty(this.Parent);
        }

        public override string ToString()
        {
            return this.Address;
        }
    }
}