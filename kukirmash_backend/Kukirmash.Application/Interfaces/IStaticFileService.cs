using Microsoft.AspNetCore.Http;

namespace Kukirmash.Application.Interfaces;

public interface IStaticFileService
{
    Task<string> AddUniq(IFormFile file, string folderPath);

    void Delete(string path);
}
