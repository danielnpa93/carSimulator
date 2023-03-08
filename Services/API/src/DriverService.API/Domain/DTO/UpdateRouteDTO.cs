using DriverService.API.Domain.Entities;

namespace DriverService.API.Domain.DTO
{
    public class UpdateRouteDTO : BaseRouteDTO
    {
        public override string? Title { get; set; }
        public override Position? StartPosition { get; set; }
        public override Position? EndPosition { get; set; }
    }
}
