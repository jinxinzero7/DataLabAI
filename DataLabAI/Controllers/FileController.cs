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

        // �������� ���������� ��� ���������� � ��� ������
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
                // ��������� 1
                if (file == null || file.Length == 0)
                    return BadRequest("File not chosen");

                // ��������� 2
                var extension = Path.GetExtension(file.FileName).ToLower();
                if (extension != ".csv")
                    return BadRequest("Only CSV files are available");

                var fileName = Guid.NewGuid().ToString() + extension;
                var filePath = Path.Combine(_uploadsFolder, fileName);

                // ���������� �����
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // ��� ����� ���� ������
                // var scriptPath = ".py";
                // var result = await RunPythonScript(scriptPath, filePath);

                // ��������� ����� data = result
                return Ok(new { message = "File succesfully uploaded" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

    }
}
