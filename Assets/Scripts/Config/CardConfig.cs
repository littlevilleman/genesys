using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardConfig", menuName = "Config/Card/CardConfig", order = 1)]
public class CardConfig : ScriptableObject
{
    [Header("Settings")]
    public string cardName;
    public string description;
    public int manaCost;

    [PreviewField(Height = 100)]
    public Sprite picture;

    [Header("Requirements")]
    [EnumToggleButtons, HideLabel]
    public ETargetType targetType;

    [LabelText("Source")]
    [ListDrawerSettings(DraggableItems = false, ShowIndexLabels = false, ShowPaging = false, ShowItemCount = false, HideRemoveButton = true, HideAddButton = true)]
    public FactionLocationSet sourceLocations = new FactionLocationSet { faction = EFaction.Ally };

    [ShowIf("targetType", ETargetType.TARGET)]
    [LabelText("Target")]
    [ListDrawerSettings(DraggableItems = false, ShowIndexLabels = false, ShowPaging = false, ShowItemCount = false, HideRemoveButton = true, HideAddButton = true)]
    public FactionLocationSet[] targetLocations = new FactionLocationSet[] { new FactionLocationSet() { faction = EFaction.Ally }, new FactionLocationSet() { faction = EFaction.Enemy} };

    public bool multiTarget;

    [Header("Actions")]
    public List<CardActionConfig> actions;

    public Card Build()
    {
        Card card = new Card();
        card.Setup(this);
        return card;
    }
}