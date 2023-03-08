using GiantSms.Net.Model.Requests;
using GiantSms.Net.Model.Responses;

namespace GiantSms.Net.Interfaces
{
    public interface IGiantSmsService
    {
        Task<SingleSmsResponse> CheckMessageStatus(string messageId);
        Task<BaseResponse> GetBalance();
        Task<SenderIdResponse> GetSenderIds();
        Task<BaseResponse> RegisterSenderIds(RegisterSenderIdRequest senderIdRequest);
        Task<BaseResponse> SendBulkMessages(BulkMessageRequest messageRequest);
        Task<SingleSmsResponse> SendMessageWithToken(SingleMessageRequest messageRequest);
        Task<SingleSmsResponse> SendSingleMessage(string to, string message);
    }
}
