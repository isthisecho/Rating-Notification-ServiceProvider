namespace HomeRun.RatingService.Models
{
    public class CommonResponse
    {
        public object? Result { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
