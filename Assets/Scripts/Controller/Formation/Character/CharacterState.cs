using System.Collections.Generic;

public class CharacterState
{
    public int Level = 1;
    public int Experience = 0;
    public int LifeBase = 0;
    public int Life = 0;
    public int ManaBase = 0;
    public int Mana = 0;
    public List<ModifierEffect> Effects = new();


    public CharacterState(CharacterStats stats)
    {
        LifeBase = stats.strenght + stats.spirit + stats.mind;
        Life = LifeBase;
    }
}
