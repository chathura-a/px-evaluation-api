using RestSharp;

namespace PropertyExperts.Evaluation.API.Utils.FileUtils
{
    public static class FileUtils
    {
        public static async Task<byte[]> ConvertFormFileToByteArray(IFormFile file)
        {
            await using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            return memoryStream.ToArray();
        }
    }
}
