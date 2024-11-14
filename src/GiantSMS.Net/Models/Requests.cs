namespace GiantSms.Net.Model.Requests
{
    public class BaseRequest
    {
        public string Msg { get; set; }
    }

    public class SingleMessageRequest : BaseRequest
    {
        public string To { get; set; }
    }

    public class BulkMessageRequest : BaseRequest
    {
        public string[] Recipients { get; set; }
    }

    public class RegisterSenderIdRequest
    {
        public string Name { get; set; }
        public string Purpose { get; set; }
    }
}
