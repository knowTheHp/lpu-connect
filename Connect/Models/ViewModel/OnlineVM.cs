using System.ComponentModel.DataAnnotations;
namespace Connect.Models.ViewModel {
    public class OnlineVM {
        public OnlineVM() {

        }

        public OnlineVM(Online online) {
            OnlineId = online.OnlineId;
            UserId = online.UserId;
            ConnectionId = online.ConnectionId;
        }
        [Key]
        public long OnlineId { get; set; }
        public long? UserId { get; set; }
        public string ConnectionId { get; set; }
    }
}