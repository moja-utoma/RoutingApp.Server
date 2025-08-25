using FluentValidation;
using RoutingApp.API.Models.DTO;

namespace RoutingApp.API.Validation
{
	public class RouteValidator : AbstractValidator<CreateRouteRequest>
	{
		public RouteValidator()
		{
			RuleFor(r => r.DeliveryPointIds)
				.NotNull()
				.NotEmpty()
				.Must(p => p.Count() >= 1)
				.WithMessage("A route must have at least 1 delivery point.");


			RuleFor(r => r.WarehouseIds)
				.NotNull()
				.NotEmpty()
				.Must(p => p.Count() >= 1)
				.WithMessage("A route must have at least 1 warehouse.");
		}
	}
}
