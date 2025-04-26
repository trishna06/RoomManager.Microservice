using System.Threading;
using System.Threading.Tasks;
using MediatR;
using RoomManager.Domain.Aggregates.RoomAggregate;
using RoomManager.Domain.Repositories;

namespace RoomManager.Application.Commands.RoomAggregate
{
    public class CreateRoomCommand : IRequest<int>
    {
        public string Number { get; set; }
        public string Type { get; set; }
    }
    public class CreateRoomCommandCommandHandler : IRequestHandler<CreateRoomCommand, int>
    {
        private readonly IRoomRepository _repository;

        public CreateRoomCommandCommandHandler(IRoomRepository repository)
        {
            _repository = repository;
        }
        public async Task<int> Handle(CreateRoomCommand command, CancellationToken cancellationToken)
        {
            Room room = new Room(command.Number, command.Type);
            await _repository.AddAsync(room);
            await _repository.UnitOfWork.SaveEntitiesAsync();
            return room.Id;
        }
    }
}
