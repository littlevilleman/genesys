using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandView : MonoBehaviour
{
    [SerializeField] private List<CardBattleView> cards = new ();
    [SerializeField] private CardViewPool cardViewPool;

    private HorizontalLayoutGroup layout;
    private RectTransform rectTransform;

    private CardView selectedCard;
    private ICharacter turnOwner;

    private void Awake()
    {
        layout = GetComponent<HorizontalLayoutGroup>();
        rectTransform = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        EventBus.Register<EventPointerEnterCard>(OnPointerEnterCard);
        EventBus.Register<EventPointerExitCard>(OnPointerExitCard);
        EventBus.Register<EventPointerClickCard>(OnPointerClickCard);
        EventBus.Register<EventPointerEnterCharacter>(OnPointerEnterCharacter);
        EventBus.Register<EventPointerExitCharacter>(OnPointerExitCharacter);
        EventBus.Register<EventPointerClickCharacter>(OnPointerClickCharacter);
        EventBus.Register<EventStartTurnPhase>(OnStartTurnPhase);
        EventBus.Register<EventDrawCard>(OnDrawCard);
        EventBus.Register<EventPlayCardStart>(OnPlayCardStart);
        EventBus.Register<EventPlayCardEnd>(OnPlayCardEnd);
        EventBus.Register<EventEndTurn>(OnTurnOver);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            var hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.forward);

            if (hit.transform?.GetComponent<CardBattleView>())
                return;

            if (hit.transform?.GetComponent<CharacterView>())
                return;

            OnPointerClickCard(new EventPointerClickCard() { cardView = null });
            OnPointerExitCharacter(new EventPointerExitCharacter() { characterView = null });
        }
    }

    private void OnStartTurnPhase(EventStartTurnPhase context)
    {
        turnOwner = context.character;
    }

    private void OnTurnOver(EventEndTurn context)
    {
        Clear();
    }

    private void OnDrawCard(EventDrawCard context)
    {
        RectTransform slot = GetEmptySlot();

        if (slot)
        {
            CardBattleView cardView = cardViewPool.Pull();
            cards.Add(cardView);
            cardView.Setup(context.card as Card, slot);
            cardView.Draw();

            layout.padding.left = cards.Count % 2 == 0 ? 1 : 0;
        }
    }

    private void OnPlayCardStart(EventPlayCardStart context)
    {
        if (context.card != selectedCard.Card)
            return;

        selectedCard.Play();
    }

    private void OnPlayCardEnd(EventPlayCardEnd context)
    {
        OnPointerClickCard(new EventPointerClickCard() { cardView = null });

        EventBus.Send(new EventFocusCharacter ());
    }

    private void OnPointerEnterCard(EventPointerEnterCard context)
    {
        if (selectedCard != null)
            return;

        context.cardView.Highlight();
        EventBus.Send(new EventUpdatePointer { state = EPointerState.Hidden });
    }

    private void OnPointerExitCard(EventPointerExitCard context)
    {
        if (selectedCard != null)
            return;

        context.cardView.Unhighlight();
        EventBus.Send(new EventUpdatePointer { state = EPointerState.Default });
    }

    private void OnPointerClickCard(EventPointerClickCard context)
    {
        if (context.cardView != null && context.cardView.Card.GetManaCost() < turnOwner.GetState().Mana)
            return;

        selectedCard?.Unselect();
        selectedCard = context.cardView;
        selectedCard?.Select();
        OnSelectCard();
    }

    private void OnSelectCard()
    {
        if (selectedCard == null)
            rectTransform.DOAnchorPosY(8f, .25f);
        else
            rectTransform.DOAnchorPosY(-16f, .25f);

        List<ICharacter> availableTargets = BattleManager.Instance.GetAvailableTargets(selectedCard?.Card, turnOwner);

        EventBus.Send(new EventFocusCharacter { availableTargets = availableTargets });
        EventBus.Send(new EventUpdatePointer { state = selectedCard ? EPointerState.CardSelect : EPointerState.Default });
    }

    private void OnPointerEnterCharacter(EventPointerEnterCharacter context)
    {
        if (context.characterView == null)
            return;

        List<ICharacter> availableTargets = BattleManager.Instance.GetAvailableTargets(selectedCard?.Card, turnOwner);
        List<ICharacter> targets = BattleManager.Instance.GetTargets(selectedCard?.Card, turnOwner, context.characterView.Character);

        EventBus.Send(new EventFocusCharacter { availableTargets = availableTargets, targets = targets});
        EventBus.Send(new EventUpdatePointer { state = selectedCard ? EPointerState.Hidden : EPointerState.Default });
    }

    private void OnPointerExitCharacter(EventPointerExitCharacter context)
    {
        if (context.characterView == null)
            return;

        List<ICharacter> availableTargets = BattleManager.Instance.GetAvailableTargets(selectedCard?.Card, turnOwner);

        EventBus.Send(new EventFocusCharacter { availableTargets = availableTargets });
        EventBus.Send(new EventUpdatePointer { state = selectedCard ? EPointerState.CardSelect : EPointerState.Default });
    }

    private void OnPointerClickCharacter(EventPointerClickCharacter context)
    {
        if (context.character == null)
            return;

        if (selectedCard == null)
            return;

        EventBus.Send(new EventTryPlayCard { card = selectedCard.Card, source = turnOwner, target = context.character.Character });
    }

    private RectTransform GetEmptySlot()
    {
        foreach (RectTransform slot in transform)
        {
            if (!slot.gameObject.activeInHierarchy)
            {
                slot.gameObject.SetActive(true);
                LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
                return slot;
            }
        }

        return null;
    }

    private void Clear()
    {
        foreach (CardBattleView card in cards)
        {
            card.transform.parent.gameObject.SetActive(false);
            cardViewPool.Recycle(card);
        }

        cards.Clear();
    }

    private void OnDisable()
    {
        EventBus.Unregister<EventPointerEnterCard>(OnPointerEnterCard);
        EventBus.Unregister<EventPointerExitCard>(OnPointerExitCard);
        EventBus.Unregister<EventPointerClickCard>(OnPointerClickCard);
        EventBus.Unregister<EventPointerEnterCharacter>(OnPointerEnterCharacter);
        EventBus.Unregister<EventPointerExitCharacter>(OnPointerExitCharacter);
        EventBus.Unregister<EventPointerClickCharacter>(OnPointerClickCharacter);
        EventBus.Unregister<EventStartTurnPhase>(OnStartTurnPhase);
        EventBus.Unregister<EventDrawCard>(OnDrawCard);
        EventBus.Unregister<EventPlayCardStart>(OnPlayCardStart);
        EventBus.Unregister<EventPlayCardEnd>(OnPlayCardEnd);
        EventBus.Unregister<EventEndTurn>(OnTurnOver);
    }
}