using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IBattle
{
    void StartBattle();
    Task PlayCard(ICard card, ICharacter source, ICharacter target);
    void RemoveCharacter(ICharacter character);
    void EndTurn();
    void Surrender();
}

public class Battle : IBattle
{
    private List<Formation> formations = new();
    private List<List<ICharacter>> roundQueues = new(2) { new(), new() };
    private ETurnPhase turnPhase = ETurnPhase.Effects;
    private int round = 1;
    private int turn = 1;

    public List<ICharacter> CurrentRound => roundQueues[0];
    public List<ICharacter> NextRound => roundQueues[1];
    public ICharacter CurrentTurn => CurrentRound[0];
    public EBattleResult BattleResult => GetBattleResult();
    public List<Formation> Formations => formations;
    public Formation AllyFormation => formations[0];
    public Formation EnemyFormation => formations[1];

    #region Callbacks

    public Action<List<List<ICharacter>>> OnStartBattle;
    public Action<EBattleResult> OnEndBattle;
    public Action<int, ETurnPhase, ICharacter> OnStartTurnPhase;
    public Action<int, ICharacter> OnEndTurn;
    public Action<ICharacter> OnDrawRandomCard;
    public Action<ICard, ICharacter, ICharacter> OnPlayCardStart;
    public Action<ICard, ICharacter, ICharacter> OnPlayCardEnd;
    public Action<List<List<ICharacter>>> OnUpdateTurnQueue;

    #endregion

    #region Public

    public Battle(BattleConfig battleConfig)
    {
        SetupFormations(battleConfig.formations);
    }

    public void StartBattle()
    {
        turnPhase = ETurnPhase.Effects;
        round = 1;
        turn = 1;

        OnStartBattle?.Invoke(roundQueues);
        StartTurn();
    }

    public async Task PlayCard(ICard card, ICharacter source, ICharacter target)
    {
        source.PlayCard(card, target);
        OnPlayCardStart?.Invoke(card, source, target);
        await Task.Delay(1000);
        OnPlayCardEnd?.Invoke(card, source, target);
    }

    public List<ICharacter> GetAvailableTargets(ICard card, ICharacter source)
    {
        List<ICharacter> targets = new();
        foreach (var factionLocationSet in card.GetTargetLocations(source))
        {
            Formation formation = GetFormationByFaction(factionLocationSet.faction, source);
            for (int i = 0; i < factionLocationSet.Locations.Length; i++)
            {
                ICharacter character = formation.GetCharacters()[i];

                if (factionLocationSet.Locations[i] && character != null)
                    targets.Add(character);
            }
        }

        return targets;
    }

    private Formation GetFormationByFaction(EFaction faction, ICharacter source)
    {
        if (faction == EFaction.Ally)
            return source.GetFormation();

        if (source.GetFormation() == formations[0])
            return formations[1];
        else
            return formations[0];
    }

    public void EndTurn()
    {
        NextRound.Add(CurrentTurn);
        CurrentRound.Remove(CurrentTurn);

        if (CurrentRound.Count == 0)
            EndRound();

        OnUpdateTurnQueue.Invoke(roundQueues);
        OnEndTurn?.Invoke(turn, CurrentTurn);
        StartTurn();
    }

    public void RemoveCharacter(ICharacter character)
    {
        foreach (List<ICharacter> queue in roundQueues)
        {
            if (queue.Remove(character))
                break;
        }

        if (BattleResult != EBattleResult.None)
            EndBattle();
    }

    public void Surrender()
    {
    }

    #endregion

    #region Private
    private void SetupFormations(FormationConfig[] formationsConfig)
    {
        foreach (FormationConfig formationConfig in formationsConfig)
        {
            Formation formation = new Formation();
            formation.Setup(formationConfig.characters, formationConfig.faction);
            formations.Add(formation);
            CurrentRound.AddRange(formation.AliveCharacters);
        }
    }

    private async void StartTurn()
    {
        turn++;
        turnPhase = ETurnPhase.Start;

        while (turnPhase <= ETurnPhase.Play)
        {
            //if (CurrentTurn.IsAlive)

            OnStartTurnPhase?.Invoke(turn, turnPhase, CurrentTurn);
            await GetTurnPhase(turnPhase);
            turnPhase++;
        }
    }

    private void EndRound()
    {
        roundQueues.RemoveAt(0);
        roundQueues.Add(new());
        round++;
    }

    private void EndBattle()
    {
        OnEndBattle?.Invoke(BattleResult);
    }

    #endregion

    #region Turn phases

    private async Task StartPhase()
    {
    }

    private async Task ResolveEffectsPhase()
    {
        //OnResolveEffects?.Invoke(CurrentTurn);
        foreach (ModifierEffect stateEffect in CurrentTurn.GetModifierEffects())
        {
          await Task.Delay(1000);// Effect
        }
    }

    private async Task DrawCardsPhase()
    {
        for (int i = 0; i < 5; i++)
        {
            OnDrawRandomCard?.Invoke(CurrentTurn);
            await Task.Delay(250);
        }
    }

    private async Task PlayPhase()
    {
        //if (CurrentTurn as CharacterAI)
        //{
        //    (CurrentTurn as CharacterAI)?.PlayAI();
        //    return;
        //}
    }

    private async Task EndPhase()
    {
    }

    #endregion

    #region Getters

    private Task GetTurnPhase(ETurnPhase phase)
    {
        switch (phase)
        {
            case ETurnPhase.Start:
                return StartPhase();
            case ETurnPhase.Effects:
                return ResolveEffectsPhase();
            case ETurnPhase.Draw:
                return DrawCardsPhase();
            case ETurnPhase.Play:
                return PlayPhase();
        }

        return Task.Delay(0);
    }

    private EBattleResult GetBattleResult()
    {
        foreach (Formation formation in formations)
        {
            if (formation.AliveCharacters.Count == 0)
            {
                return formation.Faction == EFaction.Ally ? EBattleResult.Lose : EBattleResult.Win;
            }
        }

        return EBattleResult.None;
    }

    #endregion
}