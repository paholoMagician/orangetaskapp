using Microsoft.AspNetCore.SignalR;
using orangebackend6.Models;

namespace orangebackend6
{
    public class ComentariosHub: Hub
    {
        public async Task SendComentarios( Comentario comentario ) {
            await Clients.All.SendAsync("SendComentarios", comentario);
        }
    }

}
