using Connect.Models;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System.Diagnostics;
using System.Linq;
using System.Web.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;

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

        public override Task OnConnected() {
            //log user conection
            Trace.WriteLine("Here I am " + Context.ConnectionId);

            //get logggedIn userId
            User loggedInUser = lpuContext.Users.Where(user => user.Username.Equals(Context.User.Identity.Name)).FirstOrDefault();
            long userId = loggedInUser.UserId;

            //get the connection Id
            string connId = Context.ConnectionId;

            //add to Online
            if (!lpuContext.Onlines.Any(online => online.OnlineId == userId)) {
                Online onlineDTO = new Online();
                onlineDTO.OnlineId = userId;
                onlineDTO.UserId = userId;
                onlineDTO.ConnectionId = connId;
                lpuContext.Onlines.Add(onlineDTO);
                lpuContext.SaveChanges();
            }
            //get all online id
            List<long> onlineIds = lpuContext.Onlines.ToArray().Select(x => x.OnlineId).ToList();

            //get friend list
            List<long> receiverIds = lpuContext.Connections.Where(x => x.User_Sender == userId && x.Active == true).ToArray().Select(x => x.User_Receiver).ToList();
            List<long> senderIds = lpuContext.Connections.Where(x => x.User_Receiver == userId && x.Active == true).ToArray().Select(x => x.User_Sender).ToList();
            List<long> allFriendsIds = receiverIds.Concat(senderIds).ToList();

            //get final set of id
            List<long> resultList = onlineIds.Where((userid) => allFriendsIds.Contains(userid)).ToList();

            //create a dic of friend ids and username
            Dictionary<long, string> dictOfFriends = new Dictionary<long, string>();
            foreach (var id in resultList) {
                var users = lpuContext.Users.Find(id);
                string friend = users.Username;
                if (!dictOfFriends.ContainsKey(id)) {
                    dictOfFriends.Add(id, friend);
                }
            }

            var transformed = from key in dictOfFriends.Keys
                              select new { id = key, friend = dictOfFriends[key] };
            string json = JsonConvert.SerializeObject(transformed);

            //set clients
            var clients = Clients.Caller;

            //Call js function
            clients.getonlinefriends(Context.User.Identity.Name, json);

            //Update chat
            UpdateChat();

            //return
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled) {
            // Log
            Trace.WriteLine("gone - " + Context.ConnectionId + " " + Context.User.Identity.Name);

            // Get user id
            User userDTO = lpuContext.Users.Where(x => x.Username.Equals(Context.User.Identity.Name)).FirstOrDefault();
            long userId = userDTO.UserId;

            // Remove from db
            if (lpuContext.Onlines.Any(x => x.OnlineId == userId)) {
                Online online = lpuContext.Onlines.Find(userId);
                lpuContext.Onlines.Remove(online);
                lpuContext.SaveChanges();
            }

            // Update chat
            UpdateChat();

            // Return
            return base.OnDisconnected(stopCalled);
        }

        //to increment the friends online
        public void UpdateChat() {
            // Get all online ids
            List<long> onlineIds = lpuContext.Onlines.ToArray().Select(x => x.OnlineId).ToList();

            // Loop thru onlineids and get friends
            foreach (long userId in onlineIds) {
                // Get username
                User user = lpuContext.Users.Find(userId);
                string username = user.Username;

                // Get all friend ids
                List<long> receiverIds = lpuContext.Connections.Where(x => x.User_Sender == userId && x.Active == true).ToArray().Select(x => x.User_Receiver).ToList();
                List<long> senderIds = lpuContext.Connections.Where(x => x.User_Receiver == userId && x.Active == true).ToArray().Select(x => x.User_Sender).ToList();
                List<long> allFriendsIds = receiverIds.Concat(senderIds).ToList();

                // Get final set of ids
                List<long> resultList = allFriendsIds.Where((ids) => allFriendsIds.Contains(ids)).ToList();

                // Create a dict of friend ids and usernames
                Dictionary<long, string> dictFriends = new Dictionary<long, string>();
                foreach (var id in resultList) {
                    var users = lpuContext.Users.Find(id);
                    string friend = users.Username;
                    if (!dictFriends.ContainsKey(id)) {
                        dictFriends.Add(id, friend);
                    }
                }
                var transformed = from key in dictFriends.Keys
                                  select new { id = key, friend = dictFriends[key] };
                string json = JsonConvert.SerializeObject(transformed);

                // Set clients
                var clients = Clients.All;

                // Call js function
                clients.updatechat(username, json);
            }
        }

        public void SendChat(int friendId, string friendUsername, string message) {
            // Get user id
            User userDTO = lpuContext.Users.Where(x => x.Username.Equals(Context.User.Identity.Name)).FirstOrDefault();
            long userId = userDTO.UserId;

            // Set clients
            var clients = Clients.All;

            // Call js function
            clients.sendchat(userId, Context.User.Identity.Name, friendId, friendUsername, message);
        }
    }
}