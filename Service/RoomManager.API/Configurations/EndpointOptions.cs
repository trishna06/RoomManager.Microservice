using System.Collections.Generic;
using System.Linq;

namespace RoomManager.API.Configurations
{
    public class EndpointOptions
    {
        public List<Endpoint> Endpoints { get; set; }
        public Endpoint GetEndpoint(string name)
        {
            return Endpoints.Where(x => x.Name == name).FirstOrDefault();
        }
        public string GetEndpointUrl(string name)
        {
            Endpoint endpoint = Endpoints.Where(x => x.Name == name).FirstOrDefault();
            return $"{endpoint.Scheme}://{endpoint.Host}:{endpoint.Port}{endpoint.Path}";
        }
    }

    public class Endpoint
    {
        public string Name { get; set; }
        public string Host { get; set; }
        public string Port { get; set; }
        public string Path { get; set; }
        public string Scheme { get; set; }
    }
}
