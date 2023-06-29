using System.Collections.Generic;

public interface ICard
{
    public FactionLocationSet[] GetTargetLocations(ICharacter source);
    public void Play(ICharacter source, List<ICharacter> targets);
    public int GetManaCost();

    public CardConfig GetConfig();
}
