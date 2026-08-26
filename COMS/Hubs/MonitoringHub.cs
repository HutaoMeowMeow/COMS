using COMS.DTOs;
using COMS.Models;
using COMS.Services;
using Microsoft.AspNetCore.SignalR;

namespace COMS.Hubs;

public class MonitoringHub : Hub
{
    private readonly ISensorService _sensorService;
    private readonly IAlertService _alertService;

    public MonitoringHub(ISensorService sensorService, IAlertService alertService)
    {
        _sensorService = sensorService;
        _alertService = alertService;
    }

    public async Task SubscribeToCanal(Guid canalId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, $"canal-{canalId}");
        await Clients.Caller.SendAsync("Subscribed", $"Subscribed to canal {canalId}");
    }

    public async Task UnsubscribeFromCanal(Guid canalId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"canal-{canalId}");
        await Clients.Caller.SendAsync("Unsubscribed", $"Unsubscribed from canal {canalId}");
    }

    public override async Task OnConnectedAsync()
    {
        await Clients.Caller.SendAsync("Connected", "Connected to COMS Monitoring Hub");
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await Clients.Caller.SendAsync("Disconnected", "Disconnected from COMS Monitoring Hub");
        await base.OnDisconnectedAsync(exception);
    }
}
