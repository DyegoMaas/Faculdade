namespace SmartOutlet.Outlet.Mqtt
{
    public interface IPublisher
    {
        void Publish(string topic, string message);
    }
}