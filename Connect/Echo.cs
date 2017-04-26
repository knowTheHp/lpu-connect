using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System.Diagnostics;

namespace Connect {
    [HubName("echo")]
    public class Echo : Hub {
        public void Hello(string message) {
            //Clients.All.hello();
            Trace.WriteLine(message);
        }
    }
}