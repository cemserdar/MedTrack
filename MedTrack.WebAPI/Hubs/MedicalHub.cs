using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace MedTrack.WebAPI.Hubs
{
    [Authorize]
    public class MedicalHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            var username = Context.User?.Identity?.Name ?? Context.ConnectionId;
            await Clients.Others.SendAsync("UserConnected", username);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var username = Context.User?.Identity?.Name ?? Context.ConnectionId;
            await Clients.Others.SendAsync("UserDisconnected", username);
            await base.OnDisconnectedAsync(exception);
        }

        /// <summary>
        /// Doktor grubuna katılım (Sadece o doktora ait bildirimleri dinlemek için)
        /// </summary>
        public async Task JoinDoctorGroup(string doctorId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"Doctor_{doctorId}");
        }

        /// <summary>
        /// Doktor grubundan ayrılma
        /// </summary>
        public async Task LeaveDoctorGroup(string doctorId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"Doctor_{doctorId}");
        }

        /// <summary>
        /// Hasta grubuna katılım
        /// </summary>
        public async Task JoinPatientGroup(string patientId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"Patient_{patientId}");
        }

        /// <summary>
        /// Hasta grubundan ayrılma
        /// </summary>
        public async Task LeavePatientGroup(string patientId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"Patient_{patientId}");
        }
    }
}
