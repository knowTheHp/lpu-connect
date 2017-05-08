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
        //A method to send friend request notification
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

        //A method to display total number of requests
        public void RequestCount() {
            User userData = lpuContext.Users.Where(user => user.Username.Equals(Context.User.Identity.Name)).FirstOrDefault();
            long userId = userData.UserId;

            //get request count
            int requestCount = lpuContext.Connections.Count(request => request.User_Receiver == userId && request.Active == false);

            var clients = Clients.Caller;
            //call JS function
            clients.requestCount(Context.User.Identity.Name, requestCount);
        }

        //A method to increment and display the total connected user
        public void ConnectionCount(long friendId) {
            //get logged-in userId
            User loggedInUserData = lpuContext.Users.Where(user => user.Username.Equals(Context.User.Identity.Name)).FirstOrDefault();
            long loggedInUserId = loggedInUserData.UserId;

            //get the connected user count for the logged-in user
            int loggedInUserCount = lpuContext.Connections.Count(connectedUser => connectedUser.User_Receiver == loggedInUserId && connectedUser.Active == true || connectedUser.User_Receiver == loggedInUserId && connectedUser.Active == true);

            //get friend username
            User userData = lpuContext.Users.Where(friend => friend.UserId == friendId).FirstOrDefault();
            string username = userData.Username;

            //get friend count for userData
            int userFriendCount = lpuContext.Connections.Count(friendCount => friendCount.User_Receiver == friendId && friendCount.Active == true || friendCount.User_Sender == friendId && friendCount.Active == true);
            var clients = Clients.All;
            //call JS function
            clients.connectionCount(Context.User.Identity.Name, username, loggedInUserCount, userFriendCount);
        }
    }
}