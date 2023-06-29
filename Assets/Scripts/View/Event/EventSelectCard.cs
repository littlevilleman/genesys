public class EventSelectCard : EventContext
{
    public ICharacter source;
    public ICard card;
}

public class EventSelectCardResult : EventContext
{
    public ICard card;
    public Result result;

    public enum Result
    {
        Success = 0,
        InsufficentMana = 1
    }
}