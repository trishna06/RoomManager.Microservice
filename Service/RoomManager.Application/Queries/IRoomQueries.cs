using System.Collections.Generic;
using System.Threading.Tasks;
using RoomManager.Application.Queries.Models;

namespace RoomManager.Application.Queries
{
    public interface IRoomQueries
    {
        Task<RoomModel> GetAsync(int id);
        Task<List<RoomModel>> GetAsync();
    }
}
