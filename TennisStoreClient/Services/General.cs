using System.Text.Json.Serialization;
using System.Text.Json;

namespace TennisStoreClient.Services
{
    public static class General
    {
        #region Serialization Methods

        public static string SerilazedObj(object modelObj) =>
            JsonSerializer.Serialize(modelObj, JsonOptions());

        public static T DeserializedJsonString<T>(string jsonString) =>
            JsonSerializer.Deserialize<T>(jsonString, JsonOptions())!;

        public static IList<T> DeserializedJsonStringList<T>(string jsonString) =>
            JsonSerializer.Deserialize<IList<T>>(jsonString, JsonOptions())!;

        public static StringContent GenerateStringContent(string serialiazedObj) =>
            new(serialiazedObj, System.Text.Encoding.UTF8, "application/json");

        #endregion

        #region Json Options

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

        #endregion

        #region Utility Methods

        public static string GetDescription(string description)
        {
            const string appendDots = "...";
            const int maxLength = 100;

            return description.Length > maxLength
                ? $"{description.Substring(0, maxLength)}{appendDots}"
                : description;
        }

        #endregion
    }
}
