using System.Text.Json;
using System.Text.Json.Serialization;
using TennisStoreSharedLibrary.Contracts;
using TennisStoreSharedLibrary.Models;
using TennisStoreSharedLibrary.Responses;

namespace TennisStoreClient.Services
{
    public class ClientServices(HttpClient httpClient) : IProduct
    {
        private const string BaseUrl = "api/product";
        private static string SerilazedObj(object modelObj) => JsonSerializer.Serialize(modelObj,JsonOptions());
        private static T DeserializedJsonString<T>(string jsonString) => JsonSerializer.Deserialize<T>(jsonString, JsonOptions())!;
        private static StringContent GenerateStringContent(string serialiazedObj) => new(serialiazedObj, System.Text.Encoding.UTF8, "application/json");
        private static IEnumerable<T> DeserializedJsonStringList<T>(string jsonString) => JsonSerializer.Deserialize<IEnumerable<T>>(jsonString, JsonOptions())!;   
        private static JsonSerializerOptions JsonOptions()
        {
            return new JsonSerializerOptions
            {
                AllowTrailingCommas = true,
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                UnmappedMemberHandling = JsonUnmappedMemberHandling.Skip
            };
        }

        public Task<ServiceResponse> AddProduct(Product model)
        {
            throw new NotImplementedException();
        }

        public Task<List<Product>> GetAllProducts(bool featuredProducts)
        {
            throw new NotImplementedException();
        }
    }
}
