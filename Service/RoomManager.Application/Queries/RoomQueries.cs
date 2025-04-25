using System;
using AutoMapper;
using Microservice.Utility.Domain.SeedWork;


namespace ContentManager.Application.Queries
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
    }
}
