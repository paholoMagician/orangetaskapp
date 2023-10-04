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
    [Route("api/ApiSystems")]
    [ApiController]
    public class ApiSystemsController : ControllerBase
    {

        readonly private orangeContext _context;
        //readonly private IHubContext<tablaHub> _tablaHubContext;

        public ApiSystemsController(orangeContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("apiprefix")]
        public async Task<IActionResult> apiprefix([FromBody] Apiurlactive model)
        {

            if (ModelState.IsValid)
            {
                _context.Apiurlactive.Add(model);
                if (await _context.SaveChangesAsync() > 0)
                {
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

        [HttpGet("obtenerPrefix")]
        public async Task<IActionResult> obtenerPrefix()
        {

            string Sentencia = " select * from Apiurlactive where estado = 1 ";

            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(Sentencia, connection))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.SelectCommand.CommandType = CommandType.Text;
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
