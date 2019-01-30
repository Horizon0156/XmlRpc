using System;
using System.Text;

namespace Horizon.XmlRpc.Core
{
    [Flags]
    public enum XmlRpcNonStandard
    {
        None = 0x00,
        AllowStringFaultCode = 0x01,
        AllowNonStandardDateTime = 0x02,
        IgnoreDuplicateMembers = 0x4,
        MapZerosDateTimeToMinValue = 0x8,
        MapEmptyDateTimeToMinValue = 0x10,
        AllowInvalidHTTPContent = 0x20,
        All = 0x7fff,
    }
}
