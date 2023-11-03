using Microsoft.AspNetCore.SignalR;

namespace SignalRChat.Hubs
{
    public class ChatHub : Hub<IChatClient>
    {
        #region 處理連線事件
        /// <summary>
        /// 處理連線事件(連上線時)
        /// </summary>
        /// <returns></returns>
        public override async Task OnConnectedAsync()
        {
            //新增該使用者至某個群組
            //await Groups.AddToGroupAsync(Context.ConnectionId, "SignalR Users");

            //回傳目前連線Id
            await Clients.Caller.GetConnectionId(Context.ConnectionId);
            await base.OnConnectedAsync();
        }
        /// <summary>
        /// 處理連線事件(斷線時)
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await base.OnDisconnectedAsync(exception);
        }

        #endregion
        /// <summary>
        /// 目前連線者全體廣播
        /// </summary>
        /// <param name="user"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.ReceiveMessage(user, message);
        }
        /// <summary>
        /// 傳訊息至呼叫者
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="user"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task SendMessageToCaller(string userId, string user, string message)
        {

            await Clients.Caller.ReceiveMessage(user, message);
        }

        /// <summary>
        /// 傳送至特定對象
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="user"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task SendMessageToSpec(string userId,string userName, string message)
        {
            await Clients.Client(userId).SendSpecOne(userName,message);
        }
        /// <summary>
        /// 特定多人傳訊息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="user"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task SendMessageToOthers(string[] userIds, string userName, string message)
        {
            await Clients.Clients(userIds).ReceiveMessage(userName, message);
        }


        /// <summary>
        /// 傳訊息到某個群組
        /// </summary>
        /// <param name="user"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task SendMessageToGroup(string user, string message)
        {
            
            await Clients.Group("SignalR Users").ReceiveMessage(user, message);
        }

        /// <summary>
        /// 錯誤處理
        /// </summary>
        /// <returns></returns>
        /// <exception cref="HubException"></exception>
        public Task ThrowException()
        => throw new HubException("This error will be sent to the client!");
    }
}