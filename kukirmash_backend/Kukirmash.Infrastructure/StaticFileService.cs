using Kukirmash.Application.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Kukirmash.Infrastructure;

public class StaticFileService : IStaticFileService
{
    //*----------------------------------------------------------------------------------------------------------------------------
    private readonly IWebHostEnvironment _env;

    public StaticFileService(IWebHostEnvironment env)
    {
        _env = env;
    }

    //*----------------------------------------------------------------------------------------------------------------------------
    public async Task<string> AddUniq(IFormFile file, string folderPath)
    {
        if (file == null || file.Length == 0)
            return null;

        try
        {
            // 1. Формируем путь к папке: wwwroot/...
            // Path.Combine сам подставит нужные слэши
            var uploadsFolder = Path.Combine(_env.WebRootPath, folderPath);

            // 2. Создаем директорию, если её нет
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            // 3. Генерируем уникальное имя файла
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var fullPath = Path.Combine(uploadsFolder, fileName);

            // 4. Сохраняем файл на диск
            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // 5. Возвращаем относительный путь для хранения в БД (веб-путь)
            // Результат: /media/server-icons/название_файла.jpg
            return $"/{folderPath}/{fileName}";
        }
        catch (Exception ex)
        {
            throw new Exception($"Ошибка при сохранении файла {file.FileName}: {ex.Message}");
        }
    }

    //*----------------------------------------------------------------------------------------------------------------------------
    public void Delete(string path)
    {
        throw new NotImplementedException();
    }

    //*----------------------------------------------------------------------------------------------------------------------------
}
