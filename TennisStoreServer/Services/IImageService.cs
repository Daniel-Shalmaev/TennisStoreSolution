using Newtonsoft.Json;

namespace TennisStoreServer.Services
{
    public interface IImageService
    {
        Task<string> UploadImageAsync(string base64Image);
    }

    public class ImageService(IConfiguration configuration) : IImageService
    {
        private readonly string _clientId = configuration["Imgur:ClientId"]!;

        public async Task<string> UploadImageAsync(string base64Image)
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", $"Client-ID {_clientId}");

            var content = new MultipartFormDataContent
        {
            { new StringContent(base64Image), "image" }
        };

            var response = await client.PostAsync("https://api.imgur.com/3/image", content);
            if (!response.IsSuccessStatusCode)
                throw new Exception("Failed to upload image to Imgur.");

            var responseBody = await response.Content.ReadAsStringAsync();
            dynamic result = JsonConvert.DeserializeObject(responseBody)!;
            return result.data.link;
        }
    }
}