namespace SubscriberExample.EventProcessing
{
    public interface IEventProcessor
    {
        void ProcessEvent(string message);
    }
}
