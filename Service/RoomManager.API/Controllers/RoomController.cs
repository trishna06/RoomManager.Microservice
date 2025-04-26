using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using RoomManager.Application.Commands.DataTransferObjects;
using RoomManager.Application.Commands.RoomAggregate;
using RoomManager.Application.Helpers;
using RoomManager.Application.Queries;

namespace RoomManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IRoomQueries _queries;
        private readonly KafkaProducerHelper _kafkaProducer;

        public RoomController(IMediator mediator, IRoomQueries queries, KafkaProducerHelper kafkaProducer)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _queries = queries ?? throw new ArgumentNullException(nameof(queries));
            _kafkaProducer = kafkaProducer ?? throw new ArgumentNullException(nameof(kafkaProducer));
        }

        [HttpGet]
        public async Task<IActionResult> GetRoomsAsync()
        {
            return Ok(await _queries.GetAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoomByIdAsync(int id)
        {
            return Ok(await _queries.GetAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> CreateRoomAsync([FromBody] CreateRoomCommand command)
        {
            int id = await _mediator.Send(command);
            return Ok(id);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRoomAsync(int id, [FromBody] UpdateRoomCommand command)
        {
            command.Id = id;
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoomAsync(int id)
        {
            await _mediator.Send(new DeleteRoomCommand(id));
            return NoContent();
        }

        [HttpPost("Producer")]
        public async Task<IActionResult> ProducerAsync([FromBody] RoomDto room)
        {
            await _kafkaProducer.ProduceAsync(room);

            return Ok("Message sent to Kafka!");
        }
    }
}
