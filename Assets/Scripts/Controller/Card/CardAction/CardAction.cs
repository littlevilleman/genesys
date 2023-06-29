using System.Threading.Tasks;

public abstract class CardAction<T> : ICardAction where T : CardActionConfig
{
    public T config;
    public EFaction TargetFaction;
    public int[] Targets;

    public abstract void Setup(T actionConfig);

    public abstract Task Launch(ICharacter source, ICharacter target);
}
public abstract class CardAction : CardAction<CardActionConfig>
{

}