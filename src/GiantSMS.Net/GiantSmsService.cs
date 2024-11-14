using GiantSms.Net.Model.Requests;
using GiantSms.Net.Model.Responses;
using GiantSms.Net.Interfaces;
using Newtonsoft.Json;
using System.Text;
using GiantSms.Net.Constants;
using Microsoft.Extensions.Options;

namespace GiantSms.Net
{
    public class GiantSmsService : IGiantSmsService
    {
        private static readonly string BaseEndpointUrl = AppConstants.BASE_URL;
        private readonly GiantSmsConnection _connection;
        private readonly HttpClient _httpClient;
        private bool _isReady;
        public bool IsReady => _isReady;

        public GiantSmsService(IHttpClientFactory httpClientFactory, IOptions<GiantSmsConnection> connection)
        {
            _connection = connection.Value;
            _httpClient = httpClientFactory.CreateClient();
            _isReady = !string.IsNullOrWhiteSpace(_connection.Token) &&
                       !string.IsNullOrWhiteSpace(_connection.Username);
        }

        /// <summary>
        /// Checks the delivery status and details of a message 
        /// </summary>
        /// <param name="messageId"></param>
        /// <returns></returns>
        public async Task<SingleSmsResponse> CheckMessageStatus(string messageId)
        {
            var request = new HttpRequestMessage();
            request.RequestUri = new Uri($"{BaseEndpointUrl}/status");
            request.Method = HttpMethod.Post;

            var token = _connection.Token;

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

            var apiResponse = JsonConvert.DeserializeObject<SingleSmsResponse>(responseString);
         
            return apiResponse;
        }

        /// <summary>
        /// Returns remaining SMS Credits
        /// </summary>
        /// <returns></returns>
        public async Task<BaseResponse> GetBalance()
        {
            var username = _connection.Username;
            var password = _connection.Password;

            var response =
                await _httpClient!.GetAsync($"{BaseEndpointUrl}/balance?username={username}&password={password}");

            var responseString = await response.Content.ReadAsStringAsync();

            var apiResponse = JsonConvert.DeserializeObject<BaseResponse>(responseString);

            return new BaseResponse
            {
                Status = apiResponse.Status,
                Message = apiResponse.Message
            };
        }

        /// <summary>
        /// Returns an array object of registered sender IDs
        /// </summary>
        /// <returns></returns>
        public async Task<SenderIdResponse> GetSenderIds()
        {
            var username = _connection.Username;
            var password = _connection.Password;

            var response =
                await _httpClient!.GetAsync($"{BaseEndpointUrl}/sender?username={username}&password={password}");

            var responseString = await response.Content.ReadAsStringAsync();

            var apiResponse = JsonConvert.DeserializeObject<SenderIdResponse>(responseString);

            return new SenderIdResponse
            {
                Status = apiResponse.Status,
                Message = apiResponse.Message,
                Data = apiResponse.Data
            };
        }

        /// <summary>
        /// Registers a sender ID subject to approval.
        /// </summary>
        /// <param name="senderIdRequest"></param>
        /// <returns></returns>
        public async Task<BaseResponse> RegisterSenderId(RegisterSenderIdRequest senderIdRequest)
        {
            var request = new HttpRequestMessage();
            request.Method = HttpMethod.Post;

            var username = _connection.Username;
            var password = _connection.Password;

            request.RequestUri = new Uri($"{BaseEndpointUrl}/sender/register?username={username}&password={password}");

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

            var apiResponse = JsonConvert.DeserializeObject<BaseResponse>(responseString);

            return new BaseResponse
            {
                Status = apiResponse.Status,
                Message = apiResponse.Message
            };
        }

        /// <summary>
        /// Sends bulk text messages to multiple contacts
        /// </summary>
        /// <param name="messageRequest"></param>
        /// <returns></returns>
        public async Task<BaseResponse> SendBulkMessages(BulkMessageRequest messageRequest)
        {
            var request = new HttpRequestMessage();
            request.RequestUri = new Uri($"{BaseEndpointUrl}/send");
            request.Method = HttpMethod.Post;

            var token = _connection.Token;

            request.Headers.Add($"Authorization", $"Basic {token}");

            var bodyString = new
            {
                from = _connection.SenderId,
                recipients = messageRequest.Recipients,
                msg = messageRequest.Msg
            };

            var content = new StringContent(JsonConvert.SerializeObject(bodyString),
                Encoding.UTF8, "application/json");

            request.Content = content;

            var response = await _httpClient.SendAsync(request);

            var responseString = await response.Content.ReadAsStringAsync();

            var apiResponse = JsonConvert.DeserializeObject<BaseResponse>(responseString);

            return new BaseResponse
            {
                Status = apiResponse.Status,
                Message = apiResponse.Message
            };
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
            request.RequestUri = new Uri($"{BaseEndpointUrl}/send");
            request.Method = HttpMethod.Post;

            var token = _connection.Token;

            request.Headers.Add($"Authorization", $"Basic {token}");

            var bodyString = new
            {
                from = _connection.SenderId,
                to = messageRequest.To,
                msg = messageRequest.Msg
            };

            var content = new StringContent(JsonConvert.SerializeObject(bodyString),
                Encoding.UTF8, "application/json");

            request.Content = content;

            var response = await _httpClient.SendAsync(request);

            var responseString = await response.Content.ReadAsStringAsync();

            var apiResponse = JsonConvert.DeserializeObject<SingleSmsResponse>(responseString);

            return apiResponse;
        }


        /// <summary>
        /// Send text messages to a single contact
        /// </summary>
        /// <param name="to"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task<SingleSmsResponse> SendSingleMessage(string to, string msg)
        {
            var username = _connection.Username;
            var password = _connection.Password;
            var from = _connection.SenderId;

            var response = await _httpClient.GetAsync(
                $"{BaseEndpointUrl}/send?username={username}&password={password}&from={from}&to={to}&msg={msg}");
            var responseString = await response.Content.ReadAsStringAsync();

            var apiResponse = JsonConvert.DeserializeObject<SingleSmsResponse>(responseString);

            return apiResponse;
        }
    }
}