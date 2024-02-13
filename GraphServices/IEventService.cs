namespace Test1.GraphServices
{
    public interface IEventService
    {
        Task<string> CreateEventAsync(string subject, DateTime start, DateTime end);
    }
}
