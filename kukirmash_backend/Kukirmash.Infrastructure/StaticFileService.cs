using Kukirmash.Application.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Kukirmash.Infrastructure;

public class StaticFileService : IStaticFileService
{
    //*----------------------------------------------------------------------------------------------------------------------------
    private readonly IWebHostEnvironment _env;
    private readonly string[] _allowedExtensions = { ".jpg", ".jpeg", ".png", ".webp" };

    public StaticFileService(IWebHostEnvironment env)
    {
        _env = env;
    }

    //*----------------------------------------------------------------------------------------------------------------------------
    public async Task<string> UploadFile(Stream fileStream, string fileName, string folderPath)
    {
        // 1.Проверки входных данных
        if (fileStream == null || fileStream.Length == 0)
            throw new ArgumentException("Файл пуст", nameof(fileStream));

        // 2. Проверка расширения
        var extension = Path.GetExtension(fileName).ToLowerInvariant();
        if (string.IsNullOrEmpty(extension) || !_allowedExtensions.Contains(extension))
            throw new ArgumentException($"Расширение {extension} не поддерживается.");

        // 3. Формируем физический путь
        // _env.WebRootPath указывает на папку wwwroot
        var physicalUploadPath = Path.Combine(_env.WebRootPath, folderPath);

        if (!Directory.Exists(physicalUploadPath))
            Directory.CreateDirectory(physicalUploadPath);

        // 4. Генерируем уникальное имя
        var uniqueFileName = $"{Guid.NewGuid()}{extension}";
        var fullPhysicalPath = Path.Combine(physicalUploadPath, uniqueFileName);

        // 5. Сохраняем (используем переданный Stream)
        try
        {
            // Важно: если поток пришел не с начала, сбрасываем позицию
            if (fileStream.CanSeek)
                fileStream.Position = 0;

            using (var fileStreamOutput = new FileStream(fullPhysicalPath, FileMode.Create))
            {
                await fileStream.CopyToAsync(fileStreamOutput);
            }
        }
        catch (Exception ex)
        {
            // Логируем ошибку здесь или пробрасываем выше
            throw new Exception($"Не удалось сохранить файл на диск: {ex.Message}", ex);
        }

        // 6. Возвращаем веб-путь (URL-friendly)
        return $"/{folderPath}/{uniqueFileName}".Replace("\\", "/");
    }

    //*----------------------------------------------------------------------------------------------------------------------------
    public void Delete(string path)
    {
        throw new NotImplementedException();
    }

    //*----------------------------------------------------------------------------------------------------------------------------
}
