using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using orangebackend6.Models;
using System.Data;

namespace orange.Controllers
{
    [Route("api/Mensajes")]
    [ApiController]
    public class MesnajesController : ControllerBase
    {

        readonly private orangeContext _context;
        readonly private IHubContext<tablaHub> _tablaHubContext;

        public MesnajesController(orangeContext context, IHubContext<tablaHub> tablaHubContext)
        {
            _context = context;
            _tablaHubContext = tablaHubContext;
        }




        [HttpPost]
        [Route("guardarMensaje")]
        public async Task<IActionResult> guardarAsignacionAsesor([FromBody] Mensaje model)
        {
            if (ModelState.IsValid)
            {
                _context.Mensaje.Add(model);
                if (await _context.SaveChangesAsync() > 0)
                {
                    //Console.WriteLine("CORTE DEL TIPO PARA VERIFICACION");
                    //Console.WriteLine(model.Codtipo.ToString());

                    // Realiza la validación de los dos primeros caracteres
                    if (model.Codtipo != null && model.Codtipo.Length >= 2)
                    {
                        string primerosDosCaracteres = model.Codtipo.Substring(0, 2);
                        if (primerosDosCaracteres == "US")
                        {
                            // No ejecuta SendAsync si los dos primeros caracteres son "US"
                            return Ok(model);
                        }
                    }

                    // Si los dos primeros caracteres no son "US", ejecuta SendAsync
                    await _tablaHubContext.Clients.All.SendAsync("TableMessage", model);
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


        [HttpGet("TareaCompletada/{codmsj}/{ctipo}")]
        public async Task<IActionResult> TareaCompletada([FromRoute] string codmsj, [FromRoute] string ctipo)
        {

            string Sentencia = " exec CompleteTask @cmsj, @ctipo ";

            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(Sentencia, connection))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.SelectCommand.CommandType = CommandType.Text;
                    adapter.SelectCommand.Parameters.Add(new SqlParameter("@cmsj", codmsj));
                    adapter.SelectCommand.Parameters.Add(new SqlParameter("@ctipo", ctipo));
                    adapter.Fill(dt);
                }
            }

            if (dt == null)
            {
                return NotFound("No se ha podido crear...");
            }

            return Ok(dt);

        }

        [HttpGet("ObtenerMensajes/{ccuser}")]
        public async Task<IActionResult> ObtenerMensajes([FromRoute] string ccuser)
        {

            string Sentencia = " exec ObtenerMensajes @coduser ";

            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(Sentencia, connection))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.SelectCommand.CommandType = CommandType.Text;
                    adapter.SelectCommand.Parameters.Add(new SqlParameter("@coduser", ccuser));
                    adapter.Fill(dt);
                }
            }

            if (dt == null)
            {
                return NotFound("No se ha podido crear...");
            }

            return Ok(dt);

        }

        [HttpPut]
        [Route("actualizarMensaje/{cmsj}")]
        public async Task<IActionResult> actualizarMensaje([FromRoute] string cmsj, [FromBody] Mensaje model )
        {

            if (cmsj != model.Codmsj)
            {
                return BadRequest("No existe este mensaje");
            }

            _context.Entry(model).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok(model);

        }

        [HttpGet("EstadoMsj/{estado}/{coduser}/{codmsj}")]
        public async Task<IActionResult> EstadoMsj([FromRoute] int estado, [FromRoute] string coduser, [FromRoute] string codmsj)
        {

            string Sentencia = " exec [MsjEstado] @st, @cuser, @cmsj ";

            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(Sentencia, connection))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.SelectCommand.CommandType = CommandType.Text;
                    adapter.SelectCommand.Parameters.Add(new SqlParameter("@st", estado));
                    adapter.SelectCommand.Parameters.Add(new SqlParameter("@cuser", coduser));
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


        [HttpGet("EliminarMensajes/{codmsj}")]
        public async Task<IActionResult> EliminarMensajes([FromRoute] string codmsj)
        {

            string Sentencia = " delete from mensaje where codmsj = @cmsj ";

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
                return NotFound("No se ha podido eliminar...");
            }

            return Ok(dt);

        }

    }
}
