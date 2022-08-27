using System;
using System.Threading.Tasks;
using kiosk_solution.Data.ViewModels;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace kiosk_solution.Business.Hubs
{
    public class SystemEventHub : Hub
    {
        public static string KIOSK_CONNECTION_CHANNEL = "KIOSK_CONNECTION_CHANNEL";
        public static string KIOSK_STATUS_CHANNEL = "KIOSK_STATUS_CHANNEL";
        public static string KIOSK_MESSAGE_CONNECTION_CHANNEL = "KIOSK_MESSAGE_CONNECTED_CHANNEL";
        public static string PARTY_NOTIFICATION_CONNECTION_CHANNEL = "PARTY_NOTIFICATION_CONNECTION_CHANNEL";
        public static string RELOAD_KIOSK_CHANNEL = "RELOAD_KIOSK_CHANNEL";
        public static string SYSTEM_BOT = "SYSTEM_BOT";

        public async Task JoinRoom(KioskConnectionViewModel kioskConnection)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, kioskConnection.RoomId);
            Console.WriteLine($"{kioskConnection.KioskId} has joined {kioskConnection.RoomId}");
            await Clients.Group(kioskConnection.KioskId)
                .SendAsync(KIOSK_MESSAGE_CONNECTION_CHANNEL,
                    SYSTEM_BOT, "Connected On Kiosk System Success");
        }
    }
}