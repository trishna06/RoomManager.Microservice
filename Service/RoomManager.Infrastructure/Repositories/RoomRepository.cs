using System;
using System.Threading.Tasks;
using Microservice.Utility.Domain.SeedWork;
using Microsoft.EntityFrameworkCore;
using RoomManager.Domain.Exceptions;
using RoomManager.Domain.Repositories;

namespace RoomManager.Infrastructure.Repositories
{
    public class RoomRepository : IRoomRepository
    {
        private readonly RoomManagerContext _context;

        public RoomRepository(RoomManagerContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IUnitOfWork UnitOfWork => _context;

        public async Task<Domain.Aggregates.RoomAggregate.Room> AddAsync(Domain.Aggregates.RoomAggregate.Room aggregate)
        {
            if (await _context.Room.AnyAsync(j => j.Number == aggregate.Number))
                throw new RoomManagerExistedException(aggregate.Number);
            return _context.Room.Add(aggregate).Entity;
        }

        public Task<Domain.Aggregates.RoomAggregate.Room> GetAsync(int id)
        {
            return _context.Room.FirstOrDefaultAsync(j => j.Id == id);
        }
    }
}
