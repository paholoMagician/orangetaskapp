using Microsoft.AspNetCore.SignalR;
using orangebackend6.Models;

namespace orange.Controllers
{
    public class tablaHub : Hub
    {
        public async Task SendTableMessage(Mensaje message)
        {
            await Clients.All.SendAsync("TableMessage", message);
        }
    }

}


