using UnityEngine;

[CreateAssetMenu(fileName = "CardStateConfig", menuName = "Config/UI/CardStateConfig", order = 1)]
public class CardStateConfig : ScriptableObject
{
    public ECardViewState state;
    public Color color;
}