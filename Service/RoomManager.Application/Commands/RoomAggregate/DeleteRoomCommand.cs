using System.Threading;
using System.Threading.Tasks;
using MediatR;
using RoomManager.Domain.Aggregates.RoomAggregate;
using RoomManager.Domain.Exceptions;
using RoomManager.Domain.Repositories;

namespace RoomManager.Application.Commands.RoomAggregate
{
    public class DeleteRoomCommand : IRequest
    {
        public int Id { get; set; }

        public DeleteRoomCommand(int id)
        {
            Id = id;
        }
    }
    public class DeleteRoomCommandCommandHandler : IRequestHandler<DeleteRoomCommand>
    {
        private readonly IRoomRepository _repository;

        public DeleteRoomCommandCommandHandler(IRoomRepository repository)
        {
            _repository = repository;
        }
        public async Task Handle(DeleteRoomCommand command, CancellationToken cancellationToken)
        {
            Room room = await _repository.GetAsync(command.Id) ?? throw new RoomNotFoundException(command.Id);
            room.Delete();
            await _repository.UnitOfWork.SaveEntitiesAsync();
        }
    }
}
