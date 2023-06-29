using System;
using UnityEngine;

[CreateAssetMenu(fileName = "LifeModifierActionConfig", menuName = "Config/Skill/Action/LifeModifierActionConfig", order = 1)]
public class LifeModifierActionConfig : CardActionConfig
{
    [Header("Life Modifier")]
    public EActionEffect ActionEffect = EActionEffect.Damage;
    public int value;
    public int hits = 1;
    public int valueHitFactor = 1;
    
    public override ICardAction BuildAction()
    {
        return new LifeModifierAction() { config = this };
    }
}