using System.Net;

namespace Horizon.XmlRpc.ServerDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            var listener = new HttpListener();
            listener.Prefixes.Add("http://127.0.0.1:5678/");
            listener.Start();

            System.Console.WriteLine("Started Demo service. Press CTRL+C to exit..." );
            while (true)
            {
                var context = listener.GetContext();
                var service = new AddService();
                service.ProcessRequest(context);
            }
        }
    }
}
