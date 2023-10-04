using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using orangebackend6.Models;
using System.Data;

namespace orangebackend6.Controllers
{
    [Route("api/PostReaction")]
    [ApiController]
    public class PostReactionController : ControllerBase
    {

        private readonly orangeContext _context;

        public PostReactionController(  orangeContext context)
        {
            _context = context;
        }

        //[HttpPost]
        //[Route("GuardarReactionPost")]
        //public async Task<IActionResult> GuardarReactionPost([FromBody] Postreaction model)
        //{

        //    if (ModelState.IsValid)
        //    {
        //        _context.Postreaction.Add(model);
        //        if (await _context.SaveChangesAsync() > 0)
        //        {
        //            return Ok(model);
        //        }

        //        else
        //        {
        //            return BadRequest("Datos incorrectos");
        //        }
        //    }
        //    else
        //    {
        //        return BadRequest("ERROR");
        //    }
        //}

        public class EmoticonDto
        {
            public string codpost { get; set; }
            public string codemoticon { get; set; }
            public string coduser { get; set; }
        }

        [HttpGet("ObtenerReactionPost/{codposts}")]
        public async Task<IActionResult> ObtenerReactionPost([FromRoute] string codposts )
        {

            string Sentencia = "exec ObtenerEmoticones @codpost";

            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(Sentencia, connection))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.SelectCommand.CommandType = CommandType.Text;
                    adapter.SelectCommand.Parameters.Add(new SqlParameter("@codpost", codposts));
                    adapter.Fill(dt);
                }
            }

            if (dt == null)
            {
                return NotFound("No se ha podido crear...");
            }

            return Ok(dt);


        }
        [HttpPost("GuardarReactionPost")]
        public async Task<IActionResult> GuardarReactionPost([FromBody] EmoticonDto model )
        {

            string Sentencia = "exec GuardarEmoticon @codpost, @codemoticon, @coduser";

            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(Sentencia, connection))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.SelectCommand.CommandType = CommandType.Text;
                    adapter.SelectCommand.Parameters.Add(new SqlParameter("@codpost",     model.codpost));
                    adapter.SelectCommand.Parameters.Add(new SqlParameter("@codemoticon", model.codemoticon));
                    adapter.SelectCommand.Parameters.Add(new SqlParameter("@coduser",     model.coduser));
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
