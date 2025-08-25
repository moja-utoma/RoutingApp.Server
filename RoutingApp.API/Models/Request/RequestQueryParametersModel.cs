using RoutingApp.API.Enumerations;

namespace RoutingApp.API.Models;

public class FromQueryParametersModel
{
	public string OrderBy { get; set; } = "Name";
	public bool IsDesc { get; set; } = false;
	public string SearchString { get; set; } = "";
	public int Page { get; set; } = 1;
	public int PageSize { get; set; } = 10;
	//public PointType PointType { get; set; } = PointType.Warehouse;
}
