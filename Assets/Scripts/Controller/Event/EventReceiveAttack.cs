public class EventReceiveDamage : EventContext
{
    public ICharacter source;
    public ICharacter target;
    public int damage;
}