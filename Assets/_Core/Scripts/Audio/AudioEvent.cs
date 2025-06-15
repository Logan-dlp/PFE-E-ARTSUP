using FMODUnity;

public class AudioEvent
{
    public EventReference EventRef { get; }

    public AudioEvent(EventReference eventRef)
    {
        EventRef = eventRef;
    }
}