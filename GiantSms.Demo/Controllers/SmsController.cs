using GiantSms.Net.Model.Responses;
using GiantSms.Net.Interfaces;
using Microsoft.AspNetCore.Mvc;
using GiantSms.Net.Model.Requests;

namespace GiantSms.Demo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SmsController : ControllerBase
    {
        private readonly IGiantSmsService _giantSmsService;

        public SmsController(IServiceProvider serviceProvider)
        {
            _giantSmsService = serviceProvider.GetService<IGiantSmsService>() ??
                throw new ArgumentNullException(nameof(IGiantSmsService));
        }


        [HttpPost("single-sms")]
        public async Task<ActionResult<SingleSmsResponse>> SendSms([FromQuery] string phoneNumber, [FromQuery] string message)
        {
            var result = await _giantSmsService.SendSingleMessage(phoneNumber, message);

            var responseData = result.Data != null ? new MessageStatus
            {
                Message_id = result.Data.Message_id,
                Scheduled_date = result.Data.Scheduled_date,
                Rate = result.Data.Rate,
                Status = result.Data.Status,
                Reason = result.Data.Reason,
                Last_updated_date = result.Data.Last_updated_date
            } : null;

            var response = new SingleSmsResponse { Status = result.Status, Message = result.Message, Data = responseData };

            return Ok(response);
        }

        [HttpPost("bulk-sms")]
        public async Task<ActionResult<SingleSmsResponse>> SendBulkSms([FromBody] BulkMessageRequest request)
        {
            var result = await _giantSmsService.SendBulkMessages(new BulkMessageRequest
            {
                Msg = request.Msg,
                Recipients = request.Recipients
            });

            var response = new SingleSmsResponse { Status = result.Status, Message = result.Message };

            return Ok(response);

        }

        [HttpGet("balance")]
        public async Task<ActionResult<BaseResponse>> GetBalance()
        {
            var balance = await _giantSmsService.GetBalance();
            return Ok(balance);
        }


        [HttpGet("sender-id")]
        public async Task<ActionResult<SenderIdResponse>> GetSenderId()
        {
            var result = await _giantSmsService.GetSenderIds();

            return Ok(new SenderIdResponse { Status = result.Status, Message = result.Message, Data = result.Data });
        }

    }
}

