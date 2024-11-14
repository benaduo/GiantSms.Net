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
        public async Task<ActionResult<object>> SendSms([FromQuery] string phoneNumber,
            [FromQuery] string message)
        {
            var result = await _giantSmsService.SendSingleMessage(phoneNumber, message);
            return Ok(result);
        }

        [HttpPost("bulk-sms")]
        public async Task<ActionResult<object>> SendBulkSms([FromBody] BulkMessageRequest request)
        {
            var result = await _giantSmsService.SendBulkMessages(new BulkMessageRequest
            {
                Msg = request.Msg,
                Recipients = request.Recipients
            });

            return Ok(result);
        }

        [HttpGet("balance")]
        public async Task<ActionResult<object>> GetBalance()
        {
            var balance = await _giantSmsService.GetBalance();
            return Ok(balance);
        }


        [HttpGet("sender-id")]
        public async Task<ActionResult<object>> GetSenderId()
        {
            var result = await _giantSmsService.GetSenderIds();
            return Ok(result);
        }

        [HttpPost("register-sender-id")]
        public async Task<ActionResult<object>> RegisterSenderId([FromBody] RegisterSenderIdRequest request)
        {
            var result = await _giantSmsService.RegisterSenderId(new RegisterSenderIdRequest
            {
                Name = "GiantSMS",
                Purpose = "SMS provider",
            });
            return Ok(result);
        }
        
        [HttpGet("message-status/{messageId}")]
        public async Task<ActionResult<object>> GetMessageStatus([FromRoute] string messageId)
        {
            var result = await _giantSmsService.CheckMessageStatus(messageId);
            return Ok(result);
        }
        
        [HttpPost("send-message-with-token")]
        public async Task<ActionResult<object>> SendMessageWithToken([FromBody] SingleMessageRequest request)
        {
            var result = await _giantSmsService.SendMessageWithToken(new SingleMessageRequest
            {
                Msg = request.Msg,
                To = request.To
            });
            return Ok(result);
        }
    }
}