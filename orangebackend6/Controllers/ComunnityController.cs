using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using orangebackend6.Models;
using System.Data;

namespace orangebackend6.Controllers
{
    [Route("api/Community")]
    [ApiController]
    public class ComunnityController : ControllerBase
    {

        private readonly orangeContext _context;

        public ComunnityController(orangeContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("guardarComunnity")]
        public async Task<IActionResult> guardarComunnity([FromBody] Community model)
        {

            if (ModelState.IsValid)
            {
                _context.Community.Add(model);
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

        [HttpGet("ObtenerComunnity/{stat}/{cusrem}/{permison}/{tp}")]
        public async Task<IActionResult> ObtenerComunnity([FromRoute] string stat, [FromRoute] string cusrem, [FromRoute] string permison, [FromRoute] string tp)
        {

            string Sentencia = " exec InvitacionesComunidades @state, @cuserremit, @perm, @type ";

            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(Sentencia, connection))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.SelectCommand.CommandType = CommandType.Text;
                    adapter.SelectCommand.Parameters.Add(new SqlParameter("@state", stat));
                    adapter.SelectCommand.Parameters.Add(new SqlParameter("@cuserremit", cusrem));
                    adapter.SelectCommand.Parameters.Add(new SqlParameter("@perm", permison));
                    adapter.SelectCommand.Parameters.Add(new SqlParameter("@type", tp));
                    adapter.Fill(dt);
                }
            }

            if (dt == null)
            {
                return NotFound("No se ha podido crear...");
            }

            return Ok(dt);


        }
        
