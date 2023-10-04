using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

[Route("api/storage")]
public class StorageController : ControllerBase
{
    private readonly IWebHostEnvironment _hostingEnvironment;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public StorageController(IWebHostEnvironment hostingEnvironment, IHttpContextAccessor httpContextAccessor)
    {
        _hostingEnvironment = hostingEnvironment;
        _httpContextAccessor = httpContextAccessor;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> UploadFile()
    {
        try
        {
            var file = _httpContextAccessor.HttpContext.Request.Form.Files[0];

            if (file.Length > 0)
            {
                var allowedExtensions = new[] { ".mp4", ".jpg", ".jpeg", ".png", ".pdf", ".xls", ".xlsx", ".txt", ".doc", ".mp3" };
                var fileExtension = Path.GetExtension(file.FileName).ToLower();

                if (!allowedExtensions.Contains(fileExtension))
                {
                    return BadRequest("Tipo de archivo no permitido.");
                }

                var storagePath = Path.Combine(_hostingEnvironment.WebRootPath, "storage");

                // Utiliza el nombre original del archivo
                var originalFileName = file.FileName;

                // Reemplaza espacios en blanco por guiones bajos
                var fileName = originalFileName.Replace(" ", "_");

                var filePath = Path.Combine(storagePath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Puedes devolver la URL del archivo como respuesta.
                var fileUrl = $"/storage/{fileName}";

                return Ok(new { message = "Archivo cargado con éxito", fileUrl });
            }
            else
            {
                return BadRequest("El archivo está vacío.");
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error interno del servidor: {ex.Message}");
        }
    }


}
