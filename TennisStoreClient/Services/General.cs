using System.Text.Json.Serialization;
using System.Text.Json;

namespace TennisStoreClient.Services
{
    public static class General
    {
        public static string SerilazedObj(object modelObj) => JsonSerializer.Serialize(modelObj, JsonOptions());
        public static T DeserializedJsonString<T>(string jsonString) => JsonSerializer.Deserialize<T>(jsonString, JsonOptions())!;
        public static StringContent GenerateStringContent(string serialiazedObj) => new(serialiazedObj, System.Text.Encoding.UTF8, "application/json");
        public static IList<T> DeserializedJsonStringList<T>(string jsonString) => JsonSerializer.Deserialize<IList<T>>(jsonString, JsonOptions())!;
        public static JsonSerializerOptions JsonOptions()
        {
            return new JsonSerializerOptions
            {
                AllowTrailingCommas = true,
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                UnmappedMemberHandling = JsonUnmappedMemberHandling.Skip
            };
        }
        public static string GetDescription(string description)
        {
            string appendDots = "...";
            int maxLength = 100;
            int descriptionLength = description.Length;
            return descriptionLength > maxLength ? $"{description.Substring(0, 100)}{appendDots}" : description;
        }
    }
}
