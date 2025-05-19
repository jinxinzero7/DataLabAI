using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Reflection;


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
        public async Task<IActionResult> UploadFile(IFormFile file, string delimiter = ",")
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

                var scriptPath = Path.Combine(Directory.GetCurrentDirectory(), "PythonCode", "quick.py");

                var args = $"{filePath} {delimiter}";

                try
                {
                    var result = await RunPythonScript(scriptPath, args);

                    // Десериализация JSON результата
                    var results = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, object>>>(result); // Изменено для соответствия JSON структуре

                    return Ok(results);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"Error running Python script: {ex.Message}");
                }

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        private async Task<string> RunPythonScript(string scriptPath, string args)
        {
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "D:\\anaconda\\envs\\DataLabAI\\python.exe",
                    Arguments = $"{scriptPath} {args}",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };

            process.Start();

            string output = await process.StandardOutput.ReadToEndAsync();
            string error = await process.StandardError.ReadToEndAsync();

            await process.WaitForExitAsync();

            if (!string.IsNullOrEmpty(error))
            {
                Console.WriteLine($"Python script error: {error}"); // Добавьте вывод ошибки
                throw new Exception($"Python script error: {error}");
            }

            return output;
        }


    }
}
