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

        // Constructor to initialize the IGiantSmsService
        public SmsController(IServiceProvider serviceProvider)
        {
            _giantSmsService = serviceProvider.GetService<IGiantSmsService>() ??
                               throw new ArgumentNullException(nameof(IGiantSmsService));
        }

        // Endpoint to send a single SMS
        [HttpPost("single-sms")]
        public async Task<ActionResult<object>> SendSms([FromQuery] string phoneNumber,
            [FromQuery] string message)
        {
            var result = await _giantSmsService.SendSingleMessage(phoneNumber, message);
            return Ok(result);
        }

        // Endpoint to send bulk SMS
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

        // Endpoint to get the balance
        [HttpGet("balance")]
        public async Task<ActionResult<object>> GetBalance()
        {
            var balance = await _giantSmsService.GetBalance();
            return Ok(balance);
        }

        // Endpoint to get sender IDs
        [HttpGet("sender-id")]
        public async Task<ActionResult<object>> GetSenderId()
        {
            var result = await _giantSmsService.GetSenderIds();
            return Ok(result);
        }

        // Endpoint to register a new sender ID
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
        
        // Endpoint to get the status of a message by its ID
        [HttpGet("message-status/{messageId}")]
        public async Task<ActionResult<object>> GetMessageStatus([FromRoute] string messageId)
        {
            var result = await _giantSmsService.CheckMessageStatus(messageId);
            return Ok(result);
        }
        
        // Endpoint to send a message with a token
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