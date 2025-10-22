namespace Demo.Services;
using System.IO;

public static class ImageService
{
    public static IFormFile ConvertToIFormFile(string fileName)
    {
        //check if file is present or no
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot\\images\\{fileName}");
        if (!File.Exists(filePath))
            throw new FileNotFoundException("File not found", filePath);

        var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
        //public FormFile(Stream baseStream, long baseStreamOffset, long length, string name, string fileName)
        var ff = new FormFile(fileStream, 0, fileStream.Length, null, Path.GetFileName(filePath));

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