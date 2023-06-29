using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterBattleView : MonoBehaviour
{
    [SerializeField] private Animator selectorAnimator;
    [SerializeField] private Image focusHolder;
    [SerializeField] private Image lifeBar;

    private CharacterViewBase character;

    private void Awake()
    {
        character = GetComponentInParent<CharacterViewBase>();
    }

    private void OnEnable()
    {
        EventBus.Register<EventStartBattle>(OnStartBattle);
        EventBus.Register<EventStartTurn>(OnTurnStart);
        EventBus.Register<EventFocusCharacter>(OnFocusCharacter);
        EventBus.Register<EventSelectCard>(OnSelectCard);
        EventBus.Register<EventPlayCardStart>(OnPlayCardStart);
        EventBus.Register<EventPlayCardEnd>(OnPlayCardEnd);
        EventBus.Register<EventReceiveDamage>(OnReceiveDamage);
        EventBus.Register<EventCharacterDie>(OnCharacterDie);
        EventBus.Register<EventUpdateRoundQueue>(OnUpdateTurnQueue);
    }

    private void OnMouseEnter()
    {
        EventBus.Send(new EventPointerEnterCharacter { characterView = character});
    }

    private void OnMouseExit()
    {
        EventBus.Send(new EventPointerExitCharacter { characterView = character });
    }

    private void OnMouseDown()
    {
        EventBus.Send(new EventPointerClickCharacter { character = character });
    }

    private void OnStartBattle(EventStartBattle context)
    {
        //lifeBar.fillAmount = character.Character.State.Life * 1f / character.Character.MaxLife;
    }

    private void OnCharacterDie(EventCharacterDie context)
    {
        if (character.Character != context.character)
            return;

        gameObject.SetActive(false);
    }

    private void OnUpdateTurnQueue(EventUpdateRoundQueue context)
    {
        string trigger = character.Character == context.rounds[0][0] ? "Current" : "Hidden";
        selectorAnimator.SetTrigger(trigger);
    }

    private void OnReceiveDamage(EventReceiveDamage context)
    {
        if (context.target != character.Character)
            return;

        float value = context.target.GetState().Life * 1f / context.target.GetMaxLife();
        lifeBar.DOFillAmount(value, .25f).SetEase(Ease.OutQuint);
    }

    private void OnTurnStart(EventStartTurn context)
    {
        string trigger = character.Character == context.character ? "Current" : "Hidden";
        selectorAnimator.SetTrigger(trigger);
    }

    private void OnPlayCardStart(EventPlayCardStart context)
    {
        selectorAnimator.SetTrigger("Hidden");
    }

    private void OnPlayCardEnd(EventPlayCardEnd context)
    {
        string trigger = character.Character == context.source ? "Current" : "Hidden";
        selectorAnimator.SetTrigger(trigger);
    }

    private void OnSelectCard(EventSelectCard context)
    {
        //if (context.result != EventSelectCardResult.Result.Success)
        //    return;

    }

    private void OnFocusCharacter(EventFocusCharacter context)
    {
        focusHolder.gameObject.SetActive(context.availableTargets.Contains(character.Character));

        if (context.targets.Contains(character.Character))
            focusHolder.color = Color.green;
        else
            focusHolder.color = Color.gray;
    }

    private void OnDisable()
    {
        EventBus.Unregister<EventStartBattle>(OnStartBattle);
        EventBus.Unregister<EventStartTurn>(OnTurnStart);
        EventBus.Unregister<EventFocusCharacter>(OnFocusCharacter);
        EventBus.Unregister<EventSelectCard>(OnSelectCard);
        EventBus.Unregister<EventPlayCardStart>(OnPlayCardStart);
        EventBus.Unregister<EventPlayCardEnd>(OnPlayCardEnd);
        EventBus.Unregister<EventReceiveDamage>(OnReceiveDamage);
        EventBus.Unregister<EventCharacterDie>(OnCharacterDie);
        EventBus.Unregister<EventUpdateRoundQueue>(OnUpdateTurnQueue);
    }
}