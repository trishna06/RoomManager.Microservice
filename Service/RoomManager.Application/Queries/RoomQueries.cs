using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using ContentManager.Application.Queries.Specifications.ContentAggregate;
using Microservice.Utility.Domain.SeedWork;
using RoomManager.Application.Queries.Models;
using RoomManager.Domain.Aggregates.RoomAggregate;
using RoomManager.Domain.Exceptions;

namespace RoomManager.Application.Queries
{
    public class RoomQueries : IRoomQueries
    {
        private readonly IContextQuery _contextQuery;
        private readonly IMapper _mapper;
        public RoomQueries(IContextQuery contextQuery,
                        IMapper mapper)
        {
            _contextQuery = contextQuery ?? throw new ArgumentNullException(nameof(contextQuery));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<RoomModel> GetAsync(int id)
        {
            Room content = await _contextQuery.FirstOrDefaultAsync(new RoomByIdSpecification(id)) ?? throw new RoomNotFoundException(id);
            return _mapper.Map<RoomModel>(content);
        }

        public async Task<List<RoomModel>> GetAsync()
        {
            List<Room> content = await _contextQuery.FindAsync(new RoomSpecification());
            return _mapper.Map<List<RoomModel>>(content);
        }
    }
}
