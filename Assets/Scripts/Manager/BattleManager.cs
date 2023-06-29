using System.Collections;
using System.Collections.Generic;
using UnityEngine; 

public class BattleManager : MonoBehaviour
{
    private Battle battle;
    private static BattleManager instance;
    public static BattleManager Instance => instance;

    private void Awake()
    {
        instance = this;
    }

    public void OnEnable()
    {
        EventBus.Register<EventTryEndTurn>(OnTryEndTurn);
        EventBus.Register<EventTryPlayCard>(OnTryPlayCard);
        EventBus.Register<EventCharacterDie>(OnCharacterDie);
    }

    public void SetupBattle(BattleConfig battleConfig)
    {
        Debug.Log($"BATTLE - Setup battle [{battleConfig}]");

        battle = new Battle(battleConfig);

        battle.OnStartBattle+= StartBattle;
        battle.OnEndBattle += EndBattle;
        battle.OnStartTurnPhase += StartTurnPhase;
        battle.OnPlayCardStart += PlayCardStart;
        battle.OnPlayCardEnd += PlayCardEnd;
        battle.OnDrawRandomCard += DrawRandomCard;
        battle.OnUpdateTurnQueue += UpdateTurnQueue;
        battle.OnEndTurn += EndTurn;
    }

    public void LaunchBattle(float delay)
    {
        StartCoroutine(StartBattleDelay(delay));
    }

    private void StartBattle(List<List<ICharacter>> roundQueues)
    {
        Debug.Log($"BATTLE - Start battle");
        EventBus.Send(new EventStartBattle { rounds = roundQueues });
    }

    private void EndBattle(EBattleResult result)
    {
        Debug.Log($"BATTLE - End battle");
        EventBus.Send(new EventEndBattle() { result = result });
    }

    private void StartTurnPhase(int turn, ETurnPhase turnPhase, ICharacter character)
    {
        Debug.Log($"BATTLE - Start turn phase - [{turnPhase}]");
        EventBus.Send(new EventStartTurnPhase() { turn = turn, phase = turnPhase, character = character });
    }

    private void UpdateTurnQueue(List<List<ICharacter>> roundQueues)
    {
        Debug.Log($"BATTLE - Update turn queue [{roundQueues}]");
        EventBus.Send(new EventUpdateRoundQueue { rounds = roundQueues });
    }

    private void DrawRandomCard(ICharacter character)
    {
        Debug.Log($"BATTLE - Draw random card");
        EventBus.Send(new EventDrawCard() { card = DeckManager.Instance.DrawRandomCard(character) });
    }

    private void PlayCardStart(ICard card, ICharacter source, ICharacter focus)
    {
        Debug.Log($"BATTLE - Play card start - [{card}] [{source}] [{focus}]");

        EventBus.Send(new EventPlayCardStart { card = card, focus = focus, source = source });
    }

    private void PlayCardEnd(ICard card, ICharacter source, ICharacter focus)
    {
        Debug.Log($"BATTLE - Play card end - [{card}] [{source}] [{focus}]");

        EventBus.Send(new EventPlayCardEnd { card = card, focus = focus, source = source });
    }

    private void EndTurn(int turn, ICharacter character)
    {
        Debug.Log($"BATTLE - End turn");
        EventBus.Send(new EventEndTurn() { character = character, turn = turn });
    }

    private IEnumerator StartBattleDelay(float delay = 0f)
    {
        yield return new WaitForSeconds(delay);
        battle.StartBattle();
    }

    private async void OnTryPlayCard(EventTryPlayCard context)
    {
        Debug.Log($"BATTLE - Try play card - [{context.card}] [{context.source}] [{context.target}]");

        if (!GetAvailableTargets(context.card, context.source).Contains(context.target))
            return;

        await battle.PlayCard(context.card, context.source, context.target);
    }

    private void OnCharacterDie(EventCharacterDie context)
    {
        Debug.Log($"BATTLE - Character defeat - [{context.character}]");

        battle.RemoveCharacter(context.character);
    }

    private void OnTryEndTurn(EventTryEndTurn context)
    {
        Debug.Log("BATTLE - Try end turn");
        battle.EndTurn();
    }

    public List<ICharacter> GetAvailableTargets(ICard card, ICharacter source)
    {
        if (card == null)
            return new List<ICharacter>();

        return battle.GetAvailableTargets(card, source);
    }

    public List<ICharacter> GetTargets(ICard card, ICharacter source, ICharacter focus)
    {
        if (card == null || focus == null)
            return new List<ICharacter>();
            
        if (!card.GetConfig().multiTarget)
            return new List<ICharacter> { focus };

        return battle.GetAvailableTargets(card, source);
    }

    private void OnDisable()
    {
        EventBus.Unregister<EventTryEndTurn>(OnTryEndTurn);
        EventBus.Unregister<EventTryPlayCard>(OnTryPlayCard);
        EventBus.Unregister<EventCharacterDie>(OnCharacterDie);
    }
}

public enum ETurnPhase
{
    Start, Effects, Draw, Play, End
}

public enum EBattleResult
{
    None, Lose, Win
}