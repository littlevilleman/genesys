using System.Threading.Tasks;

public class LifeModifierAction : CardAction<LifeModifierActionConfig>
{
    public override async Task Launch(ICharacter source, ICharacter target)
    {
        if (config.ActionEffect == EActionEffect.Damage)
            target.ReceiveDamage(source, config.value);
        else
            target.ReceiveHeal(source, config.value);

        await Task.Delay(100);
    }

    public override void Setup(LifeModifierActionConfig actionConfig)
    {
        config = actionConfig;
    }
}