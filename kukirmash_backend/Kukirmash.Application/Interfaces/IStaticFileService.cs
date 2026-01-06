namespace Kukirmash.Application.Interfaces;

public interface IStaticFileService
{
    Task<string> UploadFile(Stream file, string fileName, string folderPath);

    void Delete(string path);
}
