using System.Collections.Generic;

public class EventPlayCardStart : EventContext
{
    public ICard card;
    public ICharacter source;
    public ICharacter focus;
    //public List<ICharacter> targets = new ();
}
