using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using orangebackend6.Models;

namespace orangebackend6.Controllers
{
    [Route("api/Listas")]
    [ApiController]
    public class ListasController : ControllerBase
    {

        readonly private orangeContext _context;

        public ListasController (orangeContext  context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("guardarLista")]
        public async Task<IActionResult> guardarLista( [FromBody] Listascontrol model )
        {

            if (ModelState.IsValid)
            {
                _context.Listascontrol.Add(model);
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

    }
}
