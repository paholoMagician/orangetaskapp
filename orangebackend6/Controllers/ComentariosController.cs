using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using orange.Controllers;
using orangebackend6.Models;
using System.Data;

namespace orangebackend6.Controllers
{
    [Route("api/Comentarios")]
    [ApiController]
    public class ComentariosController : ControllerBase
    {

        private readonly orangeContext _context;
        private readonly IHubContext<ComentariosHub> _comentariosHubContext;

        public ComentariosController (  orangeContext context, IHubContext<ComentariosHub> comentariosHubContext)
        {
            _context = context;
            _comentariosHubContext = comentariosHubContext;
        }

        [HttpPost]
        [Route("guardarComentarios")]
        public async Task<IActionResult> guardarComentarios([FromBody] Comentario model)
        {

            if (ModelState.IsValid)
            {
                _context.Comentario.Add(model);
                if (await _context.SaveChangesAsync() > 0)
                {
                    await _comentariosHubContext.Clients.All.SendAsync("SendComentarios", model);
                    return Ok(model);
                }

                else
                {
                    return BadRequest("Datos incorrectos");
                }
            }
            else
            {
                return BadRequest("ERROR");
            }
        }

        [HttpGet("ObtenerComentarios/{codmsj}")]
        public async Task<IActionResult> ObtenerComentarios([FromRoute] string codmsj)
        {

            string Sentencia = "exec ObtenerComentarios @cmsj";

            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(Sentencia, connection))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.SelectCommand.CommandType = CommandType.Text;
                    adapter.SelectCommand.Parameters.Add(new SqlParameter("@cmsj", codmsj));
                    adapter.Fill(dt);
                }
            }

            if (dt == null)
            {
                return NotFound("No se ha podido crear...");
            }

            return Ok(dt);

        }

        

        [HttpGet("ActualizarEstadoComentarios/{idcomen}/{idest}/{coduser}")]
        public async Task<IActionResult> ActualizarEstadoComentarios([FromRoute] string idcomen, [FromRoute] string idest, [FromRoute] string coduser)
        {

            string Sentencia = "exec CambiarEstadoComments @idcoments, @estado, @ccuser";

            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(Sentencia, connection))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.SelectCommand.CommandType = CommandType.Text;
                    adapter.SelectCommand.Parameters.Add(new SqlParameter("@idcoments", idcomen));
                    adapter.SelectCommand.Parameters.Add(new SqlParameter("@estado",    idest));
                    adapter.SelectCommand.Parameters.Add(new SqlParameter("@ccuser",    coduser));
                    adapter.Fill(dt);
                }
            }

            if (dt == null)
            {
                return NotFound("No se ha podido crear...");
            }

            return Ok(dt);


        }

    }
}
