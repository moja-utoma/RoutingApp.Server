namespace RoutingApp.API.Models.Responses
{
    public class PaginatedResponseDTO<T>
    {
        public int PageIndex { get; set; }
        public int TotalPages { get; set; }
        public bool HasPreviousPage { get; set; }
        public bool HasNextPage { get; set; }
        public List<T> Items { get; set; }
    }
}
