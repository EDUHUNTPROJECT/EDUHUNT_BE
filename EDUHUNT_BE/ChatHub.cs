// ChatHub.cs
using Microsoft.AspNetCore.SignalR;
using EDUHUNT_BE.Model;
using EDUHUNT_BE.Data;
namespace EDUHUNT_BE
{

    public sealed class ChatHub : Hub
    {
        private readonly AppDbContext _context;

        public ChatHub(AppDbContext context)
        {
            _context = context;
        }

        public override async Task OnConnectedAsync()
        {
            await Clients.All.SendAsync("UserConnected say hello to ", Context.ConnectionId);
        }

        public async Task SendMessage(Message message)
        {
            try
            {
                if (message.Sender != null && message.Content != null && message.Receiver != null)
                {
                    var newMessage = new Message
                    {
                        Sender = message.Sender,
                        Receiver = message.Receiver,
                        Content = message.Content,
                    };

                    _context.Messages.Add(newMessage);
                    await _context.SaveChangesAsync();
                }
            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
           
          


            await Clients.All.SendAsync("ReceiveMessage", message);
        }
    }
}
