namespace MiniGolf.Event
{
    public class EventService
    {
        public EventController OnMouseDown { get; private set; }
        public EventController OnMouseNormal { get; private set; }
        public EventController OnMouseUp { get; private set; }
        public EventController OnPause { get; private set; }

        public EventService()
        {
            OnMouseDown = new EventController();
            OnMouseNormal = new EventController();
            OnMouseUp = new EventController();
            OnPause = new EventController();
        }
    }
}