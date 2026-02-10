using Microsoft.AspNetCore.SignalR;

namespace RegGoodMd5Server.Hubs
{
    public class DataHub : Hub
    {
        public async Task SendMessage(string message)
        {
            await Clients.All.SendAsync("receiveMessage", message);
        }
    }
}
