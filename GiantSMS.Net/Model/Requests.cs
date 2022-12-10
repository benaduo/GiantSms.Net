using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiantSMS.Net.Model
{
    public class BaseRequest
    {
        public string? Msg { get; set; }
    }

    public class SingleMessageRequest : BaseRequest
    {
        public string? To { get; set; }
    }

    public class BulkMessageRequest : BaseRequest
    {
        public string[]? Recipients { get; set; }
    }

    public class RegisterSenderIdRequest
    {
        public string? Name { get; set; }
        public string? Purpose { get; set; }
    }
}
