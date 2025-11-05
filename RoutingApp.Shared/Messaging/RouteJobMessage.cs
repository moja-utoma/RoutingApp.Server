namespace RoutingApp.Shared.Messaging
{
    public class RouteJobMessage
    {
        public int RouteId { get; set; }
        public string CorrelationId { get; set; } = default!;
        public string RequestedBy { get; set; } = "system";
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public string ReplyTo { get; set; } = "";
	}
}
