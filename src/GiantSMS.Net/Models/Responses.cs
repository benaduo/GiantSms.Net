namespace GiantSms.Net.Model.Responses
{
    public class BaseResponse
    {
        public bool Status { get; init; }
        public string Message { get; init; }
    }
    public class MessageStatus
    {
        public string? Message_id { get; init; }
        public DateTime? Scheduled_date { get; init; }
        public int? Rate { get; init; }
        public string? Status { get; init; }
        public string? Reason { get; init; }
        public DateTime? Last_updated_date { get; init; }
    }
    public class SenderIdData
    {
        public string Name { get; set; }
        public string Purpose { get; set; }
        public bool Approved { get; set; }
        public string Approval_status { get; set; }
    }

    public class SingleSmsResponse : BaseResponse
    {
        public MessageStatus? Data { get; init; }
    }

    public class SenderIdResponse : BaseResponse
    {
        public List<SenderIdData> Data { get; init; }
    }
}
