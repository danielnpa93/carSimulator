namespace DriverService.API.Domain.DTO
{
    public class ResultViewModel
    {
        public string? DisplayMessage { get; set; }

        public IEnumerable<string>? Errors { get; set; }

        public bool Success { get; set; }
    }

    public class ResultViewModel<T> : ResultViewModel
    {
        public T Data { get; set; }

    }
}
