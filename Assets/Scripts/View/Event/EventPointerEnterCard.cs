
public class EventPointerEnterCard : EventContext
{
    public CardView cardView;
}

public class EventUpdatePointer : EventContext
{
    public EPointerState state;
    public ICard card;
}

public enum EPointerState
{
    Default,
    Hidden,
    CardSelect,
}