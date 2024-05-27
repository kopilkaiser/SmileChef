using Newtonsoft.Json;

namespace SmileChef.Extensions
{
    public static class SessionExtensions
    {
        public static void SetObjectAsJson(this ISession session, string key, object value)
        {
            try
            {
                session.SetString(key, JsonConvert.SerializeObject(value));
            }
            catch (Exception ex)
            {
                // Log error
                Console.WriteLine($"Error setting session object: {ex.Message}");
            }
        }

        public static T? GetObjectFromJson<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            if (value == null)
            {
                return default(T);
            }

            try
            {
                return JsonConvert.DeserializeObject<T>(value);
            }
            catch (JsonException ex)
            {
                // Log error and return default value
                Console.WriteLine($"Error deserializing session object: {ex.Message}");
                return default(T);
            }
        }
    }
}
