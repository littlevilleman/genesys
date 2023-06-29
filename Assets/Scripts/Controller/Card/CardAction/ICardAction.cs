using System.Threading.Tasks;

public interface ICardAction
{
    public Task Launch(ICharacter source, ICharacter target);
}
