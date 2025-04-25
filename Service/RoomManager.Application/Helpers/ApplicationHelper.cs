namespace RoomManager.Application.Helpers
{
    public static class ApplicationHelper
    {
        public static readonly string Namespace = typeof(ApplicationHelper).Namespace;
        public static readonly string ApplicationName = Namespace.Substring(0, Namespace.IndexOf(".")) + "Service";
    }
}
