using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using RoomManager.Application.Commands.DataTransferObjects;
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
        public IActionResult GetRooms()
        {
            List<object> rooms = new List<object>
            {
                new { Id = 1, Name = "Room A", Capacity = 10 },
                new { Id = 2, Name = "Room B", Capacity = 20 }
            };
            return Ok(rooms);
        }

        [HttpGet("{id}")]
        public IActionResult GetRoomById(int id)
        {
            var room = new { Id = id, Name = $"Room {id}", Capacity = 15 };
            return Ok(room);
        }

        [HttpPost]
        public IActionResult CreateRoom([FromBody] object roomDto)
        {
            // Assume creation successful
            return CreatedAtAction(nameof(GetRoomById), new { id = 123 }, roomDto);
        }

        // PUT: api/Room/5
        [HttpPut("{id}")]
        public IActionResult UpdateRoom(int id, [FromBody] object roomDto)
        {
            // Assume update successful
            return NoContent();
        }

        // DELETE: api/Room/5
        [HttpDelete("{id}")]
        public IActionResult DeleteRoom(int id)
        {
            // Assume deletion successful
            return NoContent();
        }

        [HttpPost("Producer")]
        public async Task<IActionResult> ProducerAsync([FromBody] RoomAvailabilityDto room)
        {
            await _kafkaProducer.ProduceAsync(room);

            return Ok("Message sent to Kafka!");
        }
    }
}
