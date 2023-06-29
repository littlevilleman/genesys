using System.Collections.Generic;

public interface ICharacter
{
    public void PlayCard(ICard card, ICharacter target);
    public void ReceiveDamage(ICharacter source, int damage);
    public void ReceiveHeal(ICharacter source, int heal);

    public CharacterConfig GetConfig();
    public CharacterState GetState();
    public Formation GetFormation();
    public EFaction GetFaction();
    public List<ModifierEffect> GetModifierEffects();
    public int GetFormationLocation();
    public int GetMaxLife();
    public bool IsAlive();
}
