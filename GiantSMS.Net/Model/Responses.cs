using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiantSMS.Net.Model
{
    public class BaseResponse
    {
        public bool Status { get; set; }
        public string? Message { get; set; }
    }
    public class MessageStatus
    {
        public string? Message_id { get; set; }
        public DateTime Scheduled_date { get; set; }
        public int Rate { get; set; }
        public string? Status { get; set; }
        public string? Reason { get; set; }
        public DateTime Last_updated_date { get; set; }
    }
    public class SenderIdData
    {
        public string? Name { get; set; }
        public string? Purpose { get; set; }
        public bool Approved { get; set; }
        public string Approval_status { get; set; }
    }

    public class SingleSmsResponse : BaseResponse
    {
        public MessageStatus? Data { get; set; }
    }

    public class SenderIdResponse : BaseResponse
    {
        public List<SenderIdData>? Data { get; set; }
    }
}
