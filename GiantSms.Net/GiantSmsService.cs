using GiantSms.Net.Model.Requests;
using GiantSms.Net.Model.Responses;
using GiantSms.Net.Interfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Text;

namespace GiantSms.Net
{
    public class GiantSmsService : IGiantSmsService
    {
        private static readonly string BASE_ENDPOINT_URL = "https://api.giantsms.com/api/v1";

        private readonly GiantSmsConnection connection;

        private readonly HttpClient _httpClient;

        private bool isReady;

        public GiantSmsService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            connection = configuration.GetSection(nameof(GiantSmsConnection)).Get<GiantSmsConnection>();

            _httpClient = httpClientFactory.CreateClient();

            if (connection!.Token!.Length > 0 || connection!.Username!.Length > 0) isReady = true;

        }

        /// <summary>
        /// Checks the delivery status and details of a message 
        /// </summary>
        /// <param name="messageId"></param>
        /// <returns></returns>
        public async Task<SingleSmsResponse> CheckMessageStatus(string messageId)
        {
            var request = new HttpRequestMessage();

            request.RequestUri = new Uri($"{BASE_ENDPOINT_URL}/status");

            request.Method = HttpMethod.Post;

            var token = connection.Token;

            request.Headers.Add($"Authorization", $"Basic {token}");

            var bodyString = new
            {
                message_id = messageId
            };

            var content = new StringContent(JsonConvert.SerializeObject(bodyString),
                Encoding.UTF8, "application/json");

            request.Content = content;

            var response = await _httpClient.SendAsync(request);

            var responseString = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<SingleSmsResponse>(responseString) ?? throw new ArgumentNullException(nameof(responseString));
        }

        /// <summary>
        /// Returns remaining SMS Credits
        /// </summary>
        /// <returns></returns>
        public async Task<BaseResponse> GetBalance()
        {
            var username = connection.Username;

            var password = connection.Password;

            var response = await _httpClient!.GetAsync($"{BASE_ENDPOINT_URL}/balance?username={username}&password={password}");

            var responseString = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<BaseResponse>(responseString) ?? throw new ArgumentNullException(nameof(responseString));
        }

        /// <summary>
        /// Returns an array object of registered sender IDs
        /// </summary>
        /// <returns></returns>
        public async Task<SenderIdResponse> GetSenderIds()
        {
            var username = connection.Username;

            var password = connection.Password;

            var response = await _httpClient!.GetAsync($"{BASE_ENDPOINT_URL}/sender?username={username}&password={password}");

            var responseString = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<SenderIdResponse>(responseString) ?? throw new ArgumentNullException(nameof(responseString));
        }

        /// <summary>
        /// Registers a sender ID subject to approval.
        /// </summary>
        /// <param name="senderIdRequest"></param>
        /// <returns></returns>
        public async Task<BaseResponse> RegisterSenderIds(RegisterSenderIdRequest senderIdRequest)
        {
            var request = new HttpRequestMessage();

            request.Method = HttpMethod.Post;

            var username = connection.Username;

            var password = connection.Password;

            request.RequestUri = new Uri($"{BASE_ENDPOINT_URL}/sender/register?username={username}&password={password}");

            var bodyString = new
            {
                name = senderIdRequest.Name,
                purpose = senderIdRequest.Purpose
            };

            var content = new StringContent(JsonConvert.SerializeObject(bodyString),
                Encoding.UTF8, "application/json");

            request.Content = content;

            var response = await _httpClient.SendAsync(request);

            var responseString = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<BaseResponse>(responseString) ?? throw new ArgumentNullException(nameof(responseString));

        }

        /// <summary>
        /// Sends bulk text messages to multiple contacts
        /// </summary>
        /// <param name="messageRequest"></param>
        /// <returns></returns>
        public async Task<BaseResponse> SendBulkMessages(BulkMessageRequest messageRequest)
        {
            var request = new HttpRequestMessage();

            request.RequestUri = new Uri($"{BASE_ENDPOINT_URL}/send");

            request.Method = HttpMethod.Post;

            var token = connection.Token;

            request.Headers.Add($"Authorization", $"Basic {token}");

            var bodyString = new
            {
                from = connection.SenderId,
                recipients = messageRequest.Recipients,
                msg = messageRequest.Msg
            };

            var content = new StringContent(JsonConvert.SerializeObject(bodyString),
                Encoding.UTF8, "application/json");

            request.Content = content;

            var response = await _httpClient.SendAsync(request);

            var responseString = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<BaseResponse>(responseString) ?? throw new ArgumentNullException(nameof(responseString));
        }


        /// <summary>
        /// Send text messages to a single contact. 
        /// This method uses Basic Authorization header
        /// </summary>
        /// <param name="messageRequest"></param>
        /// <returns></returns>
        public async Task<SingleSmsResponse> SendMessageWithToken(SingleMessageRequest messageRequest)
        {
            var request = new HttpRequestMessage();

            request.RequestUri = new Uri($"{BASE_ENDPOINT_URL}/send");

            request.Method = HttpMethod.Post;

            var token = connection.Token;

            request.Headers.Add($"Authorization", $"Basic {token}");

            var bodyString = new
            {
                from = connection.SenderId,
                to = messageRequest.To,
                msg = messageRequest.Msg
            };

            var content = new StringContent(JsonConvert.SerializeObject(bodyString),
                Encoding.UTF8, "application/json");

            request.Content = content;

            var response = await _httpClient.SendAsync(request);

            var responseString = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<SingleSmsResponse>(responseString) ?? throw new ArgumentNullException(nameof(responseString));
        }


        /// <summary>
        /// Send text messages to a single contact
        /// </summary>
        /// <param name="to"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task<SingleSmsResponse> SendSingleMessage(string to, string message)
        {
            var username = connection.Username;

            var password = connection.Password;

            var from = connection.SenderId;

            var response = await _httpClient.GetAsync($"{BASE_ENDPOINT_URL}/send?username={username}&password={password}&from={from}&to={to}&msg={message}");

            var responseString = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<SingleSmsResponse>(responseString) ?? throw new ArgumentNullException(nameof(responseString));
        }

        
    }
    
}
