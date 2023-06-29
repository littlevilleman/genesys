using UnityEngine;

public abstract class CardActionConfig : ScriptableObject
{
    public abstract ICardAction BuildAction();
}

public enum EActionEffect
{
    Heal, Damage
}

public interface ICardActionConfig
{

}