using Microsoft.AspNetCore.SignalR;

namespace AnonimGrupMesaj.Hub
{
    public class ChatHub : Microsoft.AspNetCore.SignalR.Hub
    {
        private readonly IDictionary<string, UserRoomConnection> _connections;

        public ChatHub(IDictionary<string, UserRoomConnection> connections)
        {
            _connections = connections;
        }

        public async Task JoinRoom(UserRoomConnection userRoomConnection)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName: userRoomConnection.Room!);
            _connections[Context.ConnectionId] = userRoomConnection;
            await Clients.Group(userRoomConnection.Room!).SendAsync(method: "ReceiveMessage", arg1: "Lets Program Bot", arg2: $"{userRoomConnection.User} has Joined the Group");
        }

        public async Task SendMessage(string message)
        {
            if(_connections.TryGetValue(Context.ConnectionId, out UserRoomConnection userRoomConnection))
            {
                await Clients.Groups(userRoomConnection.Room!).SendAsync(method: "ReceiveMessage", arg1: userRoomConnection.User, arg2: message, arg3: DateTime.Now);
            }
        }
    }
}
