using Microsoft.AspNetCore.SignalR;

namespace MedTrack.WebAPI.Hubs
{
    public class MedicalHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            await Clients.All.SendAsync("UserConnected", Context.ConnectionId);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await Clients.All.SendAsync("UserDisconnected", Context.ConnectionId);
            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendNotification(string message)
        {
            await Clients.All.SendAsync("ReceiveNotification", message);
        }

        // Appointment notifications
        public async Task BroadcastAppointmentCreated(object appointmentData)
        {
            await Clients.All.SendAsync("AppointmentCreated", appointmentData);
        }

        public async Task BroadcastAppointmentUpdated(object appointmentData)
        {
            await Clients.All.SendAsync("AppointmentUpdated", appointmentData);
        }

        public async Task BroadcastAppointmentCancelled(string appointmentId)
        {
            await Clients.All.SendAsync("AppointmentCancelled", appointmentId);
        }

        // Medical note notifications
        public async Task BroadcastMedicalNoteAdded(object noteData)
        {
            await Clients.All.SendAsync("MedicalNoteAdded", noteData);
        }

        // Prescription notifications
        public async Task BroadcastPrescriptionCreated(object prescriptionData)
        {
            await Clients.All.SendAsync("PrescriptionCreated", prescriptionData);
        }

        public async Task BroadcastPrescriptionUpdated(object prescriptionData)
        {
            await Clients.All.SendAsync("PrescriptionUpdated", prescriptionData);
        }

        // Lab test notifications
        public async Task BroadcastLabTestResultAvailable(object testResultData)
        {
            await Clients.All.SendAsync("LabTestResultAvailable", testResultData);
        }

        // Imaging result notifications
        public async Task BroadcastImagingResultAvailable(object imagingResultData)
        {
            await Clients.All.SendAsync("ImagingResultAvailable", imagingResultData);
        }
    }
}
