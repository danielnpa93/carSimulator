using DriverService.API.Domain.Entities;

namespace DriverService.API.Domain.DTO
{
    public abstract class BaseRouteDTO
    {
        public virtual string Title { get; set; }
        public virtual Position StartPosition { get; set; }
        public virtual Position EndPosition { get; set; }
    }
}
