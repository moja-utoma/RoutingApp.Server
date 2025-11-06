using Microsoft.AspNetCore.SignalR;
using System.Text.RegularExpressions;

namespace RoutingApp.API.Services
{
	public class RouteHub : Hub
	{
		public async Task SubscribeToRoute(string correlationId)
		{
			await Groups.AddToGroupAsync(Context.ConnectionId, correlationId);
		}
	}
}
