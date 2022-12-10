using Microsoft.Extensions.Configuration;
using GiantSMS.Net.Model;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Text;
using Newtonsoft.Json.Serialization;

namespace GiantSMS.Net
{
    public class GiantSMSClient
    {
        private static readonly string BASE_ENDPOINT_URL = "https://api.giantsms.com/api/v1";

        private string? Username { get; set; }
        private string? Password { get; set; }
        private string? Token { get; set; }
        private string? SenderId { get; set; }

        public GiantSMSClient(string username, string password, string token, string senderId)
        {
            Username = username;
            Password = password;
            Token = token;
            SenderId = senderId;
        }
        public GiantSMSClient(string username, string password)
        {
            Username = username;
            Password = password;
        }

        public GiantSMSClient(string username, string password, string senderId)
        {
            Username = username;
            Password = password;
            SenderId = senderId;
        }



        //public GiantSMSClient(HttpClient httpClient, IConfiguration configuration)
        //{
        //    connection = configuration.GetSection(nameof(GiantSmsConnector)).Get<GiantSmsConnector>();

        //    HttpClient = httpClient;

        //    if (connection.Token.Length > 0 || connection.Username.Length > 0) isReady = true;

        //}


        /// <summary>
        /// Send text messages to a single contact
        /// </summary>
        /// <param name="to"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task<SingleSmsResponse> SendSingleMessage(string to, string message)
        {
            using (var client = new HttpClient())
            {
                var username = Username;

                var password = Password;

                var from = SenderId;

                var response = await client.GetAsync($"{BASE_ENDPOINT_URL}/send?username={username}&password={password}&from={from}&to={to}&msg={message}");

                var responseString = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<SingleSmsResponse>(responseString);
            }
        }
        /// <summary>
        /// Send text messages to a single contact. 
        /// This method uses Basic Authorization header
        /// </summary>
        /// <param name="messageRequest"></param>
        /// <returns></returns>
        public async Task<SingleSmsResponse> SendMessageWithToken(SingleMessageRequest messageRequest)
        {
            using (var client = new HttpClient())
            {
                var request = new HttpRequestMessage();

                request.RequestUri = new Uri($"{BASE_ENDPOINT_URL}/send");

                request.Method = HttpMethod.Post;

                var token = Token;

                request.Headers.Add($"Authorization", $"Basic {token}");

                var bodyString = new
                {
                    from = SenderId,
                    to = messageRequest.To,
                    msg = messageRequest.Msg
                };

                var content = new StringContent(JsonConvert.SerializeObject(bodyString),
                    Encoding.UTF8, "application/json");

                request.Content = content;

                var response = await client.SendAsync(request);

                var responseString = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<SingleSmsResponse>(responseString);
            }
        }
        /// <summary>
        /// Sends bulk text messages to multiple contacts
        /// </summary>
        /// <param name="messageRequest"></param>
        /// <returns></returns>
        public async Task<BaseResponse> SendBulkMessages(BulkMessageRequest messageRequest)
        {
            using (var client = new HttpClient())
            {
                var request = new HttpRequestMessage();

                request.RequestUri = new Uri($"{BASE_ENDPOINT_URL}/send");

                request.Method = HttpMethod.Post;

                var token = Token;

                request.Headers.Add($"Authorization", $"Basic {token}");

                var bodyString = new
                {
                    from = SenderId,
                    recipients = messageRequest.Recipients,
                    msg = messageRequest.Msg
                };

                var content = new StringContent(JsonConvert.SerializeObject(bodyString),
                    Encoding.UTF8, "application/json");

                request.Content = content;

                var response = await client.SendAsync(request);

                var responseString = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<BaseResponse>(responseString);
            }
        }

        /// <summary>
        /// Checks the delivery status and details of a message 
        /// </summary>
        /// <param name="messageId"></param>
        /// <returns></returns>
        public async Task<SingleSmsResponse> CheckMessageStatus(string messageId)
        {
            using (var client = new HttpClient())
            {
                var request = new HttpRequestMessage();

                request.RequestUri = new Uri($"{BASE_ENDPOINT_URL}/status");

                request.Method = HttpMethod.Post;

                var token = Token;

                request.Headers.Add($"Authorization", $"Basic {token}");

                var bodyString = new
                {
                    message_id = messageId
                };

                var content = new StringContent(JsonConvert.SerializeObject(bodyString),
                    Encoding.UTF8, "application/json");

                request.Content = content;

                var response = await client.SendAsync(request);

                var responseString = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<SingleSmsResponse>(responseString);
            }
        }

        /// <summary>
        /// Returns remaining SMS Credits
        /// </summary>
        /// <returns></returns>
        public async Task<BaseResponse> GetBalance()
        {
            using (var client = new HttpClient())
            {
                var username = Username;

                var password = Password;

                var response = await client!.GetAsync($"{BASE_ENDPOINT_URL}/balance?username={username}&password={password}");

                var responseString = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<BaseResponse>(responseString);
            }

        }

        /// <summary>
        /// Returns an array object of registered sender IDs
        /// </summary>
        /// <returns></returns>
        public async Task<SenderIdResponse> GetSenderIds()
        {
            using (var client = new HttpClient())
            {
                var username = Username;

                var password = Password;

                var response = await client!.GetAsync($"{BASE_ENDPOINT_URL}/sender?username={username}&password={password}");

                var responseString = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<SenderIdResponse>(responseString);
            }
        }

        /// <summary>
        /// Registers a sender ID subject to approval.
        /// </summary>
        /// <param name="senderIdRequest"></param>
        /// <returns></returns>

        public async Task<BaseResponse> RegisterSenderIds(RegisterSenderIdRequest senderIdRequest)
        {
            using (var client = new HttpClient())
            {
                var request = new HttpRequestMessage();

                request.Method = HttpMethod.Post;

                var username = Username;

                var password = Password;

                request.RequestUri = new Uri($"{BASE_ENDPOINT_URL}/sender/register?username={username}&password={password}");

                var bodyString = new
                {
                    name = senderIdRequest.Name,
                    purpose = senderIdRequest.Purpose
                };

                var content = new StringContent(JsonConvert.SerializeObject(bodyString),
                    Encoding.UTF8, "application/json");

                request.Content = content;

                var response = await client.SendAsync(request);

                var responseString = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<BaseResponse>(responseString);
            }
        }





    }
}