        [HttpGet("ObtenerComunnityPerteneciente/{cuser}/{type}")]
        public async Task<IActionResult> ObtenerComunnityPerteneciente([FromRoute] string cuser, [FromRoute] int type)
        {

            string Sentencia = " exec ObtenerCommunity @coduser, @tp ";

            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(Sentencia, connection))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.SelectCommand.CommandType = CommandType.Text;
                    adapter.SelectCommand.Parameters.Add(new SqlParameter("@coduser", cuser));
                    adapter.SelectCommand.Parameters.Add(new SqlParameter("@tp", type));
                    adapter.Fill(dt);
                }
            }

            if (dt == null)
            {
                return NotFound("No se ha podido crear...");
            }

            return Ok(dt);


        }


        [HttpGet("EstadoInvitacion/{stat}/{cusrem}/{permison}/{tp}/{codcommunity}")]
        public async Task<IActionResult> EstadoInvitacion([FromRoute] string stat, [FromRoute] string cusrem, [FromRoute] string permison, [FromRoute] int tp, [FromRoute] string codcommunity)
        {

            string Sentencia = " exec EstadoInvitacion  @state, @cuserremit, @perm, @type, @ccommunity ";

            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(Sentencia, connection))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.SelectCommand.CommandType = CommandType.Text;
                    adapter.SelectCommand.Parameters.Add(new SqlParameter("@state", stat));
                    adapter.SelectCommand.Parameters.Add(new SqlParameter("@cuserremit", cusrem));
                    adapter.SelectCommand.Parameters.Add(new SqlParameter("@perm", permison));
                    adapter.SelectCommand.Parameters.Add(new SqlParameter("@type", tp));
                    adapter.SelectCommand.Parameters.Add(new SqlParameter("@ccommunity", codcommunity));
                    adapter.Fill(dt);
                }
            }

            if (dt == null)
            {
                return NotFound("No se ha podido crear...");
            }

            return Ok(dt);


        }

        [HttpGet("GestionComunidad/{codcommunity}/{tp}/{invit}")]
        public async Task<IActionResult> GestionComunidad([FromRoute] string codcommunity, [FromRoute] int tp, [FromRoute] string invit)
        {

            string Sentencia = " exec GestionComunidad  @ccommunity, @type, @invitacion ";

            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(Sentencia, connection))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.SelectCommand.CommandType = CommandType.Text;
                    adapter.SelectCommand.Parameters.Add(new SqlParameter("@ccommunity", codcommunity));
                    adapter.SelectCommand.Parameters.Add(new SqlParameter("@type", tp));
                    adapter.SelectCommand.Parameters.Add(new SqlParameter("@invitacion", invit));
                    adapter.Fill(dt);
                }
            }

            if (dt == null)
            {
                return NotFound("No se ha podido crear...");
            }

            return Ok(dt);


        }

        
        //[HttpPut]
        //[Route("EditarComunidad/{codcommunity}")]
        //public async Task<IActionResult> EditarComunidad( [FromRoute] string codcommunity,
        //                                                  [FromBody] Community model )
        //{

        //    Console.WriteLine( "Estes el codcommunity enciado: " + codcommunity);
        //    Console.WriteLine( model );

        //    if (codcommunity != model.Codcomunity)
        //    {
        //        return BadRequest("No existe la maquina");
        //    }

        //    _context.Entry(model).State = EntityState.Modified;
        //    await _context.SaveChangesAsync();
        //    return Ok(model);

        //}

        [HttpPost("EditarComunidad")]
        public async Task<IActionResult> EditarComunidad([FromBody] CommunityDto model)
        {

            string Sentencia = " exec UpdateCommunity @codcomun, @nomcommun, @icon, @color ";

            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(Sentencia, connection))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.SelectCommand.CommandType = CommandType.Text;
                    adapter.SelectCommand.Parameters.Add(new SqlParameter("@codcomun", model.Codcomunity));
                    adapter.SelectCommand.Parameters.Add(new SqlParameter("@nomcommun", model.Nombrecomunity));
                    adapter.SelectCommand.Parameters.Add(new SqlParameter("@icon", model.Icon));
                    adapter.SelectCommand.Parameters.Add(new SqlParameter("@color", model.Color));
                    adapter.Fill(dt);
                }
            }

            if (dt == null)
            {
                return NotFound("No se ha podido crear...");
            }

            return Ok(dt);


        }


        //[HttpGet("eliminarInvitacion/{id}")]
        //public async Task<IActionResult> eliminarInvitacion([FromRoute] int id)
        //{
        //    // Verificar si codcomunity está en un formato válido (por ejemplo, longitud o caracteres permitidos)
        //    //if (string.IsNullOrEmpty(codcomunity))
        //    //{
        //    //    return BadRequest("El parámetro codcomunity es inválido.");
        //    //}

        //    // Sentencia SQL para eliminar la comunidad si cumple con los criterios
        //    string Sentencia = "DELETE FROM community WHERE id = @IDS ";

        //    DataTable dt = new DataTable();

        //    try
        //    {
        //        using (SqlConnection connection = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
        //        {
        //            using (SqlCommand cmd = new SqlCommand(Sentencia, connection))
        //            {
        //                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
        //                adapter.SelectCommand.CommandType = CommandType.Text;
        //                adapter.SelectCommand.Parameters.Add(new SqlParameter("@IDS", id));
        //                adapter.Fill(dt);
        //            }
        //        }

        //        // Verificar si se eliminó correctamente y si hay resultados en el DataTable
        //        if (dt != null && dt.Rows.Count > 0)
        //        {
        //            return Ok("La comunidad ha sido eliminada correctamente.");
        //        }
        //        else
        //        {
        //            return NotFound("No se ha podido eliminar porque todavía tienes usuarios dentro de esta comunidad.");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // En caso de errores de base de datos u otras excepciones
        //        return StatusCode(500, "Error interno del servidor: " + ex.Message);
        //    }
        //}



        [HttpGet("eliminarInvitacion/{id}")]
        public async Task<IActionResult> eliminarInvitacion([FromRoute] int id)
        {

            string Sentencia = "DELETE FROM community WHERE id = @IDS ";

            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(Sentencia, connection))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.SelectCommand.CommandType = CommandType.Text;
                    adapter.SelectCommand.Parameters.Add(new SqlParameter("@IDS", id));
                    adapter.Fill(dt);
                }
            }

            if (dt == null)
            {
                return NotFound("No se ha podido eliminar...");
            }

            return Ok(dt);


        }

        [HttpGet("eliminarUsuarioCounnidad/{codcommu}/{coduserinvite}")]
        public async Task<IActionResult> eliminarUsuarioCounnidad([FromRoute] string codcommu, [FromRoute] string coduserinvite)
        {

            string Sentencia = " delete from community where codcomunity = @ccomu and coduserinvite = @ccuinvite ";

            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(Sentencia, connection))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.SelectCommand.CommandType = CommandType.Text;
                    adapter.SelectCommand.Parameters.Add(new SqlParameter("@ccomu",     codcommu));
                    adapter.SelectCommand.Parameters.Add(new SqlParameter("@ccuinvite", coduserinvite));
                    adapter.Fill(dt);
                }
            }

            if (dt == null)
            {
                return NotFound("No se ha podido crear...");
            }

            return Ok(dt);


        }


        public class CommunityDto
        {
            public string Codcomunity { get; set; }
            public string Nombrecomunity { get; set; }
            public string Icon { get; set; }
            public string Color { get; set; }
        }

    }
}
