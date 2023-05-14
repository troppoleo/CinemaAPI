using Microsoft.AspNetCore.SignalR;

namespace CinemaAPI.Hubs
{
    public class CinemaHub : Hub
    {
        public void SendSeatRoom(string user, string message)
        {
            Clients.All.SendAsync("ReceiveSeatRoom", user, message);
        }

    }
}
