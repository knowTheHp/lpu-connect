namespace Connect.Models.ViewModel {
    public class FriendRequestVM {
        public FriendRequestVM() {

        }

        public FriendRequestVM(Connection connect) {
            Sender = connect.User_Sender;
            Receiver = connect.User_Receiver;
            Message = connect.Message;
            Active = connect.Active;
        }
        public long? Sender { get; set; }
        public long? Receiver { get; set; }
        public string Message { get; set; }
        public bool? Active { get; set; }
    }
}