using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using orangebackend6.Models;
using System.Data;

namespace orange.Controllers
{
    [Route("api/Login")]
    [ApiController]
    public class LoginController : ControllerBase
    {

        readonly private orangeContext _context;

        public LoginController(orangeContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] Usuario userInfo)
        {
            var result = await _context.Usuario.FirstOrDefaultAsync(x => x.Email == userInfo.Email && x.Password == userInfo.Password);
            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                //ModelState.AddModelError(string.Empty, "Usuario o contraseña invalido");
                return BadRequest("Datos incorrectos");
            }
        }



    }
}
