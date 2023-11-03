namespace SignalRChat.Hubs
{
    public interface IChatClient
    {
        Task ReceiveMessage(string user, string message);

        Task GetConnectionId(string ConnectionId);
        Task SendSpecOne(string userName,string message);
    }
}
