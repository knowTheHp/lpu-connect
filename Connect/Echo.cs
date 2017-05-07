using Connect.Models;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System.Diagnostics;
using System.Linq;

namespace Connect {
    [HubName("echo")]
    public class Echo : Hub {
        LpuContext lpuContext = new LpuContext();
        public void Hello(string message) {
            //Clients.All.hello();
            Trace.WriteLine(message);
        }

        public void Notify(string friend) {
            //get friendId
            User friendData = lpuContext.Users.Where(user => user.Username.Equals(friend)).FirstOrDefault();
            long friendId = friendData.UserId;

            //get friend count
            int friendCount = lpuContext.Connections.Count(user => user.User_Receiver == friendId && user.Active == false);

            //set client
            var clients = Clients.Others;

            //call JS function
            clients.frnotify(friend, friendCount);
        }

        public void RequestCount() {
            User userData = lpuContext.Users.Where(user => user.Username.Equals(Context.User.Identity.Name)).FirstOrDefault();
            long userId = userData.UserId;

            //get request count
            var requestCount = lpuContext.Connections.Count(request => request.User_Receiver == userId && request.Active == false);

            var clients = Clients.Caller;

            clients.requestCount(Context.User.Identity.Name, requestCount);
        }
    }
}