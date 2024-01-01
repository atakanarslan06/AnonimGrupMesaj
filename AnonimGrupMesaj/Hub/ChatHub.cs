using Microsoft.AspNetCore.SignalR;

namespace AnonimGrupMesaj.Hub
{
    public class ChatHub : Microsoft.AspNetCore.SignalR.Hub
    {
        public async Task JoinRoom(UserRoomConnection userRoomConnection)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName: userRoomConnection.Room!);
            await Clients.Group(userRoomConnection.Room!).SendAsync(method: "ReceiveMessage", arg1: "Lets Program Bot", arg2: $"{userRoomConnection.User} has Joined the Group");
        }
    }
}
