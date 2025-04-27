using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using RoomManager.Application.Queries.Models;
namespace RoomManager.Application.Services
{
    public interface ISqlStreamService
    {
        Task<List<RoomAvailabilityModel>> GetAvailableRoomsAsync();
    }

    public class SqlStreamService: ISqlStreamService
    {
        private readonly HttpClient _httpClient;

        public SqlStreamService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<RoomAvailabilityModel>> GetAvailableRoomsAsync()
        {
            var query = new
            {
                sql = "SELECT RoomId, Status, UpdatedDateTime FROM ROOM_AVAILABILITY_STREAM;",
                properties = new Dictionary<string, string>
                {
                    { "auto.offset.reset", "earliest" }
                }
            };

            StringContent content = new StringContent(JsonSerializer.Serialize(query), Encoding.UTF8, "application/json");

            using CancellationTokenSource cts = new CancellationTokenSource();
            cts.CancelAfter(TimeSpan.FromMinutes(5)); // auto-cancel if stuck

            using HttpResponseMessage response = await _httpClient.PostAsync("http://localhost:8088/query-stream", content, cts.Token);
            response.EnsureSuccessStatusCode();

            using Stream stream = await response.Content.ReadAsStreamAsync();
            using StreamReader reader = new StreamReader(stream);

            List<RoomAvailabilityModel> roomList = new List<RoomAvailabilityModel>();

            while (!reader.EndOfStream)
            {
                var line = await reader.ReadLineAsync();

                if (!string.IsNullOrWhiteSpace(line))
                {
                    JsonDocument jsonDoc = JsonDocument.Parse(line);

                    if (jsonDoc.RootElement.ValueKind == JsonValueKind.Array)
                    {
                        JsonElement columns = jsonDoc.RootElement;

                        RoomAvailabilityModel roomData = new RoomAvailabilityModel
                        {
                            RoomId = columns[0].GetString(),
                            Status = columns[1].GetString(),
                            UpdatedDateTime = columns[2].GetString()
                        };

                        roomList.Add(roomData);
                    }
                }
            }

            return roomList;
        }
    }
}
