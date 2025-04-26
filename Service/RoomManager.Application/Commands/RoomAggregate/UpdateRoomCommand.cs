using System.Threading;
using System.Threading.Tasks;
using MediatR;
using RoomManager.Application.Commands.DataTransferObjects;
using RoomManager.Domain.Aggregates.RoomAggregate;
using RoomManager.Domain.Exceptions;
using RoomManager.Domain.Repositories;

namespace RoomManager.Application.Commands.RoomAggregate
{
    public class UpdateRoomCommand : IRequest
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public string Type { get; set; }
        public RoomAvailabilityDto Availability { get; set; }
    }
    public class UpdateRoomCommandCommandHandler : IRequestHandler<UpdateRoomCommand>
    {
        private readonly IRoomRepository _repository;

        public UpdateRoomCommandCommandHandler(IRoomRepository repository)
        {
            _repository = repository;
        }
        public async Task Handle(UpdateRoomCommand command, CancellationToken cancellationToken)
        {
            Room room = await _repository.GetAsync(command.Id) ?? throw new RoomNotFoundException(command.Id);

            room.Update(command.Number, command.Type);
            if (command.Availability != null)
            {
                room.UpdateAvailability(command.Availability.Type, command.Availability.Status);
            }
            await _repository.UnitOfWork.SaveEntitiesAsync();
        }
    }
}
