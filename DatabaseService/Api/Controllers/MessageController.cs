using Microsoft.AspNetCore.Mvc;
using Logic.Message.Interfaces;
using Api.Controllers.Messages.Requests;
using Logic.Message.Models;
using Api.Controllers.Messages.Responses;
using Api.Filters;


namespace Api.Controllers
{
    [ApiController]
    [Route("messages")]
    public class MessageController : ControllerBase
    {
        private readonly IMessageLogicManager _messageLogicManager;
        public MessageController(IMessageLogicManager messageManager)
        {
            _messageLogicManager = messageManager;
        }

        [HttpPost]
        [Route("store")]
        [ProducesResponseType(typeof(StoreMessageResponse), 200)]
        public async Task<IActionResult> Store([FromBody] StoreMessageRequest dto)
        {
            var id = await _messageLogicManager.CreateAsync(new MessageLogic
            {
                Id = dto.Id,
                Message = dto.Message,
                Channel = dto.Channel,
                Recipient = dto.Recipient,
                Subject = dto.Subject,
                Metadata = dto.Metadata
            });
            return Ok(new StoreMessageResponse()
            { 
                MessageId = id
            });
        }

        [HttpPost]
        [Route("status")]
        [DataNotFoundExceptionFilter]
        [ProducesResponseType(typeof(SetMessageStatusResponse), 200)]
        public async Task<IActionResult> SetStatus([FromBody] SetMessageStatusRequest dto)
        {
            await _messageLogicManager.UpdateStatusAsync(new MessageStatusLogic
            {
                MessageId = dto.Id,
                Status = dto.Status
            });
            return Ok(new SetMessageStatusResponse
            {
                Success = true 
            });
        }

        [HttpGet]
        [Route("message")]
        [DataNotFoundExceptionFilter]
        [ProducesResponseType(typeof(GetMessageResponse), 200)]
        public async Task<IActionResult> GetContent([FromQuery] GetMessageRequest dto)
        {
            var message = await _messageLogicManager.GetContent(dto.Id);

            return Ok(message);
        }

        [HttpGet]
        [Route("status")]
        [DataNotFoundExceptionFilter]
        [ProducesResponseType(typeof(GetCurrentStatusResponse), 200)]
        public async Task<IActionResult> GetCurrentStatus([FromQuery] GetCurrentStatusRequest dto)
        {
            var status = await _messageLogicManager.GetCurrentStatusAsync(dto.Id);

            return Ok(new GetCurrentStatusResponse
            {
                Status = status
            });
        }

        [HttpGet]
        [Route("status/history")]
        [DataNotFoundExceptionFilter]
        [ProducesResponseType(typeof(GetStatusHistoryResponse), 200)]
        public async Task<IActionResult> GetStatusHistory([FromQuery] GetStatusHistoryRequest dto)
        {
            var result = await _messageLogicManager.GetStatusHistoryAsync(dto.Id);

            return Ok(new GetStatusHistoryResponse
            {
                Entries = result.Entries
                    .Select(e => new GetStatusHistoryResponse.Entry
                    {
                        Status = e.Status,
                        TimeStamp = e.TimeStamp
                    })
                    .ToArray()
            });
        }
    }
}
