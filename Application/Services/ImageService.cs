namespace Services;

using Microsoft.AspNetCore.Http;
using System.IO;

public static class ImageService
{
    public static IFormFile ConvertToIFormFile(string fileName)
    {
        
        if(string.IsNullOrEmpty(fileName))
            return null;
        
        //check if file is present or no
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot\\images\\{fileName}");

        //using memory stream to avoid locking
        var bytes = File.ReadAllBytes(filePath);
        var memoryStream = new MemoryStream(bytes);
        
        //public FormFile(Stream baseStream, long baseStreamOffset, long length, string name, string fileName)
        var ff = new FormFile(memoryStream, 0, memoryStream.Length, null, Path.GetFileName(filePath));

        return ff;
    }

    public static void UploadImageToDirectory(IFormFile file, string directory, string  fileName)
    {
        if (file == null || file.Length == 0)
        {
            throw new ArgumentException("File is null or empty.", nameof(file));
        }
        
        string filePath = Path.Combine(directory, fileName);
        
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            file.CopyTo(stream);
        }
    }
}