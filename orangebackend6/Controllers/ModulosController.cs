using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using orangebackend6.Models;
using System.Data;

namespace orangebackend6.Controllers
{
    [Route("api/Modulos")]
    [ApiController]
    public class ModulosController : ControllerBase
    {

        private readonly orangeContext _context;

        public ModulosController(orangeContext context)
        {
            _context = context;
        }

        [HttpGet("GetModulos/{userCod}")]
        public async Task<IActionResult> GetModulos([FromRoute] string userCod)
        {

            string Sentencia = " select a.permisos, a.cod_user, b.* from asignModUser as a " +
                               " left join modulo as b on a.cod_mod = b.id " +
                               " where a.permisos != 0 and a.cod_user = @usCod order by order_mod asc ";

            DataTable dt = new DataTable();
            using ( SqlConnection connection = new SqlConnection( _context.Database.GetDbConnection().ConnectionString ) )
            {
                using (SqlCommand cmd = new SqlCommand(Sentencia, connection))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.SelectCommand.CommandType = CommandType.Text;
                    adapter.SelectCommand.Parameters.Add(new SqlParameter("@usCod", userCod));
                    adapter.Fill(dt);
                }
            }

            if (dt == null)
            {
                return NotFound("No se encontro este WebUser...");
            }

            return Ok(dt);

        }

        [HttpGet("ObtenerDetalleModulo/{Ids}")]
        public async Task<IActionResult> ObtenerDetalleModulo([FromRoute] int Ids)
        {

            string Sentencia = " select * from detalleModulo where idmodules = @id ";

            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(Sentencia, connection))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.SelectCommand.CommandType = CommandType.Text;
                    adapter.SelectCommand.Parameters.Add(new SqlParameter("@id", Ids));
                    adapter.Fill(dt);
                }
            }

            if (dt == null)
            {
                return NotFound("No se encontro este WebUser...");
            }

            return Ok(dt);

        }

    }
}
