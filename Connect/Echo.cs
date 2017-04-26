using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace Connect {
    public class Echo : Hub {
        public void Hello() {
            Clients.All.hello();
        }
    }
}