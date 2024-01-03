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
            await Clients.Group(userRoomConnection.Room!).SendAsync(method: "ReceiveMessage", arg1: "Lets Program Bot", arg2: $"{userRoomConnection.User} has Joined the Group", arg3: DateTime.Now);
            await SendConnectedUser(userRoomConnection.Room!);
        }

        public async Task SendMessage(string message)
        {
            if(_connections.TryGetValue(Context.ConnectionId, out UserRoomConnection userRoomConnection))
            {
                await Clients.Groups(userRoomConnection.Room!).SendAsync(method: "ReceiveMessage", arg1: userRoomConnection.User, arg2: message, arg3: DateTime.Now);
            }
        }

        public override Task OnDisconnectedAsync(Exception? exp)
        {
            if (!_connections.TryGetValue(Context.ConnectionId,
                out UserRoomConnection roomConnection))
            {
                return base.OnDisconnectedAsync(exp);
            }
            Clients.Group(roomConnection.Room!).SendAsync(method:
                "ReceiveMessage", arg1: "Lets Program bot", arg2: $"{roomConnection.User} has left the Group", arg3: DateTime.Now);
            SendConnectedUser(roomConnection.Room!);
            return base.OnDisconnectedAsync(exp);
        }

        public Task SendConnectedUser(string room)
        {
            var users = _connections.Values
                .Where(u => u.Room == room).Select(s => s.User);
            return Clients.Group(room).SendAsync(method: "ConnectedUser", users);
        }
    }
}
