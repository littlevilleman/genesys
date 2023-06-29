using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Character : ICharacter
{
    protected EFaction faction;
    protected CharacterConfig config;
    protected CharacterStats stats = new();
    protected CharacterState state;
    protected Formation formation;
    protected int formationLocation;

    public Formation GetFormation() => formation;
    public CharacterConfig GetConfig() => config;
    public CharacterState GetState() => state;
    public List<ModifierEffect> GetModifierEffects() => state.Effects;
    public EFaction GetFaction () => faction;
    public int GetFormationLocation () => formationLocation;
    public int GetMaxLife() => state.LifeBase;
    public bool IsAlive() => state.Life > 0;



    public Character(CharacterConfig characterConfig, Formation formationSetup, int locationSetup, EFaction factionSetup)
    {
        formation = formationSetup;
        config = characterConfig;
        formationLocation = locationSetup;
        faction = factionSetup;

        stats = config.baseStats;
        state = new CharacterState(stats);
    }

    public void ReceiveDamage(ICharacter source, int damage)
    {
        state.Life = Mathf.Clamp(state.Life - damage, 0, GetMaxLife());
        EventBus.Send(new EventReceiveDamage { source = source, target = this, damage = damage });

        if (state.Life == 0)
            EventBus.Send(new EventCharacterDie { character = this });
    }

    public void ReceiveHeal(ICharacter source, int heal)
    {
        state.Life = Mathf.Clamp(state.Life + heal, 0, GetMaxLife());
        EventBus.Send(new EventReceiveDamage { source = source, target = this, damage = heal });
    }

    public async void PlayCard(ICard card, ICharacter focus)
    {
        await Task.Delay(400);
        card.Play(this, BattleManager.Instance.GetTargets(card, this, focus));
    }
}