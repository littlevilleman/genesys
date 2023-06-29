using System.Collections.Generic;

public class Card<T> : ICard where T : CardConfig
{
    private T config;
    private List<ICardAction> actions = new ();

    public CardConfig GetConfig() => config;

    public void Setup(T cardConfig)
    {
        config = cardConfig;

        foreach (CardActionConfig actionConfig in config.actions)
        {
            actions.Add(actionConfig.BuildAction());
        }
    }

    public async void Play(ICharacter source, List<ICharacter> targets)
    {
        foreach (ICardAction cardAction in actions)
        {
            foreach (ICharacter target in targets)
            {
                await cardAction.Launch(source, target);
            }
        }
    }

    public FactionLocationSet[] GetTargetLocations(ICharacter source)
    {
        switch (config.targetType)
        {
            case ETargetType.TARGET:
                return config.targetLocations;
            case ETargetType.SELF:
                return FactionLocationSet.GetLocationByIndex(source.GetFaction(), source.GetFormationLocation());
            case ETargetType.ALLY_TEAM:
                return FactionLocationSet.AllyTeam;
            case ETargetType.ENEMY_TEAM:
                return FactionLocationSet.EnemyTeam;
            case ETargetType.GLOBAL:
                return FactionLocationSet.Global;
        }

        return null;
    }

    public int GetManaCost()
    {
        return config.manaCost;
    }
}

public class Card : Card<CardConfig>
{
}