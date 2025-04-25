namespace RoomManager.API.Helpers
{
    public static class ServiceHelper
    {
        public static readonly string ENVIRONMENT = nameof(ENVIRONMENT);
        public static readonly string SERVICE_PATH = nameof(SERVICE_PATH);
        public static readonly string SERVICE_PORT = nameof(SERVICE_PORT);
        public static readonly string CONSUL_HOST = nameof(CONSUL_HOST);
        public static readonly string CONSUL_PORT = nameof(CONSUL_PORT);
        public static readonly string CONSUL_SCHEME = nameof(CONSUL_SCHEME);
        public static readonly string CONSUL_PATH = nameof(CONSUL_PATH);
    }

    public enum ModuleEnum
    {
        Home = 1,
        User = 2,
        Asset = 3,
        Job = 4,
        Schedule = 5,
        Sales = 6,
        Inventory = 7,
    }
}
