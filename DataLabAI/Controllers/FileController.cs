using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;
using System.Diagnostics;

namespace DataLabAI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FileController : ControllerBase
    {
        private readonly string _uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");

        // создание директории для сохранения в нее файлов
        public FileController()
        {
            if (!Directory.Exists(_uploadsFolder))
                Directory.CreateDirectory(_uploadsFolder);
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            try
            {   
                // валидация 1
                if (file == null || file.Length == 0)
                    return BadRequest("File not chosen");

                // валидация 2
                var extension = Path.GetExtension(file.FileName).ToLower();
                if (extension != ".csv")
                    return BadRequest("Only CSV files are available");

                var fileName = Guid.NewGuid().ToString() + extension;
                var filePath = Path.Combine(_uploadsFolder, fileName);

                // сохранение файла
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // как будет файл создам
                // var scriptPath = ".py";
                // var result = await RunPythonScript(scriptPath, filePath);

                // присвоить потом data = result
                return Ok(new { message = "File succesfully uploaded" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

    }
}
