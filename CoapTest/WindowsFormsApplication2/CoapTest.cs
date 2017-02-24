using CoAP.Server.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication2
{
    class CoapTest : Resource
    {
        public CoapTest(String name) : base(name)
        {
            Attributes.Title = "GET a friendly greeting!";
        }
        protected override void DoGet(CoapExchange exchange)
        {
            exchange.Respond("Hello World!  DoGet");
        }
        protected override void DoPost(CoapExchange exchange)
        {
            Console.WriteLine("  服务端接收到的数据  : " + exchange.Request.PayloadString);
            exchange.Respond("Hello World!   DoPost");
        }
        // override this method to handle PUT requests

        protected override void DoPut(CoapExchange exchange)
        {
            exchange.Respond("Hello World!  DoPut ");
        }
        // override this method to handle DELETE requests
        protected override void DoDelete(CoapExchange exchange)
        {
            exchange.Respond("Hello World! DoDelete ");

        }
    }
}
