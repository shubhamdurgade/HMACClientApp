using System.Net.Http.Headers;
using System.Security.Cryptography; 
using System.Text;

namespace HMACClientApp
{
    public class HMACHelper
    {
        public static string GenerateHmacToken(string method, string path, string clientId, string secretKey, string requestBody = "")
        {
            var nonce = Guid.NewGuid().ToString();
            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();

            var requestContent = new StringBuilder()
                .Append(method.ToUpper())
                .Append(path.ToUpper())
                .Append(nonce)
                .Append(timestamp);

            if (method == HttpMethod.Post.Method || method == HttpMethod.Put.Method)
            {
                requestContent.Append(requestBody);
            }

            var secretKeyBytes = Encoding.UTF8.GetBytes(secretKey);
            var requestBytes = Encoding.UTF8.GetBytes(requestContent.ToString());

            using var hmac = new HMACSHA256(secretKeyBytes);

            var computedHash = hmac.ComputeHash(requestBytes);

            var computedToken = Convert.ToBase64String(computedHash);

            return $"HMAC {clientId}|{computedToken}|{nonce}|{timestamp}";
        }

        //Helper method to send an API request and returns HttpResponseMessage
        public static async Task<HttpResponseMessage> SendRequestAsync(
            HttpClient httpClient, 
            HttpMethod method, 
            string baseUrl, 
            string endPoint, 
            string clientId, 
            string secretKey, 
            object? data = null)
        {
            var requestBody = data != null ? System.Text.Json.JsonSerializer.Serialize(data) : string.Empty;

            var token = GenerateHmacToken(method.Method, endPoint, clientId, secretKey, requestBody);

            var requestMessage = new HttpRequestMessage(method, $"{baseUrl}{endPoint}")
            {
                Content = !string.IsNullOrEmpty(requestBody) ? new StringContent(requestBody, Encoding.UTF8, "application/json") : null
            };

            // Authorization header value contains characters (colons, plus, slash, etc.) that the default header parser may reject.
            // Use TryAddWithoutValidation so the header is sent exactly as generated.
            requestMessage.Headers.TryAddWithoutValidation("Authorization", token);

            return await httpClient.SendAsync(requestMessage);
        }
    }
}
