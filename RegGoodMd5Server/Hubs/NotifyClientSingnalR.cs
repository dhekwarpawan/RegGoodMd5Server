using Microsoft.AspNetCore.SignalR;
using RegGoodMd5Server.Hubs;
using System.Threading;

namespace RegGoodMd5Server.Hubs
{
    public class NotifyClientSingnalR
    {
        private readonly IHubContext<DataHub> _hubContext;

        public NotifyClientSingnalR(IHubContext<DataHub> hubContext)
        {
            _hubContext = hubContext;
        }
        public async Task NotifyClientsAsync(string entity, string action, int id)
        {
            try
            {
                await _hubContext.Clients.All.SendAsync("dataChanged", new
                {
                    entity,
                    action,
                    id
                });
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
