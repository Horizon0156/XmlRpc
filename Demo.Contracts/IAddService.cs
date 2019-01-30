using Horizon.XmlRpc.Core;

namespace Demo.Contracts
{
    /// <summary>
    ///     XML-RPC Service that adds two numbers
    /// </summary>
    public interface IAddService
    {
        /// <summary>
        ///     Adds the numbers.
        /// </summary>
        /// <returns> Sum of the numbers .</returns>
        /// <param name="numberA"> Summand a.</param>
        /// <param name="numberB"> Summand b.</param>
        [XmlRpcMethod("Demo.addNumbers")]
        int AddNumbers(int numberA, int numberB);
    }
}
