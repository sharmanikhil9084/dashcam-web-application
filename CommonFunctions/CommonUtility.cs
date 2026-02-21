using System.Data;
using System.Net.Http;
using System.Text.Json;

namespace TrackanDrive.Dashcam.CommonFunctions
{
    public static class CommonUtility
    {
        public static string DecodeFrom64(string encodedData)
        {
            System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
            System.Text.Decoder utf8Decode = encoder.GetDecoder();
            byte[] todecode_byte = Convert.FromBase64String(encodedData);
            int charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
            char[] decoded_char = new char[charCount];
            utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
            string result = new String(decoded_char);
            return result;
        }

        public static async Task<string> PostFormUrlAsync<TIn>(string uri, Dictionary<string, string> keyValuePairs)
        {
            string result = string.Empty;
            HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(10);
            var content = new FormUrlEncodedContent(keyValuePairs);
            var httpResponseMessage = await client.PostAsync(uri, content);
            result = httpResponseMessage.Content.ReadAsStringAsync().Result;

            if (httpResponseMessage.IsSuccessStatusCode)
            {

            }
            return result;
        }

    }
}
