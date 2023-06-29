using UnityEngine;

[CreateAssetMenu(fileName = "FormationConfig", menuName = "Config/FormationConfig", order = 1)]
public class FormationConfig : ScriptableObject
{
    public EFaction faction;
    public CharacterConfig[] characters;
}
