namespace SmartOutlet.Outlet.EventSourcing.Events
{
    public class EventDescription
    {
        public long Sequence { get; }
        public string Title { get; set; }
        public string TimeStamp { get; set; }
        public string Description { get; set; }

        public EventDescription(long sequence, string title, string timeStamp, string description)
        {
            Sequence = sequence;
            Title = title;
            TimeStamp = timeStamp;
            Description = description;
        }

        protected EventDescription()
        {
        }
    }
}