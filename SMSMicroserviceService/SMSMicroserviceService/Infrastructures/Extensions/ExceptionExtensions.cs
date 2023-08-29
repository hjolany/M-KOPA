namespace SMSMicroService.Infrastructures.Extensions
{
    public static class ExceptionExtensions
    {
        public static string? GetFullMessage(this Exception? ex)
        {
            if (ex == null)
            {
                return "Exception message is empty";
            }
            var innerException = ex.InnerException?.GetFullMessage();
            return string.IsNullOrEmpty(innerException) ? $"{ex.Message}" : $"{ex.Message}: {innerException}";
        }
        public static string? ToString(this Exception? ex)
        {
            if (ex == null)
            {
                return "Exception message is empty";
            }
            var innerException = ex.InnerException?.GetFullMessage();
            return string.IsNullOrEmpty(innerException) ? $"{ex.Message}" : $"{ex.Message}: {innerException}";
        }
    }
}
