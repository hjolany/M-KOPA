namespace SMSMicroService.Infrastructures
{
    public class CriticalException : Exception
    {
        public CriticalException(string message) : base(message)
        {

        }
    }
}
