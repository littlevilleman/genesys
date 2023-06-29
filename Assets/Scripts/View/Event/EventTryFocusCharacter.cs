using System.Collections.Generic;

public class EventFocusCharacter : EventContext
{
    //public ICard card;
    //public ICharacter source;
    public List<ICharacter> availableTargets = new();
    public List<ICharacter> targets = new ();
}