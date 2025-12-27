using Microsoft.AspNetCore.Http;

namespace Student.API.Helpers;

public interface IFileStorageService
{
    Task<string> SaveStudentPhotoAsync(IFormFile file);
}
