using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface ICardView
{
    void Setup(Card card, RectTransform slot);
    void Draw();
    void Select();
    void Unselect();
    void Highlight();
    void Unhighlight();
    void Play();
}

public abstract class CardView : MonoBehaviour, ICardView
{
    protected ICard card;
    protected RectTransform slot;
    protected RectTransform rectTransform;

    public ICard Card => card;
    public RectTransform Slot => slot;

    protected abstract void OnSetup(Card card);
    protected abstract void PlayHighlightAnimation();
    protected abstract void PlayUnhighlightAnimation();
    protected abstract void PlaySelectAnimation();
    protected abstract void PlayUnselectAnimation();
    protected abstract void PlayConsumePlayAnimation();
    protected abstract void PlayDrawAnimation();

    protected Vector2 SlotSize = new Vector2(35f, 51f);
    protected Vector2 HighlightSlotSize = new Vector2(39f, 51f);
    protected Vector2 SelectedSlotSize = new Vector2(39f, 123f);

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void Setup(Card card, RectTransform slot)
    {
        this.card = card;
        this.slot = slot;

        OnSetup(card);
    }

    public void Draw()
    {
        PlayDrawAnimation();
    }

    public void Highlight()
    {
        PlayResizeSlotAnimation(HighlightSlotSize);
        PlayHighlightAnimation();
    }

    public void Unhighlight()
    {
        PlayResizeSlotAnimation(SlotSize);
        PlayUnhighlightAnimation();
    }

    public void Select()
    {
        PlayResizeSlotAnimation(SelectedSlotSize);
        PlaySelectAnimation();
    }

    public void Unselect()
    {
        PlayResizeSlotAnimation(SlotSize);
        PlayUnselectAnimation();
        PlayUnhighlightAnimation();
    }

    public void Play()
    {
        PlayConsumePlayAnimation();
    }

    private void OnMouseEnter()
    {
        Debug.Log(card);
        EventBus.Send(new EventPointerEnterCard() { cardView = this });
    }

    private void OnMouseExit()
    {
        Debug.Log(card);
        EventBus.Send(new EventPointerExitCard() { cardView = this });
    }

    private void OnMouseDown()
    {
        Debug.Log(card);
        EventBus.Send(new EventPointerClickCard() { cardView = this });
    }

    protected void PlayResizeSlotAnimation(Vector2 size, TweenCallback onComplete = null)
    {
        TweenCallback onUpdateResizeSlot = () => LayoutRebuilder.ForceRebuildLayoutImmediate(slot.parent.GetComponent<RectTransform>());
        onComplete += onUpdateResizeSlot;
        slot.DOSizeDelta(size, .1f).OnUpdate(onUpdateResizeSlot).OnComplete(onComplete);
    }
}

public class CardBattleView : CardView
{
    [SerializeField] private Image picture;
    [SerializeField] private Image border;
    [SerializeField] private Image selectedBorder;
    [SerializeField] private List<GameObject> manaPoints;
    [SerializeField] private List<GameObject> sourceLocations;
    [SerializeField] private List<GameObject> allyTargetLocations;
    [SerializeField] private List<GameObject> enemyTargetLocations;
    [SerializeField] private GameObject frontSide;
    [SerializeField] private GameObject backSide;

    protected override void OnSetup(Card card)
    {
        picture.sprite = card.GetConfig().picture;
        SetupSource(card);
        SetupTargets(card);
        SetupMana(card);
    }

    private void SetupSource(Card card)
    {
        for (int i = 0; i < sourceLocations.Count; i++)
        {
            sourceLocations[i].GetComponent<Image>().color = card.GetConfig().sourceLocations.Locations[i] ? Color.white : sourceLocations[i].GetComponent<Image>().color;
        }
    }

    private void SetupTargets(Card card)
    {
        foreach (FactionLocationSet targetLocationSet in card.GetConfig().targetLocations)
        {
            List<GameObject> holders = targetLocationSet.faction == EFaction.Ally ? allyTargetLocations : enemyTargetLocations;
            for ( int i = 0; i < targetLocationSet.Locations.Length; i++)
            {
                holders[i].GetComponent<Image>().color = targetLocationSet.Locations[i] ? Color.white : holders[i].GetComponent<Image>().color;
            }
        }
    }

    private void SetupMana(Card card)
    {
        for (int i = 0; i < card.GetConfig().manaCost; i++)
        {
            manaPoints[i].gameObject.SetActive(true);
        }
    }

    protected override void PlayDrawAnimation()
    {
        StartCoroutine(PlayFlipAnimation(true));
        rectTransform.DOMove(slot.position, .25f).OnComplete(() => rectTransform.SetParent(slot));
    }

    protected override void PlayHighlightAnimation()
    {
        border.gameObject.SetActive(false);
        selectedBorder.gameObject.SetActive(true);
    }

    protected override void PlayUnhighlightAnimation()
    {
        border.gameObject.SetActive(true);
        selectedBorder.gameObject.SetActive(false);
    }

    protected override void PlaySelectAnimation()
    {
        rectTransform.SetParent(slot.parent.parent, true);
    }

    protected override void PlayUnselectAnimation()
    {
        rectTransform.SetParent(slot, true);
    }

    protected override void PlayConsumePlayAnimation()
    {
        void OnComplete()
        {
            slot.gameObject.SetActive(false);
            gameObject.SetActive(false);
        }

        PlayResizeSlotAnimation(SlotSize, OnComplete);
    }

    public IEnumerator PlayFlipAnimation(bool fromBack)
    {
        rectTransform.localScale = new Vector3(-1f, 1f, 1f);
        rectTransform.transform.DOScale(Vector3.one, .5f);

        Flip(fromBack);
        yield return new WaitForSeconds(.25f);
        Flip(!fromBack);
    }

    private void Flip(bool fromBack)
    {
        backSide.SetActive(fromBack);
        frontSide.SetActive(!fromBack);
    }
}

public enum ECardViewState
{
    Normal, Highlight, Selected, Fade
}